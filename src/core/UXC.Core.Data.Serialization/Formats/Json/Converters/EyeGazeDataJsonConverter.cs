/**
 * UXC.Core.Data.Serialization
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;

namespace UXC.Core.Data.Serialization.Formats.Json.Converters
{
    public class EyeGazeDataJsonConverter : GenericJsonConverter<EyeGazeData>
    {
        protected override EyeGazeData Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var validity              = obj.GetValue<EyeGazeDataValidity>(nameof(EyeGazeData.Validity), serializer);
            var gazePoint2D           = obj.GetValue<Point2>(nameof(EyeGazeData.GazePoint2D), serializer);
            var gazePoint3D           = obj.GetValue<Point3>(nameof(EyeGazeData.GazePoint3D), serializer);
            var eyePosition3D         = obj.GetValue<Point3>(nameof(EyeGazeData.EyePosition3D), serializer);
            var eyePosition3DRelative = obj.GetValue<Point3>(nameof(EyeGazeData.EyePosition3DRelative), serializer);
            var pupilDiameter         = obj.GetValue<double>(nameof(EyeGazeData.PupilDiameter), serializer);

            return new EyeGazeData
            (
                validity,
                gazePoint2D,
                gazePoint3D,
                eyePosition3D,
                eyePosition3DRelative,
                pupilDiameter
            );
        }
    }
}
