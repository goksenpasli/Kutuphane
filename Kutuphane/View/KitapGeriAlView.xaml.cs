using Extensions;
using Kutuphane.Model;
using Kutuphane.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

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