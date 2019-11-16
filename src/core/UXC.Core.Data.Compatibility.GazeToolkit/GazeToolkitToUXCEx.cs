/**
 * UXC.Core.Data.Compatibility.GazeToolkit
 * Copyright (c) 2019 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data.Compatibility.GazeToolkit
{
    public static class GazeToolkitToUXCEx
    {
        public static EyeGazeDataValidity ToUXC(this UXI.GazeToolkit.EyeValidity validity)
        {
            switch (validity)
            {
                case UXI.GazeToolkit.EyeValidity.Probably:
                    return EyeGazeDataValidity.Probably;
                case UXI.GazeToolkit.EyeValidity.Unknown:
                    return EyeGazeDataValidity.Unknown;
                case UXI.GazeToolkit.EyeValidity.Valid:
                    return EyeGazeDataValidity.Valid;
                case UXI.GazeToolkit.EyeValidity.Invalid:
                default:
                    return EyeGazeDataValidity.Invalid;
            }
        }


        public static GazeData ToUXC(this UXI.GazeToolkit.GazeData gazeData, Func<UXI.GazeToolkit.EyeData, Point3> getEyePosition3DRelative, long trackerTicks)
        {
            var leftEye = gazeData.LeftEye.ToUXC(getEyePosition3DRelative(gazeData.LeftEye));
            var rightEye = gazeData.RightEye.ToUXC(getEyePosition3DRelative(gazeData.RightEye));

            var validity = EyeValidityEx2.MergeToEyeValidity(leftEye.Validity, rightEye.Validity);

            return new GazeData
            (
                validity,
                leftEye,
                rightEye,
                trackerTicks,
                gazeData.Timestamp.DateTime
            );

        }


        public static EyeGazeData ToUXC(this UXI.GazeToolkit.EyeData gazeData, Point3 eyePosition3DRelative)
        {
            return new EyeGazeData
            (
                gazeData.Validity.ToUXC(),
                gazeData.GazePoint2D.ToUXC(),
                gazeData.GazePoint3D.ToUXC(),
                gazeData.EyePosition3D.ToUXC(),
                eyePosition3DRelative,
                gazeData.PupilDiameter
            );
        }


        public static Point2 ToUXC(this UXI.GazeToolkit.Point2 point)
        {
            return new Point2(point.X, point.Y);
        }


        public static Point3 ToUXC(this UXI.GazeToolkit.Point3 point)
        {
            return new Point3(point.X, point.Y, point.Z);
        }
    }
}
