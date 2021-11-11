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

        public DataTemplate Empty { get; set; }

        public DataTemplate Image { get; set; }

        public DataTemplate Pdf { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string dosya)
            {
                string ext = new FileInfo(dosya).Extension;
                if (string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return Pdf;
                }
                else
                {
                    return imageext.Contains(ext.ToLower()) ? Image : Empty;
                }
            }
            return Empty;
        }
    }
}