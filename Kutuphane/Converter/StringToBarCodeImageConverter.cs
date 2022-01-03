using Kutuphane.Model;
using Kutuphane.ViewModel;
using System.Windows.Data;

namespace Kutuphane
{
    public sealed class StringToBarCodeImageConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string metin)
            {
                Barkod barkod = new()
                {
                    Metin = metin,
                    QrWidth = 350,
                };
                return barkod.GenerateBarCodeImage(ZXing.BarcodeFormat.CODE_128, true);
            }
            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}