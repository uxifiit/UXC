using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Devices.EyeTracker.Calibration
{
    public class CalibrationExecutionReport
    {
        internal CalibrationExecutionReport(CalibrationPlan plan, CalibrationResult result, CalibrationData data)
        {
            Plan = plan;
            Result = result;
            Data = data;
        }


        public CalibrationPlan Plan { get; }


        public CalibrationResult Result { get; }


        public CalibrationData Data { get; }
    }
}
