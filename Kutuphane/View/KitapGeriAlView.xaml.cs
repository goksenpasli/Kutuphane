using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KitapGeriAlView.xaml
    /// </summary>
    public partial class KitapGeriAlView : UserControl
    {
        public static CollectionViewSource cvskişi;

        public KitapGeriAlView()
        {
            InitializeComponent();
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
        }
    }
}