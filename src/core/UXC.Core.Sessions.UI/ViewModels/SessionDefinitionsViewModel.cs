using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using UXI.Common;
using UXI.Common.UI;
using UXC.Core.Devices;
using System.Windows.Threading;
using UXC.Core.Common.Events;
using UXC.Core.ViewModels;
using System.Threading;
using UXI.Common.Extensions;
using System.Reactive.Disposables;

namespace UXC.Sessions.ViewModels
{
    public class SessionDefinitionsViewModel : BindableBase
    {
        private readonly SessionDefinitionsSource _definitions;
        private readonly Dispatcher _dispatcher;

        private readonly ImportSessionDefinitions _import;

        private CancellationDisposable _refreshCancellation;


        public SessionDefinitionsViewModel(SessionDefinitionsSource definitions, ILocalSessionDefinitionsService service, Dispatcher dispatcher)
        {
            _definitions = definitions;
            _dispatcher = dispatcher;

            _import = new ImportSessionDefinitions(service);
            definitions.Link(_import);

            Selection = new SelectionViewModel<ISessionChoiceViewModel>(_definitions.Definitions.Select(s => new SessionDefinitionViewModel(s)).Cast<ISessionChoiceViewModel>().Prepend(new CreateSessionViewModel()));
            _definitions.DefinitionsChanged += definitions_DefinitionsChanged;

            SelectLastDefinition();
            Selection.SelectedItemChanged += Selection_SelectedItemChanged;
        }


        private void definitions_DefinitionsChanged(object sender, CollectionChangedEventArgs<SessionDefinition> e)
        {
            var selected = Selection.SelectedItem;

            if (e.AddedItems.Any())
            {
                _dispatcher.Invoke(() => AddSessionDefinitions(e.AddedItems));
            }

            // TODO removing session definitions
            if (e.RemovedItems.Any())
            {
                if (selected is SessionDefinitionViewModel)
                {
                    var selectedDefinition = selected.GetDefinition();
                    if (selectedDefinition != null
                        && e.RemovedItems.Any(d => d.Id == selectedDefinition.Id))
                    {
                        selected = null;
                    }
                }

                _dispatcher.Invoke(() => RemoveSessionDefinitions(e.RemovedItems));
            }

            if (selected != null)
            {
                Selection.SelectedIndex = Selection.Items.IndexOf(selected);
            }
            else
            {
                SelectLastDefinition();
            }
        }


        private void Selection_SelectedItemChanged(object sender, ISessionChoiceViewModel e)
        {
            addDeviceCommand?.RaiseCanExecuteChanged();
            removeDeviceCommand?.RaiseCanExecuteChanged();
        }


        public SelectionViewModel<ISessionChoiceViewModel> Selection { get; }


        private void SelectLastDefinition()
        {
            if (Selection.Items.Any())
            {
                Selection.SelectedIndex = Selection.Items.Count - 1;
            }
        }


        private IEnumerable<SessionDefinitionViewModel> AddSessionDefinitions(IEnumerable<SessionDefinition> sessions)
        {
            var sessionsToAdd = sessions.Select(session => new SessionDefinitionViewModel(session))
                                        .ToList();

            foreach (var addedSession in sessionsToAdd)
            {
                Selection.Items.Add(addedSession);
            }

            return sessionsToAdd;
        }

        private IEnumerable<SessionDefinitionViewModel> RemoveSessionDefinitions(IEnumerable<SessionDefinition> sessions)
        {
            var sessionsToRemove = Selection.Items.OfType<SessionDefinitionViewModel>()
                                            .Where(session => sessions.Any(d => d.Id == session.Id))
                                            .ToList();

            foreach (var removedSession in sessionsToRemove)
            {
                Selection.Items.Remove(removedSession);
            }

            return sessionsToRemove;
        }


        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand => refreshCommand
            ?? (refreshCommand = new RelayCommand(() => RefreshAsync().Forget(), () => _refreshCancellation == null || _refreshCancellation.IsDisposed));

        public async Task RefreshAsync()
        {
            _refreshCancellation?.Dispose();

            using (var cancellation = new CancellationDisposable())
            {
                _refreshCancellation = cancellation;
                refreshCommand?.RaiseCanExecuteChanged();

                try
                {
                    refreshCommand?.RaiseCanExecuteChanged();

                    await _definitions.RefreshAsync(cancellation.Token);
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception ex)
                {
                    // TODO LOG
                }

                _refreshCancellation = null;

                refreshCommand?.RaiseCanExecuteChanged();
            }
        }


        private RelayCommand<DeviceType> addDeviceCommand;
        public RelayCommand<DeviceType> AddDeviceCommand => addDeviceCommand
            ?? (addDeviceCommand = new RelayCommand<DeviceType>(AddDevice, CanAddDevice));

        private void AddDevice(DeviceType device)
        {
            ISessionChoiceViewModel choice;

            if (Selection.TryGetSelectedItem(out choice)
                && choice.SelectedDevices.IsReadOnly == false)
            {
                choice.SelectedDevices.Add(device);

                addDeviceCommand?.RaiseCanExecuteChanged();
                removeDeviceCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool CanAddDevice(DeviceType device)
        {
            ISessionChoiceViewModel choice;
            
            return Selection.TryGetSelectedItem(out choice)
                && choice.SelectedDevices.Contains(device) == false;
        }


        private RelayCommand<DeviceType> removeDeviceCommand;
        public RelayCommand<DeviceType> RemoveDeviceCommand => removeDeviceCommand
            ?? (removeDeviceCommand = new RelayCommand<DeviceType>(RemoveDevice, CanRemoveDevice));

        private void RemoveDevice(DeviceType device)
        {
            ISessionChoiceViewModel choice;

            if (Selection.TryGetSelectedItem(out choice)
                && choice.SelectedDevices.IsReadOnly == false
                && choice.SelectedDevices.Remove(device))
            {
                addDeviceCommand?.RaiseCanExecuteChanged();
                removeDeviceCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool CanRemoveDevice(DeviceType device)
        {
            ISessionChoiceViewModel choice;

            return Selection.TryGetSelectedItem(out choice)
                && choice.SelectedDevices.Contains(device);
        }


        private RelayCommand openCommand;
        public RelayCommand OpenCommand => openCommand
            ?? (openCommand = new RelayCommand(() => Open()));

        private string[] OpenFiles()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "*.json";
            dialog.Filter = "JSON Files (*.json)|*.json";
            dialog.Multiselect = true;
            dialog.Title = "Open session definition(s)";
            bool? selected = dialog.ShowDialog();

            return (selected.HasValue && selected.Value)
                 ? dialog.FileNames
                 : null;
        }

        private void Open()
        {
            var files = OpenFiles();
            if (files != null && files.Any())
            {
                SessionDefinition definition;
                SessionDefinition lastDefinition = null;

                foreach (var file in files)
                {
                    try
                    {
                        if (_import.TryAdd(file, out definition))
                        {
                            lastDefinition = definition;
                        }
                    }
                    catch
                    {
                        // TODO LOG
                        // TODO ! show error message
                    }
                }

                if (lastDefinition != null)
                {
                    var id = lastDefinition.Id;
                    var vm = Selection.Items.FirstOrDefault(s => s.Id == id);
                    Selection.SelectedIndex = Selection.Items.IndexOf(vm);
                }

                // TODO select added definition
            }
        }

        // TODO locking
        //private RelayCommand unlockEditDevicesCommand;
        //public RelayCommand UnlockEditDevicesCommand => unlockEditDevicesCommand
        //    ?? (unlockEditDevicesCommand = new RelayCommand(UnlockEditDevices, CanLockUnlockEditDevices));

        //private void UnlockEditDevices()
        //{
        //    SelectedDefinition.CanEditDevices = true;
        //}

        //private bool CanLockUnlockEditDevices()
        //{
        //    return HasSelectedDefinition
        //        && SelectedDefinition.CanLockDevices;    
        //}

        //private RelayCommand lockEditDevicesCommand;
        //public RelayCommand LockEditDevicesCommand => lockEditDevicesCommand
        //    ?? (lockEditDevicesCommand = new RelayCommand(LockEditDevices, CanLockUnlockEditDevices));
    }
}
