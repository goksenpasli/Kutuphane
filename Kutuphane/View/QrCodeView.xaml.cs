using Kutuphane.ViewModel;
using System.Windows;
using System.Windows.Controls;
using Kutuphane.Properties;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for QrCodeView.xaml
    /// </summary>
    public partial class QrCodeView : UserControl
    {
        public static readonly DependencyProperty BarkodProperty = DependencyProperty.Register("Barkod", typeof(string), typeof(QrCodeView), new PropertyMetadata(null, Changed));

        public QrCodeView()
        {
            InitializeComponent();
        }

        public string Barkod
        {
            get => (string)GetValue(BarkodProperty);
            set => SetValue(BarkodProperty, value);
        }

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QrCodeView qrCodeView)
            {
                QrCodeViewModel dc = qrCodeView.DataContext as QrCodeViewModel;
                dc.Barkod.Metin = e.NewValue as string;
                dc.Barkod.BarkodImage = dc.Barkod.GenerateBarCodeImage(Settings.Default.SeçiliBarkod);
            }
        }
    }
}