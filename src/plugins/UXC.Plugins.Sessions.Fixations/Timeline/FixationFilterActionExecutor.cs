/**
 * UXC.Plugins.Sessions.Fixations
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXC.Core.Data.Conversion.GazeToolkit;
using UXC.Core.Data.Serialization;
using UXC.Sessions.Recording.Local;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;
using UXI.GazeToolkit.Interpolation;
using UXI.GazeToolkit.Selection;
using UXI.GazeToolkit.Smoothing;
using UXI.GazeToolkit.Fixations;
using UXI.GazeToolkit.Fixations.VelocityThreshold;

namespace UXC.Sessions.Timeline.Executors
{
    class FixationFilterActionExecutor : SessionStepActionExecutor<FixationFilterActionSettings>
    {
        private readonly List<IDataSerializationFactory> _serializationFactories;

        private IDisposable _subscription = null;

        public FixationFilterActionExecutor(IEnumerable<IDataSerializationFactory> serializationFactories)
        {
            _serializationFactories = serializationFactories.ToList();

        }

        protected override void Execute(SessionRecording recording, FixationFilterActionSettings settings)
        {
            var result = recording.Results.OfType<LocalSessionRecordingResult>().FirstOrDefault();
            var path = result?.Paths.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p).Equals("ET_data", StringComparison.InvariantCultureIgnoreCase));

            if (path != null)
            {
                string extension = Path.GetExtension(path).TrimStart('.');
                var serializationFactory = GetSerializationFactory(extension);
                var data = ReadInputData(path, serializationFactory);

               var fixations = data.Select(gd => gd.ToToolkit())
                                   .FillInGaps(TimeSpan.FromMilliseconds(75))
                                   .SelectEye(EyeSelectionStrategy.Average)
                                   .ReduceNoise(new ExponentialSmoothingFilter(0.3))
                                   .CalculateVelocities(TimeSpan.FromMilliseconds(20), 60)
                                   .ClassifyMovements(30)
                                   .MergeAdjacentFixations(TimeSpan.FromMilliseconds(75), 0.5)
                                   .DiscardShortFixations(TimeSpan.FromMilliseconds(60));

                string filename = "ET_fixations_post.json";
                string filepath = Path.Combine(Path.GetDirectoryName(path), filename);

                _subscription = WriteOutput(fixations, filepath, serializationFactory)
                    .Subscribe
                    (
                        onNext: _ => { },
                        onError: ex => OnCompleted(new SessionStepExceptionResult(ex)),
                        onCompleted: () => { result.Paths.Add(filepath); OnCompleted(new FixationFilterResult(filename)); }
                    );
            }
            else
            {
                OnCompleted(SessionStepResult.Skipped);
            }
        }


        protected override void OnCompleted(SessionStepResult result)
        {
            base.OnCompleted(result);

            _subscription?.Dispose();
        }


        private IDataSerializationFactory GetSerializationFactory(string extension)
        {
            return _serializationFactories.FirstOrDefault(f => f.FileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                                          .ThrowIfNull(() => new ArgumentOutOfRangeException(nameof(extension)));
        }



        private static IObservable<GazeData> ReadInputData(string filePath, IDataSerializationFactory serializationFactory)
        {
            return Observable.Create<GazeData>(observer =>
            {
                CancellationDisposable cancel = new CancellationDisposable();
                Task.Run(() =>
                {
                    try
                    {
                        using (var file = File.OpenText(filePath))
                        {
                            using (var reader = serializationFactory.CreateReaderForType(file, typeof(GazeData)))
                            {
                                foreach (var data in reader.ReadAll<GazeData>())
                                {
                                    observer.OnNext(data);
                                }
                            }
                        }
                        observer.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                    }
                }, cancel.Token);

                return cancel;
            }).Publish().RefCount();
        }


        private IObservable<T> WriteOutput<T>(IObservable<T> data, string path, IDataSerializationFactory serializationFactory)
        {
            TextWriter outputWriter;
            FileStream fileStream = null;
      
            fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            outputWriter = new StreamWriter(fileStream, new UTF8Encoding(false));

            var dataWriter = serializationFactory.CreateWriterForType(outputWriter, typeof(T));

            return data.Do(d => dataWriter.Write(d)).Finally(() => 
            {
                dataWriter.Close();
                dataWriter.Dispose();
                outputWriter.Dispose();
                fileStream?.Dispose();
            });
        }
    }
}
                   
