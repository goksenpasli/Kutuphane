using System;
using System.Globalization;
using System.Windows.Data;

namespace Kutuphane
{
    public class ListViewCezaTutarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime gerigetirmetarihi && gerigetirmetarihi != DateTime.MinValue)
            {
                TimeSpan günfarkı = DateTime.Today - gerigetirmetarihi;
                return günfarkı.TotalDays > 0 ? (günfarkı.TotalDays * Properties.Settings.Default.GünlükGecikmeBedeli).ToString("C") : "CEZA YOK";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}