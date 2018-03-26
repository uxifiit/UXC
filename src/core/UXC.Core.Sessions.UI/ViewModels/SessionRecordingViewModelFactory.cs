using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UXC.Core.Modules;
using UXC.Core.ViewModels;
using UXC.Core.ViewServices;

namespace UXC.Sessions.ViewModels
{
    public class SessionRecordingViewModelFactory : RelayViewModelFactory<SessionRecording, SessionRecordingViewModel>
    {
        public SessionRecordingViewModelFactory(IInstanceResolver resolver, Dispatcher dispatcher)
            : base(recording => new SessionRecordingViewModel(recording, resolver.Get<ViewModelResolver>(), dispatcher))
        {
        }   
    }
}
