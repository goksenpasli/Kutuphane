using Kutuphane.Model;
using Kutuphane.ViewModel;
using System;
using System.IO;
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
            cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap)?.KitapDurumId == (int)KitapDurumu.Kütüphanede;
        }

        private void CameraUserControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (DataContext is KişiGirişViewModel kişiGirişViewModel && e.PropertyName == "ResimData")
            {
                var filename = Guid.NewGuid() + ".jpg";
                File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", (sender as CameraUserControl)?.ResimData);
                kişiGirişViewModel.Kişi.Resim = filename;
            }
        }
    }
}