using System;

namespace UXC.Core.ViewServices
{
    public interface IInteractionService
    {
        event EventHandler<InteractionRequestEventArgs> InteractionRequested;

        bool RequestInteraction(object source, InteractionRequest request);
    }
}
