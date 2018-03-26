using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Managers.Adapters;

namespace UXC.Core.Services
{
    public class AdaptersControlService : IControlService
    {
        private readonly IAdaptersControl _controller;

        public AdaptersControlService(IAdaptersControl controller)
        {
            _controller = controller;
        }

        public void Start()
        {
            _controller.PrepareReadyAsync();
        }

        public void Stop()
        {
            _controller.DisconnectAsync();
        }
    }
}
