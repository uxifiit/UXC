/**
 * UXC.Devices.Streamers
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

namespace UXC.Devices.Streamers.Configuration
{
    internal class VideoStreamerRecordingPreset
    {
        public static readonly int DEFAULT_FrameRate_LOW = 15;
        public static readonly int DEFAULT_FrameRate_SD = 15;
        public static readonly int DEFAULT_FrameRate_HDR = 30;
        public static readonly int DEFAULT_FrameRate_HD = 60;
        public static readonly int DEFAULT_FrameRate_Source = 60;


        public static readonly string DEFAULT_ResolutionScale_LOW = "-2:360";
        public static readonly string DEFAULT_ResolutionScale_SD = "-2:480";
        public static readonly string DEFAULT_ResolutionScale_HDR = "-2:720";
        public static readonly string DEFAULT_ResolutionScale_HD = "-2:1080";
        public static readonly string DEFAULT_ResolutionScale_Source = "iw:ih";

        public static readonly int DEFAULT_Bitrate_LOW = 750;
        public static readonly int DEFAULT_Bitrate_SD = 1000;
        public static readonly int DEFAULT_Bitrate_HDR = 2100;
        public static readonly int DEFAULT_Bitrate_HD = 3000;
        public static readonly int DEFAULT_Bitrate_Source = 3000;

        public static readonly string DEFAULT_Preset_LOW = "Low";
        public static readonly string DEFAULT_Preset_SD = "SD";
        public static readonly string DEFAULT_Preset_HDR = "HDR";
        public static readonly string DEFAULT_Preset_HD = "HD";
        public static readonly string DEFAULT_Preset_Source = "Source";

        public static IEnumerable<VideoStreamerRecordingPreset> Presets { get; } = new VideoStreamerRecordingPreset[]
        {
              Low = new VideoStreamerRecordingPreset(DEFAULT_Preset_LOW, DEFAULT_ResolutionScale_LOW, DEFAULT_FrameRate_LOW, DEFAULT_Bitrate_LOW),
              SD = new VideoStreamerRecordingPreset(DEFAULT_Preset_SD , DEFAULT_ResolutionScale_SD , DEFAULT_FrameRate_SD , DEFAULT_Bitrate_SD ),
              HDR = new VideoStreamerRecordingPreset(DEFAULT_Preset_HDR, DEFAULT_ResolutionScale_HDR, DEFAULT_FrameRate_HDR, DEFAULT_Bitrate_HDR),
              HD = new VideoStreamerRecordingPreset(DEFAULT_Preset_HD , DEFAULT_ResolutionScale_HD , DEFAULT_FrameRate_HD , DEFAULT_Bitrate_HD ),
              Source = new VideoStreamerRecordingPreset(DEFAULT_Preset_Source , DEFAULT_ResolutionScale_Source , DEFAULT_FrameRate_Source , DEFAULT_Bitrate_Source ),
        };

        public static VideoStreamerRecordingPreset Low { get; private set; }
        public static VideoStreamerRecordingPreset SD { get; private set; } 
        public static VideoStreamerRecordingPreset HDR { get; private set; }
        public static VideoStreamerRecordingPreset HD { get; private set; }
        public static VideoStreamerRecordingPreset Source { get; private set; }


        public VideoStreamerRecordingPreset(string name, string resolutionScale, int framerate, int bitrate)
        {
            Name = name;
            ResolutionScale = resolutionScale;
            Bitrate = bitrate;
            FrameRate = framerate;
        }
        public string Name { get; }
        public int FrameRate { get; }
        public int Bitrate { get; }
        public string ResolutionScale { get; }
    }
}
