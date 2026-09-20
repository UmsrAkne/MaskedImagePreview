using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MaskedImagePreview.Utils;

namespace MaskedImagePreview.ViewModels
{
    public class ImageListViewModel : BindableBase, IDropTarget
    {
        private ObservableCollection<ImageViewModel>? imageViewModels = new ();
        private ImageViewModel? selectedImage;

        public ObservableCollection<ImageViewModel>? ImageViewModels
        {
            get => imageViewModels;
            set => SetProperty(ref imageViewModels, value);
        }

        public ImageViewModel? SelectedImage { get => selectedImage; set => SetProperty(ref selectedImage, value); }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject dataObject && dataObject.ContainsFileDropList())
            {
                var files = dataObject.GetFileDropList().Cast<string>()
                    .Where(File.Exists)
                    .Where(f => Path.GetExtension(f).Equals(".png", StringComparison.CurrentCultureIgnoreCase))
                    .Select(f => new ImageViewModel(f))
                    .ToList();

                foreach (var imageViewModel in files)
                {
                    AppLogger.Info($"Dropped file (ImageListViewModel): {imageViewModel.FullName}");
                }

                ImageViewModels.AddRange(files);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject dataObject && dataObject.ContainsFileDropList())
            {
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            }
        }
    }
}