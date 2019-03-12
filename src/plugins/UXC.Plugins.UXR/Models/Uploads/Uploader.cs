/**
 * UXC.Plugins.UXR
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXC.Plugins.UXR.Helpers;
using UXI.Common;
using UXI.Common.Extensions;
using UXI.Common.Helpers;
using UXI.ZIP;
using UXR.Studies.Client;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public class Uploader : DisposableBase, IUploader
    {
        private readonly UploadsQueue _queue;
        private readonly IUXRClient _uxrClient;
        private readonly IUXRNodeContext _uxrNode;
        private readonly SerialDisposable _runningUpload = new SerialDisposable();
        private readonly Stepper<TimeSpan> _timeoutStepper = new Stepper<TimeSpan>(TimeSpan.FromSeconds(5), current => TimeSpan.FromSeconds(Math.Min(current.TotalSeconds * 2, 120)));

        private const string SESSION_FOLDER_TIMESTAMP_FORMAT = "yyyy-MM-dd_HHmmss";

        private static readonly TimeSpan WaitTimeBeforeUpload = TimeSpan.FromSeconds(30);

        public Uploader(UploadsQueue queue, IUXRClient uxrClient, IUXRNodeContext uxrNode)
        {
            _queue = queue;
            _uxrClient = uxrClient;
            _uxrNode = uxrNode;

            _queue.UploadsChanged += queue_UploadsChanged;
        }

        private void ResetUpload()
        {
            _runningUpload.Disposable = Disposable.Empty;
            CurrentUpload = null;
        }

        private void queue_UploadsChanged(object sender, CollectionChangedEventArgs<Upload> args)
        {
            if (args.AddedItems.Any()
                && IsEnabled
                && IsWorking == false)
            {
                StartUpload(true);
            }
        }


        private bool isEnabled = false;
        public bool IsEnabled => isEnabled;

        public event EventHandler<bool> IsEnabledChanged;


        private bool isWorking = false;
        public bool IsWorking
        {
            get
            {
                return isWorking;
            }
            private set
            {
                bool wasWorking = ObjectEx.GetAndReplace(ref isWorking, value);
                if (wasWorking != value)
                {
                    IsWorkingChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> IsWorkingChanged;


        private Upload currentUpload;
        public Upload CurrentUpload
        {
            get { return currentUpload; }
            private set
            {
                var previousUpload = ObjectEx.GetAndReplace(ref currentUpload, value);
                if (previousUpload != value)
                {
                    CurrentUploadChanged?.Invoke(this, value);
                }
            }
        }
        public event EventHandler<Upload> CurrentUploadChanged;


        private void StartUpload(bool wait)
        {
            Task.Run(() => RunUploadsAsync(wait).Forget()).Forget();
        }

        // add enable, because auto start does not wait before upload - same call like from UI
        public void Start()
        {
            bool wasEnabled = ObjectEx.GetAndReplace(ref isEnabled, true);
            if (wasEnabled == false || IsWorking == false)
            {
                IsEnabledChanged?.Invoke(this, true);
                _timeoutStepper.Reset();

                StartUpload(false);
            }
        }


        public void Stop()
        {
            ResetUpload();

            bool wasEnabled = ObjectEx.GetAndReplace(ref isEnabled, false);
            if (wasEnabled)
            {
                IsEnabledChanged?.Invoke(this, false);
            }
        }


        private async Task RunUploadsAsync(bool wait)
        {
            IsWorking = true;

            try
            {
                CancellationDisposable cancellation = new CancellationDisposable();
                _runningUpload.Disposable = cancellation;

                HashSet<string> paths = new HashSet<string>();

                if (_queue.Uploads.Any() && wait)
                {
                    await Task.Delay(WaitTimeBeforeUpload, cancellation.Token);
                }

                Upload upload = null;
                while (cancellation.Token.IsCancellationRequested == false
                        && _queue.TryPeek(out upload)
                        && upload != null
                        && paths.Contains(upload.Recording.Path) == false)
                {
                    CurrentUpload = upload;
                    paths.Add(upload.Recording.Path);

                    try
                    {
                        var progress = new Progress<UploadStatus>(upload.UpdateStatus);
                        using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellation.Token))
                        {
                            await UploadRecordingDataAsync(upload.Recording, progress, cts.Token);
                        }
                    }
                    catch (Exception ex)
                    {
                        // log
                    }
                    // TODO what if failed?

                    _queue.TryDequeue(out upload);
                    if (upload.Status.Step != UploadStep.Completed)
                    {
                        _queue.TryEnqueue(upload.Recording);
                        await Task.Delay(_timeoutStepper.Current, cancellation.Token);

                        _timeoutStepper.MoveNext();
                    }
                    else
                    {
                        _timeoutStepper.Reset();
                    }
                }

                ResetUpload();
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                IsWorking = false;
            }
        }


        private static string PrepareDirectoryForUploadSegments(SessionRecordingData recording)
        {
            string sessionIdentifier = Path.GetFileName(recording.Path);
            //string sessionIdentifier = $"{recording.StartTime.ToString(SESSION_FOLDER_TIMESTAMP_FORMAT)} {recording.SessionName}";

            string sessionDirectoryName = FilenameHelper.ReplaceInvalidFileNameChars(sessionIdentifier);

            string sessionDirectory = Path.Combine(Directories.UploadFolderPath, sessionDirectoryName);

            if (Directory.Exists(sessionDirectory))
            {
                Directory.Delete(sessionDirectory, true);
            }

            Directories.EnsureDirectoryExists(sessionDirectory);

            return sessionDirectory;
        }


        private static async Task<IEnumerable<string>> PrepareUploadSegmentsAsync(SessionRecordingData recording, string targetDirectoryPath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            string uploadFileName = Path.Combine(targetDirectoryPath, "data.zip");

            var segments = await Task.Run(() => ZipHelper.Pack(uploadFileName, recording.Path, 2 * 1024 * 1024, progress, cancellationToken));

            return segments.Select(segment => Path.Combine(targetDirectoryPath, segment)).ToList();
        }


        private async Task<bool> UploadMultipleSegmentsAsync(IEnumerable<string> segments, SessionRecordingData recording, IProgress<int> progress, CancellationToken cancellationToken)
        {
            Queue<string> segmentsToUpload = new Queue<string>(segments);

            bool canUploadSegment = true;
            double segmentProgressPart = (100d / segmentsToUpload.Count);
            double finishedProgress = 0d;

            while (segmentsToUpload.Any()
                   && canUploadSegment
                   && cancellationToken.IsCancellationRequested == false)
            {
                int attempts = 0;
                bool uploaded = false;

                string segment = segmentsToUpload.Peek();

                var segmentProgress = new Progress<int>(i => progress.Report((int)Math.Min(100, finishedProgress + (i / 100d) * segmentProgressPart)));

                while (uploaded == false
                       && attempts < 3
                       && cancellationToken.IsCancellationRequested == false)
                {
                    attempts += 1;

                    uploaded = await UploadSegmentAsync(segment, recording, segmentProgress, cancellationToken);
                }

                if (uploaded)
                {
                    segmentsToUpload.Dequeue();
                }

                canUploadSegment = uploaded;

                finishedProgress += segmentProgressPart;
            }

            return segmentsToUpload.Any() == false;
        }


        private async Task<bool> UploadSegmentAsync(string segmentPath, SessionRecordingData recording, IProgress<int> progress, CancellationToken cancellationToken)
        {
            try
            {
                using (var file = File.OpenRead(segmentPath))
                {
                    bool uploaded = await _uxrClient.UploadSessionRecordingFileAsync(_uxrNode.NodeId, recording.StartTime, file, progress, cancellationToken);
                    return uploaded;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return false;
        }


        // do not use, we do not know any checksum or filesize, so the files may be incomplete
        //private async Task<IEnumerable<string>> FilterAlreadyUploadedSegmentsAsync(IEnumerable<string> segments, SessionRecordingData recording)
        //{
        //    var uploadedFiles = await _uxrClient.GetUploadedSessionRecordingFilesAsync(_uxrNode.NodeId, recording.StartTime);
        //    return segments.Where(s => uploadedFiles.Contains(Path.GetFileName(s)) == false).ToList();
        //}


        private async Task<bool> VerifyAllSegmentsAreUploadedAsync(IEnumerable<string> segments, SessionRecordingData recording)
        {
            var uploadedFiles = await _uxrClient.GetUploadedSessionRecordingFilesAsync(_uxrNode.NodeId, recording.StartTime);
            return uploadedFiles.All(upload => segments.Any(s => s.EndsWith(upload)));
        }


        private async Task UploadRecordingDataAsync(SessionRecordingData recording, IProgress<UploadStatus> progress, CancellationToken cancellationToken)
        {
            CompositeProgress<UploadStatus, int> progressStatus = new CompositeProgress<UploadStatus, int>(progress, UploadStatus.CreatePreparing(), (i, p) => p.Update(i));

            string uploadSegmentsDirectoryPath = PrepareDirectoryForUploadSegments(recording);

            var segments = await PrepareUploadSegmentsAsync(recording, uploadSegmentsDirectoryPath, progressStatus, cancellationToken);

            if (segments.Any())
            {
                //segments = await FilterAlreadyUploadedSegmentsAsync(segments, recording);

                // upload all segments
                progressStatus.Report(UploadStatus.CreateUploading());
                bool uploaded = await UploadMultipleSegmentsAsync(segments, recording, progressStatus, cancellationToken);

                if (uploaded)
                {
                    // verify if all segments are uploaded
                    progressStatus.Report(UploadStatus.CreateVerifying());
                    bool verified = await VerifyAllSegmentsAreUploadedAsync(segments, recording);

                    if (verified)
                    {   // if all segments are uploaded
                        // commit the upload
                        progressStatus.Report(UploadStatus.CreateCompleting());
                        bool saved = await _uxrClient.SaveSessionRecordingAsync(_uxrNode.NodeId, recording.StartTime, recording.SessionId);

                        if (saved)
                        {
                            progressStatus.Report(UploadStatus.CreateCompleted());

                            // delete upload files                              
                            Directory.Delete(uploadSegmentsDirectoryPath, true);

                            //if (recording.DeleteData)
                            //{
                            //    // TODO Uploader: delete recording data?
                            //    // Directory.Delete(recording.Path, true);
                            //}
                        }
                    }
                }
            }
        }

        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    // ..
                    _runningUpload.Dispose();
                    _queue.UploadsChanged -= queue_UploadsChanged;
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
