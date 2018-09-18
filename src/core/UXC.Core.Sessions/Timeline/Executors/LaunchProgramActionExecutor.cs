using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXC.Sessions.Common.Helpers;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Executors
{

    /*
        Launch A - keep running - completes immediately
        ... any other steps ...
        Close A

        Launch A - shows A, close on complete or complete on close
     
        Launch A - keep running, no tag
        Launch B - close immediately, no tag
        B closed - should not close A, even if both have no tag specified.


        Launch A - keep running, no tag
        Launch B - keep running, no tag
        Close - no tag - close A and B
    */
    

    public class LaunchProgramActionExecutor : SessionStepActionExecutor<LaunchProgramActionSettings>
    {
        private readonly IProcessService _service;

        private Process _process;
        private LaunchProgramActionSettings _settings;

        public LaunchProgramActionExecutor(IProcessService service)
        {
            _service = service;
        }

        // TODO add logging error and output
        //private readonly StringBuilder _outputData = new StringBuilder();
        //private readonly StringBuilder _errorData = new StringBuilder();

        protected override void Execute(SessionRecording recording, LaunchProgramActionSettings settings)
        {
            _settings = settings;
            _process = CreateProcess(settings, recording);

            if (settings.KeepRunning == false)
            {
                _process.EnableRaisingEvents = true;
                _process.Exited += process_Exited;
            }

            if (_process.Start())
            {
                if (settings.WaitForStart)
                {
                    if (_settings.WaitTimeout.HasValue)
                    {
                        _process.WaitForInputIdle((int)_settings.WaitTimeout.Value.TotalMilliseconds);
                        //_process.WaitForInputIdle();
                        //Thread.Sleep((int)_settings.WaitTimeout.Value.TotalMilliseconds);
                    }
                    else
                    {
                        _process.WaitForInputIdle();
                    }
                }

                if (settings.KeepRunning)
                {
                    _service.Add(_process.Id, settings.Tag);
                    _process.Dispose();
                    _process = null;

                    OnCompleted(SessionStepResult.Successful);
                }
            }
            else
            {
                // TODO show or log that the process did not start
                OnCompleted(SessionStepResult.Failed);
            }
        }

        private void process_Exited(object sender, EventArgs e)
        {
            Complete();
        }


        private static Process CreateProcess(LaunchProgramActionSettings settings, SessionRecording recording)
        {
            var process = new Process();

            string path = Path.Combine(settings.Path);
            string directory = settings.WorkingDirectoryPath ?? Path.GetDirectoryName(settings.Path);

            string arguments = settings.Arguments ?? String.Empty;
            if (settings.ArgumentsParameters != null && settings.ArgumentsParameters.Any())
            {
                arguments = SessionRecordingSettingsHelper.FillParameters(arguments, settings.ArgumentsParameters, recording.Settings);
            }

            process.StartInfo = new ProcessStartInfo(settings.Path, arguments)
            {
                WorkingDirectory = directory,
                UseShellExecute = true,
                RedirectStandardError = false,
                RedirectStandardOutput = false
            };

            if (settings.RunInBackground)
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            else
            {
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            }

            return process;
        }


        //private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    _outputData.Append(e.Data);
        //}

        //private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    _errorData.Append(e.Data);
        //}
        

        public override SessionStepResult Complete()
        {
            var process = ObjectEx.GetAndReplace(ref _process, null);

            try
            {
                if (process != null && _settings.KeepRunning == false)
                {
                    process.Exited -= process_Exited;

                    _service.CloseProcess(process, _settings.ForceClose);

                    process.Dispose();
                }
            }
            catch (Exception)
            {
                // TODO log exception
            }

            return base.Complete();
        }
    }
} 
