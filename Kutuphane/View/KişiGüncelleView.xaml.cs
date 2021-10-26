using Kutuphane.ViewModel;
using System;
using System.ComponentModel;
using System.IO;
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

        private void CameraUserControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (DataContext is KişiGüncelleViewModel kişiGüncelleViewModel && e.PropertyName == "ResimData")
            {
                var filename = Guid.NewGuid() + ".jpg";
                File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", (sender as CameraUserControl)?.ResimData);
                kişiGüncelleViewModel.SeçiliKişi.Resim = filename;
            }
        }
    }
}