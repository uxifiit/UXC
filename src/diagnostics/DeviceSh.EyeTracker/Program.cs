/**
 * DeviceSh.EyeTracker
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
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXC.Core.Modules;
using UXC.Devices.EyeTracker;
using UXC.Devices.EyeTracker.Configuration;

namespace DeviceSh.EyeTracker
{
    class EyeTrackerConfiguration : IEyeTrackerConfiguration
    {
        public string SelectedDriver
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }

    class NullModulesService : IModulesService
    {
        public void Register<T>(object target, Action<IEnumerable<T>> callback)
        {
        }

        public void Unregister(object target)
        {
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            JsonSerializer serializer = new JsonSerializer();

            var finder = new UXC.Devices.EyeTracker.TobiiPro.TobiiProFinder();
            var config = new EyeTrackerConfiguration();
            var modules = new NullModulesService();

            var browser = new TrackerBrowser(new List<ITrackerFinder>() { finder }, config, modules);
            var device = new EyeTrackerDevice(browser);

            bool connected = device.ConnectToDevice();

            device.Log += Device_Log;

            if (connected)
            {
                device.DumpInfo();

                device.DisconnectDevice();

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Failed to connect to tracker.");
            }
        }

        private static void Device_Log(UXC.Core.Devices.IDevice device, UXC.Core.LogMessage info)
        {
            Console.WriteLine($"{info.Level} {info.Tag} {info.Message}");
            if (info.Content != null)
            {
                var content = JsonConvert.SerializeObject(info.Content, Formatting.Indented);
                Console.WriteLine(content);
            }
        }
    }
}
