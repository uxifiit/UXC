using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXI.Common.UI;
using UXC.Core.ViewModels;
using UXC.Core.Data;

namespace UXC.Core.ViewModels
{
    public class PointViewModel : BindableBase, IPointViewModel
    {
        public void UpdatePosition(double x, double y)
        {
            X = x;
            Y = y;
        }


        private double x = 0d;
        public double X { get { return x; } private set { Set(ref x, value); } }


        private double y = 0d;
        public double Y { get { return y; } private set { Set(ref y, value); } }


        public Visibility Visibility => IsVisible ? Visibility.Visible : Visibility.Hidden;


        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (Set(ref isVisible, value))
                {
                    OnPropertyChanged(nameof(Visibility));
                }
            }
        }


        private double size = 10d;
        public double Size
        {
            get { return size; }
            set { Set(ref size, value); }
        }
    }
}
