using System.Windows;
using System.Windows.Input;
using MaskedImagePreview.ViewModels;

namespace MaskedImagePreview.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private Point lastDragPosition;
    private bool isDragging;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Border_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
            vm.ImageViewModel.Scale = Math.Max(0.1, vm.ImageViewModel.Scale * zoomFactor);

            // var angleFactor = e.Delta > 0 ? 2 : -2;
            // vm.ImageViewModels.Angle += angleFactor;
            e.Handled = true; // スクロール防止
        }
    }

    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is UIElement element)
        {
            isDragging = true;
            lastDragPosition = e.GetPosition(ImageScrollViewer);
            element.CaptureMouse();
            e.Handled = true;
        }
    }

    private void Image_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging && sender is UIElement { IsMouseCaptured: true, })
        {
            var currentPosition = e.GetPosition(ImageScrollViewer);
            var deltaX = currentPosition.X - lastDragPosition.X;
            var deltaY = currentPosition.Y - lastDragPosition.Y;

            ImageScrollViewer.ScrollToHorizontalOffset(ImageScrollViewer.HorizontalOffset - deltaX);
            ImageScrollViewer.ScrollToVerticalOffset(ImageScrollViewer.VerticalOffset - deltaY);

            lastDragPosition = currentPosition;
        }
    }

    private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (isDragging && sender is UIElement element)
        {
            isDragging = false;
            element.ReleaseMouseCapture();
            e.Handled = true;
        }
    }

    private void Image_LostMouseCapture(object sender, MouseEventArgs e)
    {
        isDragging = false;
    }
}