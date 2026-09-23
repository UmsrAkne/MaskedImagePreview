using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaskedImagePreview.Converters
{
    public class RectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is [double width and > 0, double height and > 0, ..])
            {
                return new Rect(0, 0, width, height);
            }

            return Rect.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}