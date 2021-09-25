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
    public class DataImageFilePathImageUriConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty DecodeHeightProperty = DependencyProperty.RegisterAttached("DecodeHeight", typeof(int), typeof(DataImageFilePathImageUriConverter), new PropertyMetadata(96));

        public static int GetDecodeHeight(DependencyObject obj) => (int)obj.GetValue(DecodeHeightProperty);

        public static void SetDecodeHeight(DependencyObject obj, int value) => obj.SetValue(DecodeHeightProperty, value);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()) && value is string filename && !string.IsNullOrEmpty(filename) && File.Exists($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}"))
            {
                BitmapImage image = new();
                image.BeginInit();
                image.DecodePixelHeight = GetDecodeHeight(this);
                image.CacheOption = BitmapCacheOption.None;//onload to bypass file lock
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                image.EndInit();
                if (!image.IsFrozen && image.CanFreeze)
                {
                    image.Freeze();
                }
                return image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}