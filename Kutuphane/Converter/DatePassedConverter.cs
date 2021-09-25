using System;
using System.Globalization;
using System.Windows.Data;

namespace Kutuphane
{
    public class DatePassedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is DateTime dateTime && dateTime < DateTime.Today;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}