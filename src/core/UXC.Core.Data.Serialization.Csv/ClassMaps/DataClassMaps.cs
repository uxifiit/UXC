using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using UXC.Core.Data;

namespace UXC.Core.Data.Serialization.Csv.ClassMaps
{
    public class DataClassMaps : IEnumerable<ClassMap>
    {
        public static readonly IEnumerable<ClassMap> Maps = new List<ClassMap>()
        {
            new Point2ClassMap(),
            new Point3ClassMap(),
            new EyeGazeDataClassMap(),
            new GazeDataClassMap(),
            new ExternalEventDataClassMap(),
            new KeyboardEventDataClassMap(),
            new MouseEventDataClassMap()
        };

        public IEnumerator<ClassMap> GetEnumerator()
        {
            return Maps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    class Point2ClassMap : ClassMap<Point2>
    {
        public Point2ClassMap()
        {
            AutoMap();
        }
    }


    class Point3ClassMap : ClassMap<Point3>
    {
        public Point3ClassMap()
        {
            AutoMap();
        }
    }


    class EyeGazeDataClassMap : ClassMap<EyeGazeData>
    {
        public EyeGazeDataClassMap()
        {
            AutoMap();

            References<Point2ClassMap>(m => m.GazePoint2D).Prefix();
            References<Point3ClassMap>(m => m.GazePoint3D).Prefix();
            References<Point3ClassMap>(m => m.EyePosition3D).Prefix();
            References<Point3ClassMap>(m => m.EyePosition3DRelative).Prefix();
        }
    }


    class GazeDataClassMap : ClassMap<GazeData>
    {
        public GazeDataClassMap()
        {
            Map(m => m.TrackerTicks);
            Map(m => m.Validity);

            References<EyeGazeDataClassMap>(m => m.LeftEye).Prefix();
            References<EyeGazeDataClassMap>(m => m.RightEye).Prefix();
        }
    }


    class ExternalEventDataClassMap : ClassMap<ExternalEventData>
    {
        public ExternalEventDataClassMap() 
        {
            AutoMap();
        }
    }


    class KeyboardEventDataClassMap : ClassMap<KeyboardEventData>
    {
        public KeyboardEventDataClassMap()
        {
            AutoMap();
        }
    }


    class MouseEventDataClassMap : ClassMap<MouseEventData>
    {
        public MouseEventDataClassMap()
        {
            AutoMap();
        }
    }
}
