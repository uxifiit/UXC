/**
 * UXC.Devices.EyeTracker.Calibration.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Devices.EyeTracker.Calibration;
using UXC.Devices.EyeTracker.Models;
using UXI.Common.UI;

namespace UXC.Devices.EyeTracker.ViewModels
{
    // TODO REFACTOR with SelectionViewModel`1
    public class StoredCalibrationsViewModel : BindableBase
    {
        private readonly ICalibrationProfilesService _service;

        public StoredCalibrationsViewModel(ICalibrationProfilesService calibrations)
        {
            _service = calibrations;
        }


        public ObservableCollection<CalibrationInfo> Calibrations { get; } = new ObservableCollection<CalibrationInfo>();


        public void Refresh(EyeTrackerDeviceInfo deviceInfo)
        {
            var calibrations = _service.GetStoredCalibrations(deviceInfo.FamilyName);

            Calibrations.Clear();

            calibrations.ForEach(Calibrations.Add);

            SelectedCalibration = Calibrations.FirstOrDefault();
        }


        private CalibrationInfo selectedCalibration;
        public CalibrationInfo SelectedCalibration
        {
            get { return selectedCalibration; }
            set
            {
                Set(ref selectedCalibration, value);
                loadCommand?.RaiseCanExecuteChanged();
            }
        }




        public event EventHandler<CalibrationData> CalibrationLoaded;


        private RelayCommand loadCommand;
        public RelayCommand LoadCommand => loadCommand
            ?? (loadCommand = new RelayCommand(() => LoadSelectedCalibration(), () => SelectedCalibration != null));

        private void LoadSelectedCalibration()
        {
            var calibration = selectedCalibration;

            if (calibration != null)
            {
                var data = _service.LoadCalibration(selectedCalibration);
                if (data != null)
                {
                    CalibrationLoaded?.Invoke(this, data);
                }
            }
        }


        // allow browse for calibration file
        // + extension
        //private RelayCommand restoreCalibrationCommand;
        //public RelayCommand RestoreCalibrationCommand => restoreCalibrationCommand
        //    ?? (restoreCalibrationCommand = _commands.Register(() =>
        //    {

        //    }, () => true));


        private CalibrationData _resultData = null;

        public void PrepareSave(CalibrationData data)
        {
            _resultData = data;
            IsSaved = false;

            saveCommand?.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(CanSave));
        }


        private string saveName = String.Empty;
        public string SaveName
        {
            get { return saveName; }
            set
            {
                if (Set(ref saveName, value))
                {
                    saveCommand?.RaiseCanExecuteChanged();
                    OnPropertyChanged(nameof(CanSave));                    
                }
            }
        }


        public bool CanSave => _resultData != null && String.IsNullOrWhiteSpace(saveName) == false;


        private RelayCommand saveCommand;
        public RelayCommand SaveCommand => saveCommand
            ?? (saveCommand = new RelayCommand(Save, () => CanSave && (isSaved == false)));

        private void Save()
        {
            CalibrationInfo info = null;
            if (_service.TrySaveCalibration(SaveName, _resultData, out info))
            {
                IsSaved = true;

                var oldInfo = Calibrations.FirstOrDefault(i => i.Id == info.Id);
                if (oldInfo != null)
                {
                    Calibrations.Remove(oldInfo);
                }
                Calibrations.Insert(0, info);
            }
        }


        private bool isSaved = false;
        public bool IsSaved
        {
            get { return isSaved; }
            private set
            {
                if (Set(ref isSaved, value))
                {
                    saveCommand?.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
