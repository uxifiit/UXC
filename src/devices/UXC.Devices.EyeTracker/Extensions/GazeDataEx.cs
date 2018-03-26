using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Extensions
{
    public static class GazeDataEx
    {
        /// <summary>
        /// Gets the eye distance for both, single, or none eye based on the <seealso cref="GazeData.Validity"/>.
        /// The distance is retrieved from the eye 3D position in millimeters in the User Coordinate System 
        /// (with origin at the centre of the frontal surface of the eye tracker). 
        /// </summary>
        /// <param name="gazeData"></param>
        /// <returns>Mean distance if both eye are valid or eye distance for the only valid eye; otherwise null, if no eye is valid.</returns>
        public static double? GetEyeDistance(this GazeData gazeData)
        {
            if (gazeData.Validity == GazeDataValidity.Both)
            {
                return (gazeData.LeftEye.EyePosition3D.Z + gazeData.RightEye.EyePosition3D.Z) / 2d;
            }
            else if (gazeData.Validity.HasLeftEye())
            {
                return gazeData.LeftEye.EyePosition3D.Z; 
            }
            else if (gazeData.Validity.HasRightEye())
            {
                return gazeData.RightEye.EyePosition3D.Z;
            }

            return null;
        }

        /// <summary>
        /// Gets the relative eye distance for both, single, or none eye based on the <seealso cref="GazeData.Validity"/>.
        /// The distance is retrieved from the 3D eye position relative to the eye tracker trackbox (Trackbox Coordinate System).
        /// </summary>
        /// <param name="gazeData"></param>
        /// <returns>Mean relative distance if both eye are valid or single relative eye distance for the only valid eye; otherwise null, if no eye is valid.</returns>
        public static double? GetRelativeEyeDistance(this GazeData gazeData)
        {
            if (gazeData.Validity == GazeDataValidity.Both)
            {
                return (gazeData.LeftEye.EyePosition3DRelative.Z + gazeData.RightEye.EyePosition3DRelative.Z) / 2d;
            }
            else if (gazeData.Validity.HasLeftEye())
            {
                return gazeData.LeftEye.EyePosition3DRelative.Z;
            }
            else if (gazeData.Validity.HasRightEye())
            {
                return gazeData.RightEye.EyePosition3DRelative.Z;
            }

            return null;
        }
    }
}
