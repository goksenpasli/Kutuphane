using System.Windows;
using System.Windows.Data;

namespace Kutuphane
{
    public sealed class DoubleToThicknessConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness((double)value);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}