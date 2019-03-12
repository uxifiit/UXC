/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXC.Sessions.Recording;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions
{
    public class SessionDefinition
    {
        private static int _idCounter = 1;
        private readonly static object _idCounterLock = new object();

        public static SessionDefinition Create()
        {
            lock (_idCounterLock)
            {
                return new SessionDefinition(_idCounter++);
            }
        }

        private readonly int _id;

        private SessionDefinition(int id)
        {
            _id = id;
        }


        public int Id => _id;

        public string Project { get; set; }

        public string Name { get; set; }

        public string Source { get; set; } // UXR, Local, External system

        public string SerializationFormat { get; set; } = "JSON";

        public List<SessionRecorderDefinition> Recorders { get; set; } = new List<SessionRecorderDefinition>();


        /// <summary>
        /// Boolean flag determining whether the recording can start only if all the devices are ready. 
        /// Set to false if recording can start immediately even without any or all devices not prepared.
        /// </summary>
        public bool StrictStart { get; set; } = true;


        private List<SessionDeviceDefinition> devices;
        public List<SessionDeviceDefinition> Devices
        {
            get { return devices; }
            set
            {
                devices = value ?? new List<SessionDeviceDefinition>();
            }
        }


        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<DeviceType> SelectedDeviceTypes => Devices?.Select(d => d.Device) ?? Enumerable.Empty<DeviceType>();

        public WelcomeActionSettings Welcome { get; set; } = new WelcomeActionSettings();

        public List<SessionStep> PreSessionSteps { get; set; } = new List<SessionStep>();

        public List<SessionStep> SessionSteps { get; set; } = new List<SessionStep>();

        public List<SessionStep> PostSessionSteps { get; set; } = new List<SessionStep>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public SessionDefinition Clone()
        {
            var clone = (SessionDefinition)this.MemberwiseClone();

            clone.Recorders = Recorders?.Select(r => r.Clone()).ToList() ?? new List<SessionRecorderDefinition>();
            clone.Devices = Devices?.Select(d => d.Clone()).ToList();
            clone.Welcome = Welcome?.Clone() as WelcomeActionSettings;

            clone.PreSessionSteps = PreSessionSteps?.Select(s => s.Clone()).ToList() 
                                    ?? new List<SessionStep>();

            clone.SessionSteps = SessionSteps?.Select(s => s.Clone()).ToList() 
                                 ?? new List<SessionStep>();

            clone.PostSessionSteps = PostSessionSteps?.Select(s => s.Clone()).ToList() 
                                     ?? new List<SessionStep>();

            return clone; 
        }
    }
}
