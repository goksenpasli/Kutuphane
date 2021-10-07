using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KişiGüncelleView.xaml
    /// </summary>
    public partial class KişiGüncelleView : UserControl
    {
        public static CollectionViewSource cvskişi;

        public KişiGüncelleView()
        {
            InitializeComponent();
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
        }
    }
}