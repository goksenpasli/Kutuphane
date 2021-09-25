using Kutuphane.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for QrCodeMultipleView.xaml
    /// </summary>
    public partial class QrCodeMultipleView : UserControl
    {
        public static readonly DependencyProperty TopluBarkodMetinProperty = DependencyProperty.Register("TopluBarkodMetin", typeof(string), typeof(QrCodeMultipleView), new PropertyMetadata(null, Changed));

        public QrCodeMultipleView()
        {
            InitializeComponent();
        }

        public string TopluBarkodMetin
        {
            get => (string)GetValue(TopluBarkodMetinProperty);
            set => SetValue(TopluBarkodMetinProperty, value);
        }

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QrCodeMultipleView qrCodeMultipleView)
            {
                var dc = qrCodeMultipleView.DataContext as QrCodeMultipleViewModel;
                dc.Barkod.Metin = qrCodeMultipleView.TopluBarkodMetin;
                dc.Barkod.BarkodImage = dc.GenerateBarCodeImage(dc.Barkod.BarcodeFormat);
                for (var i = 0; i < dc.En * dc.Boy; i++)
                {
                    dc.BarkodResimler.Add(dc.Barkod.BarkodImage);
                }
            }
        }
    }
}