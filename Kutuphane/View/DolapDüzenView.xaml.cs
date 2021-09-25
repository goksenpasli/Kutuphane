using System.Windows.Controls;
using System.Windows.Data;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for DolapDüzenView.xaml
    /// </summary>
    public partial class DolapDüzenView : UserControl
    {
        public static CollectionViewSource cvs;

        public static CollectionViewSource cvsdolaplar;

        public DolapDüzenView()
        {
            InitializeComponent();
            cvs = TryFindResource("Kitaplar") as CollectionViewSource;
            cvsdolaplar = TryFindResource("Dolaplar") as CollectionViewSource;
        }
    }
}