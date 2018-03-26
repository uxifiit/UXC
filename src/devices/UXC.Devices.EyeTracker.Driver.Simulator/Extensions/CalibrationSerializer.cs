using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;
using UXC.Devices.EyeTracker.Calibration;

namespace UXC.Devices.EyeTracker.Driver.Simulator.Extensions
{
    internal static class CalibrationSerializer
    {
        private const int POINTS_COUNT = 3 * 2;
        private const int STATES_COUNT = 2;

        private const int POINT_SIZE = sizeof(double);
        private const int STATE_SIZE = sizeof(int);

        private const int POINTS_BLOCK_SIZE = POINTS_COUNT * POINT_SIZE;
        private const int STATES_BLOCK_SIZE = STATES_COUNT * STATE_SIZE;


        private static byte[] ToBytesBlock(Point2 truePosition, CalibrationSample result)
        {
            double[] points = Enumerable.Empty<double>() // POINTS_COUNT
                                        .Concat(result.LeftEye.Point.AsEnumerable())
                                        .Concat(result.RightEye.Point.AsEnumerable())
                                        .Concat(truePosition.AsEnumerable())
                                        .ToArray();

            int[] states = new int[] { (int)result.LeftEye.Status, (int)result.RightEye.Status };  // STATES_COUNT

            var bytes = Enumerable.Empty<byte>()
                                  .Concat(BytesConverter.Convert(points))
                                  .Concat(BytesConverter.Convert(states))
                                  .ToArray();

            return bytes;
        }

        public static byte[] Serialize(IEnumerable<CalibrationPointResult> calibrationResult)
        {
            return calibrationResult.SelectMany(r => r.Samples.SelectMany(s => ToBytesBlock(r.TruePosition, s))).ToArray();
        }

        public static IEnumerable<CalibrationPointResult> Deserialize(byte[] bytes)
        {
            int length = bytes.Length;
            int offset = 0;

            double[] points = new double[POINTS_COUNT];
            int[] states = new int[STATES_COUNT];

            Point2 truePosition = new Point2(-1, -1);
            List<CalibrationSample> samples = null;

            while (offset < length)
            {
                Buffer.BlockCopy(bytes, offset, points, 0, POINTS_BLOCK_SIZE);
                offset += POINTS_BLOCK_SIZE;

                Buffer.BlockCopy(bytes, offset, states, 0, STATES_BLOCK_SIZE);
                offset += STATES_BLOCK_SIZE;

                var newTruePosition = new Point2(points[4], points[5]);

                if (samples == null)
                {
                    truePosition = newTruePosition;
                    samples = new List<CalibrationSample>();
                }
                else if (truePosition != newTruePosition && samples != null)
                {
                    yield return new CalibrationPointResult(truePosition, samples);

                    truePosition = newTruePosition;
                    samples = new List<CalibrationSample>();
                }

                samples.Add(new CalibrationSample
                (
                    new CalibrationEyeSample(new Point2(points[0], points[1]), (CalibrationSampleStatus)states[0]),
                    new CalibrationEyeSample(new Point2(points[2], points[3]), (CalibrationSampleStatus)states[1])
                ));
            }

            if (samples != null)
            {
                yield return new CalibrationPointResult(truePosition, samples);
            }
        }
    }
}
