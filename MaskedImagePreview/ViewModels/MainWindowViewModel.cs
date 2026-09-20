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

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
        }

        public MainWindowViewModel(AppSettings appSettings)
        {
            ImageViewModel.LoadImage(appSettings.DebugImagePath);
            AppLogger.Info($"Image loaded from {appSettings.DebugImagePath}");
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ImageViewModel ImageViewModel
        {
            get => imageViewModel;
            set => SetProperty(ref imageViewModel, value);
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