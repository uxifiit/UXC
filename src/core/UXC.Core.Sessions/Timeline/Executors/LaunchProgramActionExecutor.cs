using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Executors
{
    public class LaunchProgramActionExecutor : SessionStepActionExecutor<LaunchProgramActionSettings>
    {
        private Process _process;

        // add logging error and output
        //private readonly StringBuilder _outputData = new StringBuilder();
        //private readonly StringBuilder _errorData = new StringBuilder();

        protected override void Execute(SessionRecording recording, LaunchProgramActionSettings settings)
        {
            _process = CreateProcess(settings, recording);
            _process.EnableRaisingEvents = true;
            _process.Exited += process_Exited;
            
                //Observable.Create(o =>
            //{
            //    DataReceivedEventHandler handler = (e, d) =>
            //    {

            //    };

            //    return new CompositeDisposable
            //    (
            //        Disposable.Create(() => { _process.ErrorDataReceived -= handler; }),
            //        Disposable.Create(() => { _process.})
            //    );
            //});

            //if (settings.RunOnBackground)
            //{
            //    _process.ErrorDataReceived += Process_ErrorDataReceived;
            //    _process.OutputDataReceived += Process_OutputDataReceived;
            //    _process.BeginOutputReadLine();
            //    _process.BeginErrorReadLine();
            //}

            _process.Start();
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
                arguments = InsertArgumentsParameters(arguments, settings.ArgumentsParameters, recording);
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

        private static string InsertArgumentsParameters(string arguments, List<string> parameters, SessionRecording recording)
        {
            object recordingSetting;

            foreach (var parameter in parameters)
            {
                string value = String.Empty;
                if (recording.Settings.TryGetSetting(parameter, out recordingSetting))
                {
                    value = Convert.ToString(recordingSetting);
                }

                arguments = arguments.Replace($"{{{parameter}}}", value);
            }

            return arguments;
        }

        public override SessionStepResult Complete()
        {
            var process = ObjectEx.GetAndReplace(ref _process, null);

            KillProcess(process);

            return base.Complete();
        }


        private void KillProcess(Process process)
        {
            if (process != null && process.HasExited == false)
            {
                process.EnableRaisingEvents = false;
                
                //process.OutputDataReceived -= Process_OutputDataReceived;
                //process.ErrorDataReceived -= Process_ErrorDataReceived;

                process.Exited -= process_Exited;
                process.Kill();
                process.Dispose();
            }
        }
    }
}
