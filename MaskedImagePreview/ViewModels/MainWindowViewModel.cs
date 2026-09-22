using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MaskedImagePreview.Utils;

namespace MaskedImagePreview.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase, IDropTarget
    {
        private string title = "Masked Image Preview";
        private ImageViewModel imageViewModel = new (string.Empty);
        private ImageListViewModel baseImageViewModel = new ();
        private ImageListViewModel maskImageViewModel = new ();

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
        }

        public MainWindowViewModel(AppSettings appSettings)
        {
            ImageViewModel.LoadImage(appSettings.DebugImagePath);
            AppLogger.Info($"Image loaded from {appSettings.DebugImagePath}");

            #if DEBUG

            var vm = new ImageViewModel(appSettings.DebugImagePath);
            BaseImageViewModel.ImageViewModels.Add(vm);
            BaseImageViewModel.SelectedImage = vm;

            var maskImage = new ImageViewModel(appSettings.DebugMaskPath, "mask");
            AppLogger.Info($"mask image load from {appSettings.DebugMaskPath}");
            MaskImageViewModel.ImageViewModels.Add(maskImage);
            MaskImageViewModel.SelectedImage = maskImage;

            #endif
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ImageViewModel ImageViewModel
        {
            get => imageViewModel;
            set => SetProperty(ref imageViewModel, value);
        }

        public ImageListViewModel BaseImageViewModel
        {
            get => baseImageViewModel;
            set => SetProperty(ref baseImageViewModel, value);
        }

        public ImageListViewModel MaskImageViewModel
        {
            get => maskImageViewModel;
            set => SetProperty(ref maskImageViewModel, value);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject dataObject && dataObject.ContainsFileDropList())
            {
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject dataObject && dataObject.ContainsFileDropList())
            {
                var files = dataObject.GetFileDropList();
                var filePath = files[0];

                AppLogger.Info($"Dropped file: {filePath}");

                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                // 画像の読み込み処理
                ImageViewModel.LoadImage(filePath);
            }
        }
    }
}