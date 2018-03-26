using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXC.Core.Data.Serialization.Csv.ClassMaps;

namespace UXC.Core.Data.Serialization.Csv.Test
{
    [TestClass]
    public class CsvDataWriterTest
    {
        private CsvSerializationFactory _factory;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new CsvSerializationFactory(DataClassMaps.Maps);
        }

        [TestMethod]
        public void TestWritePoint2Header()
        {
            StringBuilder builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            using (var writer = _factory.CreateWriterForType(stringWriter, typeof(Point2)))
            {
            }

            string expected = "X,Y\r\n";

            Assert.AreEqual(expected, builder.ToString());
        }

        [TestMethod]
        public void TestWritePoint3DHeader()
        {
            StringBuilder builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            using (var writer = _factory.CreateWriterForType(stringWriter, typeof(Point3)))
            {
            }

            string expected = "X,Y,Z\r\n";

            Assert.AreEqual(expected, builder.ToString());
        }


        [TestMethod]
        public void TestWriteEyeGazeDataHeader()
        {
            StringBuilder builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            using (var writer = _factory.CreateWriterForType(stringWriter, typeof(EyeGazeData)))
            {
            }

            string expected = "Validity,GazePoint2D.X,GazePoint2D.Y,GazePoint3D.X,GazePoint3D.Y,GazePoint3D.Z,EyePosition3D.X,EyePosition3D.Y,EyePosition3D.Z,EyePosition3DRelative.X,EyePosition3DRelative.Y,EyePosition3DRelative.Z,PupilDiameter\r\n";

            Assert.AreEqual(expected, builder.ToString());
        }


        [TestMethod]
        public void TestWriteGazeDataHeader()
        {
            StringBuilder builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            using (var writer = _factory.CreateWriterForType(stringWriter, typeof(GazeData)))
            {
            }

            Func<string, string> eyeHeaderPart = (eye) => $"{eye}.Validity,{eye}.GazePoint2D.X,{eye}.GazePoint2D.Y,{eye}.GazePoint3D.X,{eye}.GazePoint3D.Y,{eye}.GazePoint3D.Z,{eye}.EyePosition3D.X,{eye}.EyePosition3D.Y,{eye}.EyePosition3D.Z,{eye}.EyePosition3DRelative.X,{eye}.EyePosition3DRelative.Y,{eye}.EyePosition3DRelative.Z,{eye}.PupilDiameter";

            string expected = $"TrackerTicks,Validity,TimeStamp,{eyeHeaderPart("LeftEye")},{eyeHeaderPart("RightEye")}\r\n";

            Assert.AreEqual(expected, builder.ToString());
        }
    }
}
