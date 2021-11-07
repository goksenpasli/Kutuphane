using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Kutuphane.ViewModel
{
    public class ViewerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Image { get; set; }

        public DataTemplate Pdf { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item is string dosya
                ? string.Equals(new FileInfo(dosya).Extension, ".pdf", StringComparison.OrdinalIgnoreCase) ? Pdf : Image
                : null;
        }
    }
}