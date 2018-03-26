//using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UXC;
using UXC.Core.Common.Events;
using UXC.Core.ViewServices;

namespace UXC.Core.ViewModels
{
    public class AppViewModel : ProgressViewModel
    {
        //private static readonly ILog logger = LogManager.GetLogger(typeof(AppViewModel));
        private readonly IAppService _app;

        //static AppViewModel()
        //{
        //    log4net.Config.XmlConfigurator.Configure();
        //}

        public AppViewModel(IAppService app)
        {
            _app = app;
            _app.StateChanged += app_StateChanged;
            _app.Error += app_Error;
            State = app.State;

        }

        private void app_StateChanged(object sender, ValueChangedEventArgs<AppState> e)
        {
            State = e.CurrentValue;
        }



        void app_Error(object sender, Exception e)
        {
            //UXC.Views.ErrorWindow.Show(e);
            //logger.Error(LogHelper.Prepare("Application error occured " + e));
        }



        private AppState state;
        public AppState State
        {
            get { return state; }
            private set
            {
                if (Set(ref state, value))
                {
                    IsError = value.HasFlag(AppState.Error);
                    IsLoading = value.HasFlag(AppState.Working);
                    IsLoaded = value.HasFlag(AppState.Loaded);
                }
            }
        }


        //public void Start()
        //{
        //    IsLoading = true;

        //    if (_app.State != AppState.Loaded)
        //    {
        //        IsLoaded = _app.Load();
        //    }

        //    if (_app.State != AppState.Started)
        //    {
        //        _app.Start();
        //    }

        //    IsLoading = false;
        //}

        //public void Stop()
        //{
        //    _app.Stop();
        //}




        //private ICommand openSettingsCommand;
        //public ICommand OpenSettingsCommand
        //{
        //    get
        //    {
        //        return openSettingsCommand ?? (openSettingsCommand = new RelayCommand(() => {
        //            _settings.Reload();

        //            _flyouts.Settings.Content.GoBackOrNavigate(Pages.SettingsPage);
        //            _flyouts.Settings.Open();
        //        }));
        //    }
        //}


        //private ICommand createSessionCommand;
        //public ICommand CreateSessionCommand
        //{
        //    get
        //    {
        //        return createSessionCommand ?? (createSessionCommand = new RelayCommand(() =>
        //        {
        //            _flyouts.Dialog.Content.ClearBackStack();
        //            _flyouts.Dialog.Content.Navigate(Pages.CreateSessionPage);
        //            _flyouts.Dialog.Open("create new session", FlyoutPosition.Left);
        //        }, () => IsAdvanced));
        //    }
        //}

    }
}
