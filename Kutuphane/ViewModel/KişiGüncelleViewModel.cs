using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KişiGüncelleViewModel : InpcBase
    {
        public KişiGüncelleViewModel()
        {
            Kişi = new Kişi();

            KişiGüncelle = new RelayCommand<object>(parameter => MainViewModel.DatabaseSave.Execute(null), parameter => parameter is Kişi kişi);

            KişiResimGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kişi seçilikişi)
                {
                    OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                        File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                        seçilikişi.Resim = filename;
                    }
                }
            }, parameter => true);

            KişiKitapKontrol = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kişi seçiliKişi && !seçiliKişi.KitapAlabilir && seçiliKişi.İşlem.Any(z => !z.İşlemBitti))
                {
                    MessageBox.Show($"Dikkat {seçiliKişi.Ad} {seçiliKişi.Soyad} Adlı Kişinin Bitmemiş İşlemi Var.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public ICommand KişiKitapKontrol { get; }

        public ICommand KişiResimGüncelle { get; }

        public Kişi SeçiliKişi
        {
            get => seçiliKişi;

            set
            {
                if (seçiliKişi != value)
                {
                    seçiliKişi = value;
                    OnPropertyChanged(nameof(SeçiliKişi));
                }
            }
        }

        public ObservableCollection<Kişi> SeçiliKişiler
        {
            get => seçiliKişiler;

            set
            {
                if (seçiliKişiler != value)
                {
                    seçiliKişiler = value;
                    OnPropertyChanged(nameof(SeçiliKişiler));
                }
            }
        }

        public override string ToString()
        {
            return "KİŞİ GÜNCELLE";
        }

        private Kişi kişi;

        private Kişi seçiliKişi;

        private ObservableCollection<Kişi> seçiliKişiler = new();

        private void Kişi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiAdArama")
            {
                KişiGüncelleView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.Ad.Contains(Kişi.KişiAdArama) == true;
            }

            if (e.PropertyName is "KişiSoyadArama")
            {
                KişiGüncelleView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.Soyad.Contains(Kişi.KişiSoyadArama) == true;
            }
            if (e.PropertyName is "KişiTcArama")
            {
                KişiGüncelleView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.TC.Contains(Kişi.KişiTcArama) == true;
            }
        }
    }
}