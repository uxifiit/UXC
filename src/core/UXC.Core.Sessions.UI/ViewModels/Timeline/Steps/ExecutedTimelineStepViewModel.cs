using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UXC.Core.ViewModels;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Executors;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public class ExecutedTimelineStepViewModel : BindableBase, ITimelineStepViewModel
    {
        private readonly ISessionStepActionExecutor _executor;
        private readonly ExecutedActionSettingsBase _settings;
        private readonly ViewModelResolver _resolver;

        public ExecutedTimelineStepViewModel(ExecutedActionSettingsBase settings, ISessionStepActionExecutor executor, ViewModelResolver resolver)
        {
            executor.ThrowIfNull(nameof(executor));
            executor.ThrowIf(e => e.CanExecute(settings) == false, 
                nameof(executor), 
                $"Given executor '{executor.GetType().Name}' can not execute settings of type '{settings?.GetType().Name ?? String.Empty}'.");

            _executor = executor;
            _executor.Completed += (_, args) => Dispatcher.CurrentDispatcher.Invoke(() => Completed?.Invoke(this, args));

            _settings = settings;
            _resolver = resolver;

            if (settings.Content != null && resolver.CanCreate(settings.Content))
            {
                Content = (ITimelineStepViewModel)resolver.Create(settings.Content);
                isContent = true;
            }
        }


        private bool isContent = false; 
        public bool IsContent
        {
            get { return isContent; }
            private set
            {
                if (Set(ref isContent, value))
                {
                    IsContentChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> IsContentChanged;


        public SessionStepResult Complete()
        {
            return _executor.Complete();
        }

        public event EventHandler<SessionStepResult> Completed;


        private ITimelineStepViewModel content = null;
        public ITimelineStepViewModel Content
        {
            get { return content; }
            private set { Set(ref content, value); }
        }


        public async void Execute(SessionRecordingViewModel recording)
        {
            if (isContent)
            {
                Content?.Execute(recording);

                // ???
                // add content VM to the property named Content
                // register for Completed event of the Content VM and wait for it?
                try
                {
                    await Task.Run(() => _executor.Execute(recording.Recording, _settings));
                }
                catch { }
            }
            else
            {
                _executor.Execute(recording.Recording, _settings);
            }

            IsContent = false;

            Content?.Complete();
            // Content.Complete(); ???
        }
    }
}
