using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KişiGüncelleViewModel : InpcBase
    {
        private Kişi kişi;

        public KişiGüncelleViewModel()
        {
            Kişi = new Kişi();

            KişiGüncelle = new RelayCommand<object>(parameter => MainViewModel.DatabaseSave.Execute(null), parameter => parameter is Kişi kişi);

            KişiResimGüncelle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                    (parameter as Kişi).Resim = filename;
                }
            }, parameter => true);
            Kişi.PropertyChanged += Kişi_PropertyChanged;
        }

        public Kişi Kişi
        {
            get => kişi;

            set
            {
                if (kişi != value)
                {
                    kişi = value;
                    OnPropertyChanged(nameof(Kişi));
                }
            }
        }

        public ICommand KişiGüncelle { get; }

        public ICommand KişiResimGüncelle { get; }

        private void Kişi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiAdArama")
            {
                KişiGüncelleView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Ad.Contains(Kişi.KişiAdArama);
            }

            if (e.PropertyName is "KişiSoyadArama")
            {
                KişiGüncelleView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Soyad.Contains(Kişi.KişiSoyadArama);
            }
            if (e.PropertyName is "KişiTcArama")
            {
                KişiGüncelleView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).TC.Contains(Kişi.KişiTcArama);
            }
        }
    }
}