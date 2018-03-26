using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UXC.Core.ViewModels
{
    public class PointsSequence : IEnumerable<Point>
    {
        private readonly List<Point> _points;

        public PointsSequence(IEnumerable<Point> points)
        {
            _points = points?.ToList() ?? new List<Point>();
            Length = _points.Count();
        }

        public int Length { get; }

        public IEnumerator<Point> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
