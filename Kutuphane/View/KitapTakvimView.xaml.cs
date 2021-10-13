using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KitapTakvimView.xaml
    /// </summary>
    public partial class KitapTakvimView : UserControl
    {
        public static CollectionViewSource cvskişi;

        public KitapTakvimView()
        {
            InitializeComponent();
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
        }
    }
}