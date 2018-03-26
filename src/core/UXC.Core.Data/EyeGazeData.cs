using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    public class EyeGazeData
    {
        public static readonly EyeGazeData Empty = new EyeGazeData();

        private EyeGazeData() { }

        public EyeGazeData
        (
             EyeGazeDataValidity validity,
             Point2 gazePoint2D,
             Point3 gazePoint3D,
             Point3 eyePosition3D,
             Point3 eyePosition3DRelative,
             double pupilDiameter
        )
        {
            Validity = validity;
            GazePoint2D = gazePoint2D;
            GazePoint3D = gazePoint3D;
            EyePosition3D = eyePosition3D;
            EyePosition3DRelative = eyePosition3DRelative;
            PupilDiameter = pupilDiameter;
        }

        public EyeGazeDataValidity Validity { get; } = EyeGazeDataValidity.Invalid;

        public Point2 GazePoint2D { get; }

        public Point3 GazePoint3D { get; } 

        public Point3 EyePosition3D { get; } 

        public Point3 EyePosition3DRelative { get; } 

        public double PupilDiameter { get; } 
    }
}
