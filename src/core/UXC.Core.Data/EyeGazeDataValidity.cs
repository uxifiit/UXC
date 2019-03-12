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
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    public enum EyeGazeDataValidity : byte
    {
        Invalid = GazeDataValidity.None,
        Valid = GazeDataValidity.Left,
        Probably = GazeDataValidity.ProbablyLeft,
        Unknown = GazeDataValidity.UnknownWhichOne,
        AllMask = Invalid | Valid | Probably | Unknown
    }

    public static class EyeValidityEx2
    {
        private const EyeGazeDataValidity HasEyeMask = EyeGazeDataValidity.Valid | EyeGazeDataValidity.Probably;
        public static bool HasEye(this EyeGazeDataValidity validity)
        {
            return (validity & HasEyeMask) != EyeGazeDataValidity.Invalid;
        }

        private static GazeDataValidity AlignToRightEyeValidity(this EyeGazeDataValidity validity)
        {
            int rightEyeValidity = ((byte)(validity & EyeGazeDataValidity.Valid) << 4)
                                 | ((byte)(validity & EyeGazeDataValidity.Probably) << 2)
                                 | ((byte)(validity & EyeGazeDataValidity.Unknown));

            return (GazeDataValidity)rightEyeValidity & GazeDataValidity.RightEyeMask;
        }

        private static GazeDataValidity ToLeftEyeValidity(this EyeGazeDataValidity validity)
        {
            return (GazeDataValidity)validity & GazeDataValidity.LeftEyeMask;
        }

        public static EyeGazeDataValidity GetLeftEyeValidity(this GazeDataValidity validity)
        {
            return (EyeGazeDataValidity)validity & EyeGazeDataValidity.AllMask;
        }

        public static EyeGazeDataValidity GetRightEyeValidity(this GazeDataValidity validity)
        {
            var rightEyeValidity = ((byte)(validity & GazeDataValidity.ProbablyRight) >> 2)
                                 | ((byte)(validity & GazeDataValidity.Right) >> 4)
                                 | ((byte)(validity & GazeDataValidity.UnknownWhichOne));

            return (EyeGazeDataValidity)rightEyeValidity & EyeGazeDataValidity.AllMask;
        }

        public static GazeDataValidity MergeToEyeValidity(this EyeGazeDataValidity leftEyeValidity, EyeGazeDataValidity rightEyeValidity)
        {
            return (leftEyeValidity.ToLeftEyeValidity() | rightEyeValidity.AlignToRightEyeValidity()) & GazeDataValidity.AllMask;
        }
    }
}
