using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UXC.Core.Devices;
using UXI.Common.Converters;

namespace UXC.Common.Converters
{

    public class BoolToTrayIconUriConverter : BoolToValueConverter<Uri>
    {
        public BoolToTrayIconUriConverter()
            : base(new Uri("/Resources/Green.ico", UriKind.RelativeOrAbsolute), new Uri("/Resources/Red.ico", UriKind.RelativeOrAbsolute))
        { }

        //public static Uri Convert(DeviceState state)
        //{
        //    switch (state)
        //    {
        //        case DeviceState.Disconnected:
        //            return new Uri("/Resources/Grey.ico", UriKind.RelativeOrAbsolute);
        //        case DeviceState.Connected:
        //            return new Uri("/Resources/Orange.ico", UriKind.RelativeOrAbsolute);
        //        case DeviceState.Ready:
        //        case DeviceState.Recording:
        //            return new Uri("/Resources/Green.ico", UriKind.RelativeOrAbsolute);
        //        case DeviceState.Error:
        //            return new Uri("/Resources/Red.ico", UriKind.RelativeOrAbsolute);
        //        default:
        //            return new Uri("/Resources/Grey.ico", UriKind.RelativeOrAbsolute);
        //    }
        //}
    }
}
