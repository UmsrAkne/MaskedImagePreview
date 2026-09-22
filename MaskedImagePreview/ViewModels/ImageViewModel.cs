using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MaskedImagePreview.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageViewModel : BindableBase
    {
        private readonly string debugId;
        private string fullName = string.Empty;
        private ImageSource? imageSource;
        private double scale = 1.0;
        private double angle;
        private double offsetX;
        private double offsetY;

        public ImageViewModel(string path, string debugId = "")
        {
            FullName = path;
            LoadImage(path);
            this.debugId = debugId;
        }

        public string FullName { get => fullName; private set => SetProperty(ref fullName, value); }

        public string FileName => Path.GetFileName(FullName);

        public ImageSource? ImageSource { get => imageSource; private set => SetProperty(ref imageSource, value); }

        public double Scale { get => scale; set => SetProperty(ref scale, value); }

        public double Angle { get => angle; set => SetProperty(ref angle, value); }

        public double OffsetX
        {
            get => offsetX;
            set
            {
                if (debugId == "mask")
                {
                    Console.WriteLine($"ImageViewModel.OffsetX: {value}");
                }

                SetProperty(ref offsetX, value);
            }
        }

        public double OffsetY { get => offsetY; set => SetProperty(ref offsetY, value); }

        public void LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            FullName = path;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // ファイルロック防止
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.EndInit();
            bitmap.Freeze();

            ImageSource = bitmap;
        }

        public void ResetTransform()
        {
            Scale = 1.0;
            OffsetX = 0.0;
            OffsetY = 0.0;
            Angle = 0.0;
        }
    }
}