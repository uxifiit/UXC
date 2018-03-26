using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Devices.Calibration
{
    public enum CalibratorState
    {
        None,
        Preparation,
        Running,
        Finished,
        Completed,
        Canceled
    }
}
