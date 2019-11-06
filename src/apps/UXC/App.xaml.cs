/**
 * UXC
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UXC.Modules;
using Microsoft.Shell;
using Hardcodet.Wpf.TaskbarNotification;
using Ninject;
using UXI.Common.UI;
using UXC.Devices.EyeTracker;
using UXC.Core.Modules;
using UXC.Core.Plugins;
using UXC.Plugins.GazeAccess;
using UXC.Core.ViewServices;
using UXC.Configuration;
using UXC.Devices.EyeTracker.Calibration.UI.Module;
using UXC.Sessions;
using UXC.Devices.ExternalEvents.Module;
using UXC.Plugins.SessionsAPI;
using UXC.Plugins.UXR;
using UXI.Common.Extensions;
using UXC.Devices.KeyboardMouse.Module;
using UXC.Devices.Streamers.Module;
using UXC.Plugins.DefaultAPI;
using UXI.SystemApi;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using UXC.Core.Logging;
using UXC.Plugins.Sessions.Fixations;
using UXC.Design;
using UXC.Core;
using UXC.Common;

namespace UXC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp, ITrayIconProvider
    {
        private const string AppIdentification = "UXC";

        private static ILogger _logger = NullLogger.Instance;

        [STAThread]
        public static void Main()
        {
            if (DesignTimeHelper.IsDesignTime == false && SingleInstance<App>.InitializeAsFirstInstance(AppIdentification))
            {
                var app = new App();

                app.DispatcherUnhandledException += App_DispatcherUnhandledException;

                app.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger?.Fatal("", e.Exception);
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }


        private readonly Job _job;
        private readonly IKernel _kernel;
        private readonly IAppConfiguration _config;

        internal IKernel Kernel => _kernel;


        public App()
        {
            // Wrap the current main process in the job, so any other new child process started by this process will be managed by this job. 
            // If the main process exits or is killed, the job is closed and all other child processes belonging to this job are closed as well. 
            _job = new Job();
            _job.AddProcess(Process.GetCurrentProcess().Id);

            _kernel = CreateKernel();
            _config = _kernel.Get<IAppConfiguration>();

            InitializeComponent();
        }


        internal static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new NinjectSettings()
            {
                InjectNonPublic = true
            });

            kernel.Load(new CoreModule());

#if DEBUG
            bool isDesign = DesignTimeHelper.IsDesignTime;
            if (isDesign)
            {
                kernel.Load(new DesignApplicationModule());
            }
            else
#endif
            {
                kernel.Load(new ApplicationModule());
                kernel.Load(new LoggingModule());
            }


            var modules = kernel.Get<ModulesManager>();
            modules.Connect(new AppModule());
            modules.Connect(new UXCAppModule());

            modules.Connect(new SessionsModule());
            modules.Connect(new SessionsUIModule());

            var config = kernel.Get<IAppConfiguration>();
            _logger = kernel.Get<Core.Logging.ILogger>();

            var plugins = new PluginsModule(PluginsLoadingMode.Explicit);
#if DEBUG
            if (isDesign)
            {
                plugins.Plugins.Add(new DesignDevicesModule());
            }
            else
#endif
            {
                plugins.Plugins.Add(new EyeTrackerModule());
                plugins.Plugins.Add(new EyeTrackerUIModule());
                plugins.Plugins.Add(new ExternalEventsModule());
                plugins.Plugins.Add(new KeyboardModule());
                plugins.Plugins.Add(new MouseModule());
                plugins.Plugins.Add(new StreamersModule());
                plugins.Plugins.Add(new DefaultAPIModule());
                plugins.Plugins.Add(new GazeAccessModule());
                plugins.Plugins.Add(new SessionsAPIModule());
                plugins.Plugins.Add(new UXRModule());
                plugins.Plugins.Add(new SessionFixationsModule());
                plugins.Plugins.Add(new SessionEyeTrackerModule());

                if (config.Experimental)
                {
                    plugins.Plugins.Add(new GazeVisualization.GazeVisualizationModule());
                }
            }

            modules.Connect(plugins);

            modules.Connect
            (
                new ViewServicesModule(),
                new ViewModelsModule()
            );

#if DEBUG
            if (isDesign == false)
            {
#endif
                modules.Connect(new OwinServerModule());
#if DEBUG
            }
#endif

            // TODO where to get tray icon?
            // var tray = _kernel.Get<ITrayIconProvider>();

            return kernel;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var app = _kernel.Get<IAppService>();

            // then load and start app - it's async anyway! VMs can load
            app.Load();

            var args = SingleInstance<App>.CommandLineArgs;
            args.Remove(System.Reflection.Assembly.GetEntryAssembly().Location);

            // read args
            var parser = _kernel.Get<CommandLineOptionsParser>();
            parser.Parse(args);

            // add start
            app.Start();

            var window = OpenMainWindow();

            ShowTray();

            if (window != null)
            {
                window.Closing += MainWindow_Closing;
            }
        }


        private Window OpenMainWindow()
        {
            var window = new MainWindow();

            this.MainWindow = window;

            window.Show();

            return window;
        }


        private TaskbarIcon icon;

        public void ShowTray()
        {
            if (icon == null)
            {    
                // TODO move to ViewModel
                icon = (TaskbarIcon)FindResource("TaskBarIcon");
                //initialize NotifyIcon   
                icon.ToolTipText = "UXC";
                icon.TrayMouseDoubleClick += (_, __) => RestoreFromTray();
                icon.Visibility = Visibility.Visible;
                icon.PopupActivation = PopupActivationMode.LeftClick;
                icon.PreviewTrayPopupOpen += Icon_PreviewTrayPopupOpen;
              
                // TODO where to store a tray icon?
                Icon = icon;
            }
        }

        private void Icon_PreviewTrayPopupOpen(object sender, RoutedEventArgs e)
        {
            var window = MainWindow;
            if (window != null && window.IsVisible)
            {
                window.Activate();
                e.Handled = true;
            }
        }

        private void RestoreFromTray()
        {
            MainWindow.Show();

            if (MainWindow.WindowState == WindowState.Minimized)
            {
                MainWindow.WindowState = WindowState.Normal;
            }
        }



        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window window = (Window)sender;
            if (_config.HideOnClose)
            {
                var tb = icon;
                e.Cancel = true;
                window.Hide();
            }
            else
            {
                e.Cancel = true;
                bool canClose = await CheckCanCloseAppAsync();
                if (canClose)
                {
                    CloseApp();
                }
            }
        }


        private void HideTray()
        {
            var tb = ObjectEx.GetAndReplace(ref icon, null);
            if (tb != null && tb.IsDisposed == false)
            {
                //tb.HideBalloonTip();
                tb.Dispose();
            }
        }

        private async void TrayiconExit(object sender, RoutedEventArgs e)
        {
            bool canClose = await CheckCanCloseAppAsync();
            if (canClose)
            {
                CloseApp();
            }
        }

        private bool closed = false;

        public TaskbarIcon Icon { get; private set; }

        public bool CanShowNotifications { get; private set; } = true;


        private async Task<bool> CheckCanCloseAppAsync()
        {
            var app = _kernel?.Get<IAppService>();
            if (app != null)
            {
                bool isWorking = app.CheckIfStopCancelsWorkInProgress();
                if (isWorking)
                {
                    var window = this.MainWindow as MahApps.Metro.Controls.MetroWindow;
                    RestoreFromTray();
                    window.Activate();

                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "OK",
                        NegativeButtonText = "Cancel",
                        AnimateShow = true,
                        AnimateHide = false
                    };
                    
                    var result = await window.ShowMessageAsync("Quit?",
                        "Sure you want to quit?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);

                    return result == MessageDialogResult.Affirmative;
                }
            }

            return true;
        }


        private void CloseApp()
        {
            HideTray();

            if (closed == false)
            {
                var app = _kernel?.Get<IAppService>();

                var window = MainWindow;
                if (window != null)
                {
                    MainWindow.Closing -= MainWindow_Closing;
                    window.Hide();
                    window.Close();
                }

                if (app != null)
                {
                    app.Stop();
                }

                _kernel?.Dispose();
                //await Container.StopAsync();
                //Container.Dispose();

                closed = true;
            }

            _job?.Dispose();

            Application.Current.Shutdown();
        }
    }
}
