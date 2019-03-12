/**
 * UXC.Core.Data
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    /// <summary>
    /// Class containing gaze data, based on Tobii Gaze Data. Gaze Data from other manufacturers is mapped into this class.
    /// </summary>
    public class GazeData : DeviceData
    {
        public static readonly GazeData Empty = new GazeData(GazeDataValidity.None, EyeGazeData.Empty, EyeGazeData.Empty, 0, DateTime.MinValue); 

        public GazeData(GazeDataValidity validity, EyeGazeData leftEye, EyeGazeData rightEye, long trackerTicks, DateTime timestamp)
            : base(timestamp)
        {
            Validity = validity;
            LeftEye = leftEye;
            RightEye = rightEye;
            TrackerTicks = trackerTicks;
        }

        ///<summary>
        ///Time when data was sampled by the EyeTracker. Microseconds from arbitrary point in time.
        ///</summary>
        public long TrackerTicks { get; }

        
        public GazeDataValidity Validity { get; }


        public EyeGazeData LeftEye { get; }


        public EyeGazeData RightEye { get; }
    }
}
