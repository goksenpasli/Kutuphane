using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kutuphane.ViewModel
{
    public class ViewerTemplateSelector : DataTemplateSelector
    {
        private readonly string[] imageext = new string[] { ".jpg", ".jpeg", ".tif", ".tiff", ".png" };

        private readonly string[] videoext = new string[] { ".mp4", ".wmv", ".avi", ".mpg", ".3gp" };

        public DataTemplate Empty { get; set; }

        public DataTemplate Image { get; set; }

        public DataTemplate Pdf { get; set; }

        public DataTemplate Video { get; set; }

        public DataTemplate Xps { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string dosya)
            {
                string ext = new FileInfo(dosya).Extension;
                if (string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return Pdf;
                }
                if (string.Equals(ext, ".xps", StringComparison.OrdinalIgnoreCase))
                {
                    return Xps;
                }
                if (imageext.Contains(ext.ToLower()))
                {
                    return Image;
                }
                return videoext.Contains(ext.ToLower()) ? Video : Empty;
            }
            return Empty;
        }
    }
}