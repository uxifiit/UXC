using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Models
{
    /// <summary>
    /// Class representing a point in 2D space. Weird name because all the good names are used in SDKs from tracker manufacturers.
    /// </summary>
    ///   
    [Serializable]
    public class PointTwoD
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Creates an empty instance of the PointTwoD class.
        /// </summary>
        public PointTwoD()
            : this(0d, 0d)
        {
            
        }

        /// <summary>
        /// Creates an instance of the PointTwoD with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public PointTwoD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Creates an instance of the PointTwoD as a copy of the given instance.
        /// </summary>
        public PointTwoD(PointTwoD point)
            : this(point.X, point.Y)
        {
            
        }

        /// <summary>
        /// Returns the coordinates in a handy String.
        /// </summary>
        /// <returns>Handy string with the x and y coordinates split by a semicolon</returns>
        public override String ToString()
        {
            return X + ";" + Y;
        }



        /// <summary>
        /// Returns distance between this and another point.
        /// </summary>
        /// <param name="?"></param>
        public double DistanceFrom(PointTwoD other)
        {
            double difx = this.X - other.X;
            double dify = this.Y - other.Y;
            return Math.Sqrt(difx * difx + dify * dify);
        }
    }

    /// <summary>
    /// Class representing a point in 3D space. Weird name because all the good names are used in SDKs from tracker manufacturers.
    /// </summary>
    [Serializable]
    public class PointThreeD
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
       
        public double X { get; set; }
        /// <summary>
        /// Y coordinate.
        /// </summary>

        public double Y { get; set; }
        /// <summary>
        /// Z coordinate.
        /// </summary>

        public double Z { get; set; }

        /// <summary>
        /// Creates an empty instance of the PointThreeD class.
        /// </summary>
        public PointThreeD() 
          : this(0d, 0d, 0d) 
        { }

        /// <summary>
        /// Creates an instance of the PointThreeD with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public PointThreeD(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns the coordinates in a handy String.
        /// </summary>
        /// <returns>X, Y and Z coordinates split by semicolons</returns>
        public override String ToString()
        {
            return X + ";" + Y + ";" + Z + "";
        }
    }
}
