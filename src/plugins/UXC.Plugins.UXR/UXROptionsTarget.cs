using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Plugins.UXR.Models;
using UXI.Common;

namespace UXC.Plugins.UXR
{
    class UXROptionsTarget : IOptionsTarget
    {
        private readonly UXRNodeContext _node;

        public UXROptionsTarget(UXRNodeContext node)
        {
            _node = node;
        }

        public Type OptionsType { get; } = typeof(UXROptions);

        public void ReceiveOptions(object options)
        {
            var uxrOptions = options as UXROptions;
            if (uxrOptions != null)
            {
                _node.NodeName = uxrOptions.NodeName ?? String.Empty;
            }                               
        }
    }
}
