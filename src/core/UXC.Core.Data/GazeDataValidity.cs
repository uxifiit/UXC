using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    public enum GazeDataValidity : byte
    {
        None = 0,
        Left = 1,
        ProbablyLeft = 2,
        UnknownWhichOne = 4,
        ProbablyRight = 8,
        Right = 16,
        Both = Left | Right,

        LeftEyeMask = Left | ProbablyLeft | UnknownWhichOne,
        RightEyeMask = Right | ProbablyRight | UnknownWhichOne,
        AllMask = LeftEyeMask | RightEyeMask
    }


    public static class EyeValidityEx
    {
        private const GazeDataValidity HasLeftEyeMask = GazeDataValidity.Left | GazeDataValidity.ProbablyLeft;
        private const GazeDataValidity HasRightEyeMask = GazeDataValidity.Right | GazeDataValidity.ProbablyRight;

        public static bool HasLeftEye(this GazeDataValidity validity)
        {
            return (validity & HasLeftEyeMask) != GazeDataValidity.None;
        }

        public static bool HasRightEye(this GazeDataValidity validity)
        {
            return (validity & HasRightEyeMask) != GazeDataValidity.None;
        }
    }
}
