using Kutuphane.Properties;
using Kutuphane.ViewModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for QrCodeMultipleView.xaml
    /// </summary>
    public partial class QrCodeMultipleView : UserControl
    {
        public static readonly DependencyProperty SeçiliKitaplarBarkodProperty = DependencyProperty.Register("SeçiliKitaplarBarkod", typeof(IList), typeof(QrCodeMultipleView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TopluBarkodMetinProperty = DependencyProperty.Register("TopluBarkodMetin", typeof(string), typeof(QrCodeMultipleView), new PropertyMetadata(null, Changed));

        public QrCodeMultipleView()
        {
            InitializeComponent();
        }

        public IList SeçiliKitaplarBarkod
        {
            get => (IList)GetValue(SeçiliKitaplarBarkodProperty);
            set => SetValue(SeçiliKitaplarBarkodProperty, value);
        }

        public string TopluBarkodMetin
        {
            get => (string)GetValue(TopluBarkodMetinProperty);
            set => SetValue(TopluBarkodMetinProperty, value);
        }

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QrCodeMultipleView qrCodeMultipleView && qrCodeMultipleView.DataContext is QrCodeMultipleViewModel dc)
            {
                dc.BarkodResimler.Clear();
                dc.Barkod.Metin = qrCodeMultipleView.TopluBarkodMetin;
                dc.Barkod.BarkodImage = dc.Barkod.GenerateBarCodeImage(Settings.Default.SeçiliBarkod);
                dc.BarkodResimler.Clear();
                for (int i = 0; i < dc.En * dc.Boy; i++)
                {
                    dc.BarkodResimler.Add(dc.Barkod.BarkodImage);
                }
            }
        }
    }
}