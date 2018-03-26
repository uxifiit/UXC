using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Devices.Adapters;
using UXI.Common.Extensions;
using UXC.Core.ViewServices;
using System.Windows.Threading;
using UXC.Core.Modules;
using System.Collections.Concurrent;

namespace UXC.Core.ViewModels.Adapters
{
    public class AdapterViewModelFactory : IViewModelFactory
    {
        public Type SourceType => typeof(IDeviceAdapter);

        public Type ViewModelType => typeof(AdapterViewModel);

        public object Create(object source)
        {
            source.ThrowIfNull(nameof(source));
            source.ThrowIf(s => s.GetType().IsInstanceOfType(SourceType), nameof(source), $"The type does not match the ${nameof(SourceType)} = ${SourceType.FullName}");

            return new AdapterViewModel((IDeviceAdapter)source);
        }
    }
}
