using Kutuphane.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Kutuphane
{
    public sealed class TakvimGünİşlemCountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()) && values[0] is DateTime gün && values[1] is ObservableCollection<Kişi> kişiler)
            {
                return kişiler.SelectMany(z => z.İşlem).Count(z => !z.İşlemBitti && z.GeriGetirmeTarihi == gün).ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}