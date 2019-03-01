using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Core.Common.Commands;
using UXC.Sessions.Models;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions.ViewModels.Timeline
{
    class ImageTimelineStepViewModel : ContentTimelineStepViewModelBase
    {
        private readonly ImageActionSettings _settings;
        private readonly IImageService _images;
        private readonly Dispatcher _dispatcher;

        public ImageTimelineStepViewModel(ImageActionSettings settings, IImageService images, Dispatcher dispatcher)
            : base(settings)
        {
            _settings = settings;
            _images = images;
            _dispatcher = dispatcher;

            if (settings.ShowContinue)
            {
                ContinueCommand = new RelayCommand(() => Complete());
                string label = settings.ContinueButtonLabel?.Trim();
                ContinueButtonLabel = String.IsNullOrWhiteSpace(label) ? null : label;
            }
        }


        public Visibility ContinueButtonVisibility => _settings.ShowContinue ? Visibility.Visible : Visibility.Collapsed;


        public ICommand ContinueCommand { get; } = NullCommand.Instance;


        public string ContinueButtonLabel { get; } = null;


        private BitmapSource imageSource;
        public BitmapSource ImageSource
        {
            get { return imageSource; }
            private set { Set(ref imageSource, value); }
        }


        private double width;
        public double Width
        {
            get { return width; }
            private set { Set(ref width, value); }
        }

        private double height;
        public double Height
        {
            get { return height; }
            private set { Set(ref height, value); }
        }


        private Stretch stretch = Stretch.Uniform;
        public Stretch Stretch
        {
            get { return stretch; }
            private set { Set(ref stretch, value); }
        }


        public override void Execute(SessionRecordingViewModel recording)
        {
            BitmapSource image;
            if (_images.TryGet(_settings.Path, out image))
            {
                UpdateSize(image);

                image.DownloadCompleted += Image_DownloadCompleted;

                ImageSource = image;
            }
            else
            {
                Width = 0;
                Height = 0;
            }
        }

        private void Image_DownloadCompleted(object sender, EventArgs e)
        {
            _dispatcher.Invoke(() => UpdateSize(imageSource));

        }

        private void UpdateSize(BitmapSource image)
        {
            if (image != null)
            {

                if (_settings.Stretch)
                {
                    Stretch = Stretch.Uniform;
                    Width = (double?)_settings.Width ?? Double.NaN;
                    Height = (double?)_settings.Height ?? Double.NaN;
                }
                else
                {
                    Stretch = Stretch.Fill;
                    Width = _settings.Width ?? image.PixelWidth;
                    Height = _settings.Height ?? image.PixelHeight;
                }
            }
        }



        public override SessionStepResult Complete()
        {
            var result = SessionStepResult.Successful; // TODO add the bounding box of image

            OnCompleted(result);

            if (imageSource != null)
            {
                imageSource.DownloadCompleted -= Image_DownloadCompleted;
            }

            return result;
        }
    }
}
