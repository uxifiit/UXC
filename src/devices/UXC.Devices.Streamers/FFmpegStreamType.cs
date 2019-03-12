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

namespace UXC.Devices.Streamers
{
    /// <summary>
    /// Defines enumeration of stream types provided by FFmpeg. Instances of this enumeration may be implicitly cast to string. 
    /// </summary>
    public class FFmpegStreamType
    {
        /// <summary>
        /// Gets the instance of <seealso cref="FFmpegStreamType"/> for audio stream type.
        /// </summary>                                            
        public static readonly FFmpegStreamType AUDIO = new FFmpegStreamType("audio");
        /// <summary>
        /// Gets the instance of <seealso cref="FFmpegStreamType"/> for video stream type.
        /// </summary>
        public static readonly FFmpegStreamType VIDEO = new FFmpegStreamType("video");

        public static IEnumerable<FFmpegStreamType> Types
        {
            get
            {
                yield return AUDIO;
                yield return VIDEO;        
            }
        }

        private readonly string _type;
        private FFmpegStreamType(string type)
        {
            _type = type;
        }

        public override string ToString()
        {
            return _type;
        }

        public static implicit operator string(FFmpegStreamType streamType)
        {
            return streamType._type;        
        }
    }
}
