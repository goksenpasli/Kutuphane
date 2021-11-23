using Kutuphane.Model;
using Kutuphane.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;
using ZXing;

namespace Kutuphane
{
    public class KitapBarkodToBarCodeImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Kitap kitap && values[1] is BarcodeFormat barcodeFormat && values[2] is bool pure)
            {
                Barkod Barkod = new()
                {
                    Metin = kitap.Barkod,
                };
                return Barkod.GenerateBarCodeImage(barcodeFormat, pure);
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}