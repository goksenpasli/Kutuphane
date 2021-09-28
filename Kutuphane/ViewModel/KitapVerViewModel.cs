using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapVerViewModel : InpcBase
    {
        private Kişi kişi;
        private İşlem işlem;

        public KitapVerViewModel()
        {
            Kişi = new Kişi();
            İşlem = new İşlem();

            KitapVer = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap && MessageBox.Show($"{kitap.Ad} Adlı Kitap {kişi.Ad} {kişi.Soyad} Adlı Kişiye {İşlem.KitapGün} Günlüğüne Verilecek Onaylıyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var işlem = new İşlem
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        KitapGün = İşlem.KitapGün,
                        GeriGetirmeTarihi = İşlem.GeriGetirmeTarihi,
                        KitapId = kitap.Id,
                        BaşlangıçTarihi = İşlem.BaşlangıçTarihi,
                    };

                    kişi.İşlem.Add(işlem);
                    kitap.KitapDurumId = 1;
                    MainViewModel.DatabaseSave.Execute(null);

                    İşlem.KitapGün = 1;
                    İşlem.BaşlangıçTarihi = DateTime.Today;
                }
            }, parameter => parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap && kişi.KitapAlabilir && kitap.ÖdünçVerilebilir);

            HızlıKitapVer = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap)
                {
                    var işlem = new İşlem
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        KitapGün = Properties.Settings.Default.HızlıKitapGirişGünSüresi,
                        GeriGetirmeTarihi = DateTime.Today.AddDays(Properties.Settings.Default.HızlıKitapGirişGünSüresi),
                        KitapId = kitap.Id,
                        BaşlangıçTarihi = DateTime.Today,
                    };

                    kişi.İşlem.Add(işlem);
                    kitap.KitapDurumId = 1;
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap && kişi.KitapAlabilir && kitap.ÖdünçVerilebilir);

            İşlem.GeriGetirmeTarihi = İşlem.BaşlangıçTarihi.AddDays(İşlem.KitapGün);
            Kişi.PropertyChanged += Kişi_PropertyChanged;
            İşlem.PropertyChanged += İşlem_PropertyChanged;
        }

        public ICommand HızlıKitapVer { get; }

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

        public ICommand KitapVer { get; }

        private void İşlem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "BaşlangıçTarihi" or "KitapGün")
            {
                İşlem.GeriGetirmeTarihi = İşlem.BaşlangıçTarihi.AddDays(İşlem.KitapGün);
            }
        }

        private void Kişi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KitapVerView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap).Ad.Contains(Kişi.KişiKitapAdArama);
            }

            if (e.PropertyName is "KişiKitapBarkodArama")
            {
                KitapVerView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap).Barkod.Contains(Kişi.KişiKitapBarkodArama);
            }

            if (e.PropertyName is "KişiAdArama")
            {
                KitapVerView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Ad.Contains(Kişi.KişiAdArama);
            }

            if (e.PropertyName is "KişiSoyadArama")
            {
                KitapVerView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Soyad.Contains(Kişi.KişiSoyadArama);
            }

            if (e.PropertyName is "KişiTcArama")
            {
                KitapVerView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).TC.Contains(Kişi.KişiTcArama);
            }
        }
    }
}