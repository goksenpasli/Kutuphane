using Kutuphane.Model;
using Kutuphane.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KitapVerView.xaml
    /// </summary>
    public partial class KitapVerView : UserControl
    {
        public static CollectionViewSource cvs;

        public static CollectionViewSource cvskişi;

        public KitapVerView()
        {
            InitializeComponent();
            cvs = TryFindResource("Kitaplar") as CollectionViewSource;
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
            cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap)?.KitapDurumId == 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Aktarma)
            {
                var kitapVerViewModel = DataContext as KitapVerViewModel;
                kitapVerViewModel.Kişi.KişiTcArama = kitapVerViewModel.Kişi.AktarmaTC;
            }
        }
    }
}