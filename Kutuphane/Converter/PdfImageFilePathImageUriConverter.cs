using Freeware;
using Kutuphane.ViewModel;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Kutuphane
{
    public class PdfImageFilePathImageUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()) && value is string filename && !string.IsNullOrEmpty(filename) && File.Exists($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}"))
            {
                using var pdf = new FileStream($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", FileMode.Open);
                return PdfViewer.BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, 1, 150));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value as BitmapSource;
    }
}