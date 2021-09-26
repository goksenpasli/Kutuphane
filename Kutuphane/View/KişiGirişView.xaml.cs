using Kutuphane.Model;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KişiGirişView.xaml
    /// </summary>
    public partial class KişiGirişView : UserControl
    {
        public static CollectionViewSource cvs;

        public static CollectionViewSource cvskişi;

        public KişiGirişView()
        {
            InitializeComponent();
            cvs = TryFindResource("Kitaplar") as CollectionViewSource;
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
            cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap)?.KitapDurumId == 0;
        }
    }
}