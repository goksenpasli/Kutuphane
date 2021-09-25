using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Extensions
{
    public class TimespanToSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;
            return timeSpan.TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => TimeSpan.FromSeconds((double)value);
    }
}