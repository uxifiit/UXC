using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UXC.Devices.EyeTracker.Calibration;
using UXI.Common.Extensions;

namespace UXC.Devices.EyeTracker.Models
{
    public class CalibrationInfo
    {
        private CalibrationInfo() { }

        public static CalibrationInfo Create(string name, string deviceFamilyName, string deviceName, DateTime createdAt)
        {
            var data = new CalibrationInfo()
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedAt = createdAt,
                DeviceFamilyName = deviceFamilyName,
                DeviceName = deviceName
            };

            return data;
        }
        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DeviceFamilyName { get; set; }

        public string DeviceName { get; set; }
    }
}
