using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UXC.Core.Data;
using UXC.Sessions.Common;

namespace UXC.Sessions.Serialization.Converters.Json
{
    public class DataJsonConverters : IEnumerable<JsonConverter>
    {
        public static IEnumerable<JsonConverter> Converters { get; } = new List<JsonConverter>(PointsJsonConverters.Converters)
        {
            new StringEnumConverter(camelCaseText: false),
            new EyeGazeDataJsonConverter(),
            new GazeDataJsonConverter(),
            new ExternalEventDataConverter()
        };


        public IEnumerator<JsonConverter> GetEnumerator()
        {
            return Converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class GazeDataJsonConverter : JsonConverter<GazeData>
    {
        protected override GazeData Convert(JToken token, JsonSerializer serializer)
        {
            var jObject = (JObject)token;

            var validity = jObject[nameof(GazeData.Validity)].ToObject<GazeDataValidity>(serializer);
            var trackerTicks = jObject[nameof(GazeData.TrackerTicks)].ToObject<long>(serializer);
            var leftEye = jObject[nameof(GazeData.LeftEye)].ToObject<EyeGazeData>(serializer);
            var rightEye = jObject[nameof(GazeData.RightEye)].ToObject<EyeGazeData>(serializer);
            var timestamp = jObject[nameof(GazeData.Timestamp)].ToObject<DateTime>(serializer);

            return new GazeData(validity, leftEye, rightEye, trackerTicks, timestamp);
        }
    }


    public class EyeGazeDataJsonConverter : JsonConverter<EyeGazeData>
    {
        protected override EyeGazeData Convert(JToken token, JsonSerializer serializer)
        {
            var jObject = (JObject)token;

            var validity = jObject[nameof(EyeGazeData.Validity)].ToObject<EyeGazeDataValidity>(serializer);
            var gazePoint2D = jObject[nameof(EyeGazeData.GazePoint2D)].ToObject<Point2>(serializer);
            var gazePoint3D = jObject[nameof(EyeGazeData.GazePoint3D)].ToObject<Point3>(serializer);
            var eyePosition3D = jObject[nameof(EyeGazeData.EyePosition3D)].ToObject<Point3>(serializer);
            var eyePosition3DRelative = jObject[nameof(EyeGazeData.EyePosition3DRelative)].ToObject<Point3>(serializer);
            var pupilDiameter = jObject[nameof(EyeGazeData.PupilDiameter)].ToObject<double>(serializer);

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


    public class ExternalEventDataConverter : JsonConverter<ExternalEventData>
    {
        protected override ExternalEventData Convert(JToken token, JsonSerializer serializer)
        {
            var jObject = (JObject)token;

            string source = jObject[nameof(ExternalEventData.Source)].ToObject<string>(serializer);
            string eventName = jObject[nameof(ExternalEventData.EventName)].ToObject<string>(serializer);
            string eventData = jObject[nameof(ExternalEventData.EventData)].ToObject<string>(serializer);
            DateTime timestamp = jObject[nameof(ExternalEventData.Timestamp)].ToObject<DateTime>(serializer);
            DateTime? validTill = jObject[nameof(ExternalEventData.ValidTill)].ToObject<DateTime?>(serializer);

            return new ExternalEventData(source, eventName, eventData, timestamp, validTill);
        }
    }
}
