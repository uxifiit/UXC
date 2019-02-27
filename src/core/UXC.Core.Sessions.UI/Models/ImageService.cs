using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UXC.Sessions.Models
{
    public class ImageService : IImageService
    {
        private readonly Dictionary<Uri, BitmapSource> _images = new Dictionary<Uri, BitmapSource>();

        public void Add(string path)
        {
            var uriSource = new Uri(path);

            if (_images.ContainsKey(uriSource) == false)
            {
                var image = BitmapFrame.Create(uriSource, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.Default);
            
                _images.Add(uriSource, image);
            }
        }

        public bool TryGet(string path, out BitmapSource image)
        {
            var uriSource = new Uri(path);

            return _images.TryGetValue(uriSource, out image);
        }


        public bool Contains(string path)
        {
            var uriSource = new Uri(path);

            return _images.ContainsKey(uriSource);
        }


        public void Clear()
        {
            _images.Clear();
        }
    }
}
