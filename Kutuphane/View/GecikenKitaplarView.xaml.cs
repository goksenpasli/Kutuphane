using Kutuphane.Model;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for GecikenKitaplarView.xaml
    /// </summary>
    public partial class GecikenKitaplarView : UserControl
    {
        public static CollectionViewSource cvskişi;

        public GecikenKitaplarView()
        {
            InitializeComponent();
            cvskişi = TryFindResource("Kişiler") as CollectionViewSource;
            cvskişi.Filter += (s, e) => e.Accepted = (e.Item as Kişi).İşlem.Any();
        }
    }
}