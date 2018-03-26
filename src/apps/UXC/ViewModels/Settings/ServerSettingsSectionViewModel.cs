using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Configuration;
using UXC.Core.ViewModels;
using UXI.Common.UI;

namespace UXC.ViewModels.Settings
{
    public class ServerSettingsSectionViewModel : BindableBase, ISettingsSectionViewModel
    {
        private readonly IServerConfiguration _serverConfiguration;

        public ServerSettingsSectionViewModel(IServerConfiguration serverConfiguration)
        {
            _serverConfiguration = serverConfiguration;
        }

        public string Name => "Server";

        public void Reload()
        {
            ServerPort = _serverConfiguration.ServerPort.ToString();
            IsSslEnabled = _serverConfiguration.SslEnabled;
            IsSignalREnabled = _serverConfiguration.SignalREnabled;
        }


        public void Save()
        {
            uint port = 0;
            if (UInt32.TryParse(ServerPort, out port))
            {
                _serverConfiguration.ServerPort = port;
            }

            _serverConfiguration.SslEnabled = IsSslEnabled;
            _serverConfiguration.SignalREnabled = IsSignalREnabled;
        }


        private string serverPort;
        public string ServerPort
        {
            get { return serverPort; }
            set { Set(ref serverPort, value); }
        }


        private bool isSslEnabled;
        public bool IsSslEnabled
        {
            get { return isSslEnabled; }
            set { Set(ref isSslEnabled, value); }
        }


        private bool isSignalREnabled;
        public bool IsSignalREnabled
        {
            get { return isSignalREnabled; }
            set { Set(ref isSignalREnabled, value); }
        }
    }
}
