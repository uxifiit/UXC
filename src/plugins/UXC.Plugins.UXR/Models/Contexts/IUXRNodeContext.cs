using System;

namespace UXC.Plugins.UXR.Models
{
    public interface IUXRNodeContext
    {
        bool IsConnected { get; }
        event EventHandler<bool> IsConnectedChanged;
        
        int NodeId { get; }
        event EventHandler<int> NodeIdChanged;

        string NodeName { get; }
        event EventHandler<string> NodeNameChanged;
    }
}
