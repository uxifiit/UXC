//using log4net;
using System;
using System.Collections.Generic;
using UXI.Common.Helpers;
using UXC.Core.Common.Events;
using UXC.Core.Managers;
using UXC.Core;
using System.Linq;
using UXC.Core.Modules;
using UXC.Common.Extensions;

namespace UXC.Core.ViewServices
{
    public class AppService : NotifyStateChangedBase<AppState>, IAppService
    {
        public event EventHandler<Exception> Error;

        private readonly IEnumerable<IConnector> _connectors;
        private readonly IEnumerable<ILoader> _loaders;

        private readonly List<IControlService> _controls = new List<IControlService>();

        public AppService(IEnumerable<IConnector> connectors, IEnumerable<ILoader> loaders, IEnumerable<IControlService> controls, IModulesService modules)
        {
            state = AppState.None;
            _connectors = connectors ?? Enumerable.Empty<IConnector>();
            _loaders = loaders ?? Enumerable.Empty<ILoader>();

            _controls.TryAddRange(controls);
            modules.Register<IControlService>(this, AddControlServices);
        }


        private void AddControlServices(IEnumerable<IControlService> controls)
        {
            var newControls = controls?.Where(c => _controls.Contains(c) == false)
                     .Execute(c => _controls.Add(c));

            if (State == AppState.Started)
            {
                newControls = newControls.Where(c => c.AutoStart)
                                         .Execute(c => c.Start());
            }

            newControls.Enumerate();
        }
           

        public bool Load()
        {
            try
            {
                if (State.HasFlag(AppState.Loaded) == false && State.HasFlag(AppState.Working) == false)
                {
                    State = AppState.Loading;

                    foreach (var loader in _loaders)
                    {
                        loader.Load();
                    }

                    foreach (var connector in _connectors)
                    {
                        connector.ConnectAll();
                    }

                    State = AppState.Loaded;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(LogHelper.Prepare("Error loading application " + ex));
                State = State | AppState.Error;
                Error?.Invoke(this, ex);
            }
            return false;
        }



        public bool Start()
        {
            try
            {
                if (State.HasFlag(AppState.Started) == false && State.HasFlag(AppState.Working) == false)
                {
                    State = AppState.Starting;
                    foreach (var control in _controls.Where(c => c.AutoStart))
                    {
                        control.Start();
                    }

                    State = AppState.Started;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(LogHelper.Prepare("Error initializing application " + ex));
                State = AppState.Loaded | AppState.Error;
                Error?.Invoke(this, ex);
            }
            return false;
        }


        public bool CheckIfStopCancelsWorkInProgress()
        {
            return State.HasFlag(AppState.Started) 
                && _controls.Any(c => c.IsWorking());
        }


        public bool Stop()
        {
            if (State.HasFlag(AppState.Started))
            {
                foreach (var control in _controls)
                {
                    control.Stop();
                }
                State = AppState.Loaded;
            }

            if (State.HasFlag(AppState.Loaded))
            {
                // TODO
                //foreach (var connector in _connectors)
                //{
                //    connector.DisconnectAll();
                //}
                State = AppState.None;
            }

            return true;
        }
    }
}
