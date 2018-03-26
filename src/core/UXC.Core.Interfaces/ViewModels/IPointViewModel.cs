using System.ComponentModel;

namespace UXC.Core.ViewModels
{
    public interface IPointViewModel : INotifyPropertyChanged
    {
        bool IsVisible { get; }
        double X { get; }
        double Y { get; }
    }
}
