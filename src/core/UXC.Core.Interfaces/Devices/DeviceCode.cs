using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXC.Core.Devices.Calibration;
using UXC.Models;
using UXC.Core.Configuration;

namespace UXC.Core.Devices
{
    public class DeviceCode
    {
        public static Builder Create<T>(T device, string typeCode)
            where T : IDevice
        {
            return new Builder(typeCode)
                .IsConfigurable(device is IConfigurable)
                .IsCalibratable(device is ICalibrate);
        }

        public static Builder Create(string typeCode)
        {
            return new Builder(typeCode);
        }

        private DeviceCode(DeviceType deviceType) { DeviceType = deviceType; }

        public DeviceType DeviceType { get; }

        public bool IsConfigurable { get; private set; } = false;
        public bool CanBeCalibrated { get; private set; }
        public bool RequiresCalibrationBeforeStart { get; private set; }
        public bool RunsOnMainThread { get; private set; }

        /// <summary>
        /// Gets the type of connecting to the device. 
        /// </summary>
        /// <value<see cref="DeviceConnectionType.None"/> determines that a device is not connecting at all, it is connected by creating its instance.
        /// Other <see cref="DeviceConnectionType"/> values describes amount of approximate time required to connect to device, e.g.:
        /// <see cref="DeviceConnectionType.SystemApi"/> - using operating systems API calls,
        /// <see cref="DeviceConnectionType.Process"/> - starting a new process,
        /// <see cref="DeviceConnectionType.Port"/> - searching for a device on computer ports.
        /// </value>
        public DeviceConnectionType ConnectionType { get; private set; }

        public class Builder
        {
            private readonly DeviceCode _code;

            internal Builder(string typeCode)
            {
                DeviceType deviceType = DeviceType.GetOrCreate(typeCode);

                _code = new DeviceCode(deviceType);
            }

            public Builder IsConfigurable(bool value)
            {
                _code.IsConfigurable = value;
                return this;
            }

            public Builder IsCalibratable(bool value)
            {
                _code.CanBeCalibrated = value;
                return this;
            }

            public Builder RequiresCalibrationBeforeStart(bool value)
            {
                _code.RequiresCalibrationBeforeStart = value;
                return this;
            }

            public Builder RunsOnMainThread(bool value)
            {
                _code.RunsOnMainThread = value;
                return this;
            }

            public Builder ConnectionType(DeviceConnectionType connectionType)
            {
                _code.ConnectionType = connectionType;
                return this;
            }

            public DeviceCode Build()
            {
                return _code;
            }
        }
    }
}
