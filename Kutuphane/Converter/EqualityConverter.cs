using System;
using System.Globalization;
using System.Windows.Data;

namespace Kutuphane
{
    public class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => (parameter is not null) ? values.Length >= 2 && !values[0].Equals(values[1]) : values.Length >= 2 && values[0].Equals(values[1]);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}