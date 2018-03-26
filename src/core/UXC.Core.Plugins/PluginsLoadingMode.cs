using System;

namespace UXC.Core.Plugins
{
    [Flags]
    public enum PluginsLoadingMode
    {
        Implicit = 1,
        Explicit = 2,
        ImplicitAndExplicit = Implicit | Explicit
    }
}
