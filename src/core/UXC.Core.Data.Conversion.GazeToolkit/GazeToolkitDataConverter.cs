using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Core.Data.Conversion.GazeToolkit
{
    public static class GazeDataEx
    {
        public static UXI.GazeToolkit.GazeDataValidity ToToolkit(this GazeDataValidity validity)
        {
            switch (validity)
            {
                case GazeDataValidity.Both:
                    return UXI.GazeToolkit.GazeDataValidity.Both;
                case GazeDataValidity.Left:
                    return UXI.GazeToolkit.GazeDataValidity.Left;
                case GazeDataValidity.ProbablyLeft:
                    return UXI.GazeToolkit.GazeDataValidity.ProbablyLeft;
                case GazeDataValidity.ProbablyRight:
                    return UXI.GazeToolkit.GazeDataValidity.ProbablyRight;
                case GazeDataValidity.Right:
                    return UXI.GazeToolkit.GazeDataValidity.Right;
                case GazeDataValidity.UnknownWhichOne:
                    return UXI.GazeToolkit.GazeDataValidity.UnknownWhichOne;
                case GazeDataValidity.None:
                default:
                    return UXI.GazeToolkit.GazeDataValidity.None;
            }
        }


        public static UXI.GazeToolkit.EyeGazeDataValidity ToToolkit(this EyeGazeDataValidity validity)
        {
            switch (validity)
            {
                case EyeGazeDataValidity.Probably:
                    return UXI.GazeToolkit.EyeGazeDataValidity.Probably;
                case EyeGazeDataValidity.Unknown:
                    return UXI.GazeToolkit.EyeGazeDataValidity.Unknown;
                case EyeGazeDataValidity.Valid:
                    return UXI.GazeToolkit.EyeGazeDataValidity.Valid;
                case EyeGazeDataValidity.Invalid:
                default:
                    return UXI.GazeToolkit.EyeGazeDataValidity.Invalid;
            }
        }


        public static UXI.GazeToolkit.GazeData ToToolkit(this GazeData gazeData)
        {
            return new UXI.GazeToolkit.GazeData
            (
                gazeData.LeftEye.ToToolkit(),
                gazeData.RightEye.ToToolkit(),
                gazeData.TrackerTicks,
                gazeData.Timestamp.TimeOfDay
            );
        }


        public static UXI.GazeToolkit.EyeGazeData ToToolkit(this EyeGazeData gazeData)
        {
            return new UXI.GazeToolkit.EyeGazeData
            (
                gazeData.Validity.ToToolkit(),
                gazeData.GazePoint2D.ToToolkit(),
                gazeData.GazePoint3D.ToToolkit(),
                gazeData.EyePosition3D.ToToolkit(),
                gazeData.EyePosition3DRelative.ToToolkit(),
                gazeData.PupilDiameter
            );
        }


        public static UXI.GazeToolkit.Point2 ToToolkit(this Point2 point)
        {
            return new UXI.GazeToolkit.Point2(point.X, point.Y);
        }


        public static UXI.GazeToolkit.Point3 ToToolkit(this Point3 point)
        {
            return new UXI.GazeToolkit.Point3(point.X, point.Y, point.Z);
        }
    }
}
