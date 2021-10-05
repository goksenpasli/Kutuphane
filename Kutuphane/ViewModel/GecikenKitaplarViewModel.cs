using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class GecikenKitaplarViewModel : InpcBase
    {
        private İşlem işlem;

        private Kişi kişi;

        private bool tarihEtkin = true;

        private DateTime uzatılmaTarihi = DateTime.Today;

        public GecikenKitaplarViewModel()
        {
            Kişi = new Kişi();
            İşlem = new İşlem();

            UzatmaGir = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Seçili Kitaba Süre Uzatması Girmek İstiyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    İşlem.Uzatıldı = true;
                    İşlem.UzatmaSayısı++;
                    İşlem.GeriGetirmeTarihi = UzatılmaTarihi;
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => UzatılmaTarihi > İşlem?.GeriGetirmeTarihi && İşlem?.UzatmaSayısı < Properties.Settings.Default.MaksimumUzatmaSayısı);

            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
            PropertyChanged += GecikenKitaplarViewModel_PropertyChanged;
            Kişi.PropertyChanged += Kişi_PropertyChanged;
        }

        public İşlem İşlem
        {
            get => işlem;

            set
            {
                if (işlem != value)
                {
                    işlem = value;
                    OnPropertyChanged(nameof(İşlem));
                }
            }
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

        public bool TarihEtkin
        {
            get => tarihEtkin;

            set
            {
                if (tarihEtkin != value)
                {
                    tarihEtkin = value;
                    OnPropertyChanged(nameof(TarihEtkin));
                }
            }
        }

        public DateTime UzatılmaTarihi
        {
            get => uzatılmaTarihi;

            set
            {
                if (uzatılmaTarihi != value)
                {
                    uzatılmaTarihi = value;
                    OnPropertyChanged(nameof(UzatılmaTarihi));
                }
            }
        }

        public ICommand UzatmaGir { get; }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "MaksimumUzatmaSayısı")
            {
                TarihEtkin = İşlem?.UzatmaSayısı < Properties.Settings.Default.MaksimumUzatmaSayısı;
            }
        }

        private void GecikenKitaplarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) => TarihEtkin = İşlem?.UzatmaSayısı < Properties.Settings.Default.MaksimumUzatmaSayısı;

        private void Kişi_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiAdArama")
            {
                GecikenKitaplarView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Ad.Contains(Kişi.KişiAdArama);
            }
            if (e.PropertyName is "KişiSoyadArama")
            {
                GecikenKitaplarView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Soyad.Contains(Kişi.KişiSoyadArama);
            }
            if (e.PropertyName is "KişiTcArama")
            {
                GecikenKitaplarView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).TC.Contains(Kişi.KişiTcArama);
            }
        }
    }
}