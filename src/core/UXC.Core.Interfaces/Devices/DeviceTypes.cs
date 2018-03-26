using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXC.Core.Devices
{
    public class DeviceType
    {
        private static readonly ConcurrentDictionary<string, DeviceType> instances = new ConcurrentDictionary<string, DeviceType>();


        public static bool TryResolveExistingType(string code, out DeviceType device)
        {
            return instances.TryGetValue(code, out device);
        }


        private DeviceType(string code)
        {
            code.ThrowIf(String.IsNullOrWhiteSpace, nameof(code));

            Code = code.ToUpper();

            code.ThrowIf(_ => instances.TryAdd(code, this) == false, nameof(code), $"A device type with code {code} is already defined.");
        }


        private DeviceType(DeviceType parent, string code)
            : this(parent.Code + code.ToUpper())
        {
        }

        public string Code { get; }


        public override string ToString()
        {
            return Code;
        }


        public static explicit operator DeviceType(string code)
        {
            code.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(code));

            DeviceType device;

            return TryResolveExistingType(code, out device)
                 ? device
                 : new Devices.DeviceType(code);
        }


        public static implicit operator string(DeviceType device)
        {
            return device?.Code;
        }


        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var type = obj as DeviceType;
            if (type == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Code == type.Code;
        }


        public bool Equals(DeviceType type)
        {
            // If parameter is null return false:
            if ((object)type == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Code == type.Code;
        }


        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }


        /// <summary>
        /// Creates custom device type from the supplied device name. Device type code is traditionally an abbreviation of a device name in upper case.
        /// </summary>
        /// <param name="device">A device type code that identifies the device type.</param>
        /// <returns>Instance of <seealso cref="DeviceType" /> identifying the device type.</returns>
        public static DeviceType GetOrCreate(string code)
        {
            code.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(code));

            DeviceType deviceType;
            if (DeviceType.TryResolveExistingType(code, out deviceType) == false)
            {
                deviceType = new DeviceType(code);
            }

            return deviceType;
        }


        public static Streaming StreamingDevices { get; } = new Streaming();

        public static Physiological PhysiologicalDevices { get; } = new Physiological();

        public static Input InputDevices { get; } = new Input();

        public class Streaming : IEnumerable<DeviceType>
        {
            public static readonly DeviceType CEILING_CAM = new DeviceType("CC");
            public static readonly DeviceType WEBCAM = new DeviceType("WC");

            public static readonly DeviceType SCREENCAST = new DeviceType("SC");
            public static readonly DeviceType WEBCAM_AUDIO = new DeviceType(WEBCAM, "A");
            public static readonly DeviceType WEBCAM_VIDEO = new DeviceType(WEBCAM, "V");
            public static readonly DeviceType WEBCAM_DEPTH = new DeviceType(WEBCAM, "D");

            internal Streaming() { }

            public IEnumerator<DeviceType> GetEnumerator()
            {
                yield return SCREENCAST;
                yield return WEBCAM_AUDIO;
                yield return WEBCAM_VIDEO;
                yield return WEBCAM_DEPTH;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }



        //public const string CEILING_CAM2 = "CC-CB";
        //public const string CEILING_CAM3 = "CC-LM";
        //public const string CEILING_CAM4 = "CC-LPC";

   
        public class Physiological : IEnumerable<DeviceType>
        {
            public static readonly DeviceType EYETRACKER = new DeviceType("ET");
            //public static readonly DeviceType TEMPERATURE = new DeviceType("TEMP");
            //public static readonly DeviceType EPOC = new DeviceType("EPO");
            //public static readonly DeviceType CAPTIVE = new DeviceType("CAP");
            //public static readonly DeviceType ELECTRO_DERMAL = new DeviceType("ED");
            //public static readonly DeviceType EMOTION = new DeviceType("EM");
            //public static readonly DeviceType HEART_RATE_MONITOR = new DeviceType("HRM");

            internal Physiological() { }

            public IEnumerator<DeviceType> GetEnumerator()
            {
                yield return EYETRACKER;
                //yield return TEMPERATURE;
                //yield return EPOC;
                //yield return CAPTIVE;
                //yield return ELECTRO_DERMAL;
                //yield return EMOTION;
                //yield return HEART_RATE_MONITOR;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        public class Input : IEnumerable<DeviceType>
        {
            public static readonly DeviceType KEYBOARD = new DeviceType("KB");
            public static readonly DeviceType MOUSE = new DeviceType("ME");

            internal Input() { }

            public IEnumerator<DeviceType> GetEnumerator()
            {
                yield return KEYBOARD;
                yield return MOUSE;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        public static readonly DeviceType EXTERNAL = new DeviceType("EXT");
        public static readonly DeviceType EXTERNAL_EVENTS = new DeviceType(EXTERNAL, "EV");
    }
}
                      
