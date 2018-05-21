using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Plugins.UXR.Configuration;
using UXI.Common.Extensions;

namespace UXC.Plugins.UXR.Models
{
    class UXRNodeContext : IUXRNodeContext
    {
        private readonly IUXRConfiguration _configuration;

        internal UXRNodeContext(IUXRConfiguration configuration)
        {
            _configuration = configuration;
            NodeName = configuration.NodeName;  
        }

        private bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                bool previous = ObjectEx.GetAndReplace(ref isConnected, value);
                if (previous != value)
                {
                    IsConnectedChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> IsConnectedChanged;


        private int nodeId = -1;
        public int NodeId
        {
            get
            {
                return nodeId;
            }
            set
            {
                int previous = ObjectEx.GetAndReplace(ref nodeId, value);
                if (previous != value)
                {
                    NodeIdChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<int> NodeIdChanged;



        private string nodeName = null;
        public string NodeName
        {
            get
            {
                return String.IsNullOrWhiteSpace(nodeName)
                     ? Environment.MachineName
                     : nodeName;
            }
            set
            {
                var oldName = ObjectEx.GetAndReplace(ref nodeName, value);
                if (oldName != value)
                {
                    NodeNameChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<string> NodeNameChanged;
    }
}
