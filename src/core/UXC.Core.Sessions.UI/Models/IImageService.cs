using System.Windows.Media.Imaging;

namespace UXC.Sessions.Models
{
    public interface IImageService
    {
        void Add(string path);
        void Clear();
        bool Contains(string path);
        bool TryGet(string path, out BitmapSource image);
    }
}