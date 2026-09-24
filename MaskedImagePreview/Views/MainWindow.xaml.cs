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
            var image = vm.BaseImageViewModel.SelectedImage;
            if (image == null)
            {
                return;
            }

            // Shiftキーが押されている場合は「回転」
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                var angleFactor = e.Delta > 0 ? 2.0 : -2.0; // 2度ずつ回転

                // 360度でループさせる処理（-360〜360の範囲に収める場合）
                image.Angle = (image.Angle + angleFactor) % 360.0;
            }

            // 修飾キーが押されていない場合は「拡大縮小」
            else
            {
                var zoomFactor = e.Delta > 0 ? 0.05 : -0.05;
                image.Scale = Math.Min(5.0, Math.Max(0.1, image.Scale + zoomFactor));
            }

            e.Handled = true; // スクロール防止
        }
    }

    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is UIElement element)
        {
            isDragging = true;
            lastDragPosition = e.GetPosition(Border);
            element.CaptureMouse();
            e.Handled = true;
        }
    }

    private void Image_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging && sender is UIElement { IsMouseCaptured: true, })
        {
            var currentPosition = e.GetPosition(Border);
            var deltaX = currentPosition.X - lastDragPosition.X;
            var deltaY = currentPosition.Y - lastDragPosition.Y;
            if (DataContext is MainWindowViewModel vm)
            {
                var image = vm.BaseImageViewModel.SelectedImage;
                if (image != null)
                {
                    image.OffsetX += deltaX;
                    image.OffsetY += deltaY;
                }

            }

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