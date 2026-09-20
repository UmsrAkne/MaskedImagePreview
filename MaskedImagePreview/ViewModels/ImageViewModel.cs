using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MaskedImagePreview.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageViewModel : BindableBase
    {
        private string fullName = string.Empty;
        private ImageSource? imageSource;
        private double scale = 1.0;
        private double angle;

        public ImageViewModel(string path)
        {
            FullName = path;
            LoadImage(path);
        }

        public string FullName { get => fullName; private set => SetProperty(ref fullName, value); }

        public string FileName => Path.GetFileName(FullName);

        public ImageSource? ImageSource { get => imageSource; private set => SetProperty(ref imageSource, value); }

        public double Scale { get => scale; set => SetProperty(ref scale, value); }

        public double Angle { get => angle; set => SetProperty(ref angle, value); }

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
    }
}