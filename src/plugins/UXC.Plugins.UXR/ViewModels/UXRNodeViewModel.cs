using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Plugins.UXR.Models;
using UXI.Common.UI;

namespace UXC.Plugins.UXR.ViewModels
{
    public class UXRNodeViewModel : BindableBase
    {
        private readonly IUXRNodeContext _context;
        private readonly UXRNodeService _nodeService;

        public UXRNodeViewModel(IUXRNodeContext context, UXRNodeService nodeService)
        {
            _context = context;
            _context.IsConnectedChanged += (_, __) => OnPropertyChanged(nameof(IsConnected));

            _nodeService = nodeService;
        }

        public bool IsConnected => _context.IsConnected;
    }
}
