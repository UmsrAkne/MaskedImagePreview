using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MaskedImagePreview.Utils;

namespace MaskedImagePreview.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase, IDropTarget
    {
        private string title = "Masked Image Preview";

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

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

                // 画像の読み込み処理...
            }
        }
    }
}