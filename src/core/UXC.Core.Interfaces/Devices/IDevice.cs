using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Common.Events;
using UXI.Configuration;

namespace UXC.Core.Devices
{
    //public class DeviceInfo
    //{
    //    string Name { get; }
    //    int Code { get; }
    //    string Manufacturer { get; }
    //    int Battery { get; }
    //    bool RunsOnBackground { get; }
    //}

    /// <summary>
    /// Describes the main methods and properties of a device that is supported by the UXC.Core platform.
    /// An implementation of this interface may also implement <see cref="IConfigurable"/> and <see cref="Calibration.ICalibrate"/> interfaces if such device support that functionality.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Gets the device code with information about the device. 
        /// </summary>
        DeviceCode Code { get; }

        Type DataType { get; }

        /// <summary>
        /// Attempts to connect to the device.
        /// </summary>
        /// <returns>True if the connection was succesful, otherwise false.</returns>
        bool ConnectToDevice();

        /// <summary>
        /// Disconnects from the device and frees all allocated resources required for the connection.
        /// </summary>
        /// <returns>True if the disconnection was successful, otherwise false.</returns>
        bool DisconnectDevice();

        /// <summary>
        /// Starts data recording with the connected device.
        /// </summary>
        /// <returns>True if the device has started recording.</returns>
        bool StartRecording();

        /// <summary>
        /// Stops the running data recording with the current device.
        /// </summary>
        /// <returns></returns>
        bool StopRecording();

        /// <summary>
        /// Raised when the device records new data.
        /// </summary>
        event DeviceDataReceivedEventHandler Data;

        /// <summary>
        /// Raised when an error occurres during usage of the device.
        /// </summary>
        event ErrorOccurredEventHandler ConnectionError;


        event DeviceLogEventHandler Log;

        void DumpInfo();
    }
}
