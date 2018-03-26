//using GalaSoft.MvvmLight.CommandWpf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using UXC.Core.Devices;
//using UXC.Core.Managers.Adapters;
//using UXC.Core.ViewServices;
//using UXC.Models;

//namespace UXC.Core.ViewModels.Adapters
//{
//    public class AdaptersControlViewModel
//    {
//        private readonly IAdaptersControl _controller;
//        private readonly INotificationService _notifications;
//        public AdaptersControlViewModel(IAdaptersControl controller, INotificationService notifications)
//        {
//            _notifications = notifications;

//            _controller = controller;
//            //_controller.UserDeviceActionRequired += controller_UserDeviceActionRequired;
//        }

//        //void controller_UserDeviceActionRequired(object sender, UserDeviceActionRequestedEventArgs e)
//        //{
//        //    var request = e.Requests.FirstOrDefault(r => r.Device.Equals(DeviceType.Physiological.EYETRACKER));

//        //    if (request != null)
//        //    {
//        //        //if (request.TargetState == DeviceState.Ready)
//        //        //{
//        //        //    _notifications.ShowInfoMessage("Calibrate eye tracker", "Eye tracker requires calibration.");
//        //        //}
//        //        //else 
//        //        if (request.TargetState == DeviceState.Connected)
//        //        {
//        //            _notifications.ShowInfoMessage("Connect eye tracker", "No eye tracker found.");
//        //        }
//        //        else if (request.TargetState == DeviceState.Recording)
//        //        {
//        //            _notifications.ShowInfoMessage("Prepare eye tracker", "Connect or calibrate the eye tracker.");
//        //        }
//        //        else
//        //        {
//        //            _notifications.ShowInfoMessage("Action required", "GazeHook requires your attention.");
//        //        }
//        //    }
//        //}

//        private ICommand connectCommand;
//        public ICommand ConnectCommand => connectCommand ?? (connectCommand = new RelayCommand(async () => await _controller.ConnectAsync()));

//        private ICommand disconnectCommand;
//        public ICommand DisconnectCommand => disconnectCommand ?? (disconnectCommand = new RelayCommand(() => _controller.DisconnectAsync()));

//        private ICommand startCommand;
//        public ICommand StartCommand => startCommand ?? (startCommand = new RelayCommand(() => _controller.StartRecordingAsync()));

//        private ICommand stopCommand;
//        public ICommand StopCommand => stopCommand ?? (stopCommand = new RelayCommand(() => _controller.StopRecordingAsync()));
//    }
//}
