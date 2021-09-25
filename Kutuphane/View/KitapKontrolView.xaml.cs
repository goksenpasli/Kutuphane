using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KitapKontrolView.xaml
    /// </summary>
    public partial class KitapKontrolView : UserControl
    {
        public static CollectionViewSource cvs;

        public static CollectionViewSource cvskişi;

        public KitapKontrolView()
        {
            InitializeComponent();
            cvs = TryFindResource("Kitaplar") as CollectionViewSource;
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
        }
    }
}