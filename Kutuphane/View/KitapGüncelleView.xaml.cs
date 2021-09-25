using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KitapGüncelleView.xaml
    /// </summary>
    public partial class KitapGüncelleView : UserControl
    {
        public static CollectionViewSource cvs;

        public KitapGüncelleView()
        {
            InitializeComponent();
            cvs = TryFindResource("Kitaplar") as CollectionViewSource;
        }
    }
}