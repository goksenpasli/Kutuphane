using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapGeriAlViewModel : InpcBase
    {
        private Kişi kişi;

        public KitapGeriAlViewModel()
        {
            Kişi = new Kişi();

            KitapGeriAl = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Seçili Kitabı Geri Almak İstiyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes && parameter is İşlem işlem)
                {
                    TimeSpan günfarkı = DateTime.Today - işlem.GeriGetirmeTarihi;
                    işlem.CezaTutar = günfarkı.TotalDays > 0 ? (günfarkı.TotalDays * Properties.Settings.Default.GünlükGecikmeBedeli) : 0;

                    if (işlem.CezaTutar > 0)
                    {
                        if (MessageBox.Show($"Seçili Kitapta {işlem.CezaTutar:C} Ceza Var Tutarı Tahsil Edin Kitap Geri Alınacak. Devam Etmek İstiyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                            işlem.CezaÖdemeTarihi = DateTime.Now;
                            işlem.Ceza = true;
                            işlem.İşlemBitti = true;
                            işlem.SeçiliKitap.KitapDurumId = (int)KitapDurumu.Kütüphanede;
                            MainViewModel.DatabaseSave.Execute(null);
                            Kişi.KitapCezasıAdeti++;
                        }
                    }
                    else
                    {
                        işlem.Ceza = false;
                        işlem.İşlemBitti = true;
                        işlem.GeriGetirmeTarihi = DateTime.Now;
                        işlem.SeçiliKitap.KitapDurumId = (int)KitapDurumu.Kütüphanede;
                        MainViewModel.DatabaseSave.Execute(null);
                        işlem = null;
                    }
                }
            }, parameter => parameter is İşlem işlem && işlem.SeçiliKitap?.KitapDurumId == (int)KitapDurumu.Okuyucuda);

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

        public ICommand KitapGeriAl { get; }

        private void Kişi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiAdArama")
            {
                KitapGeriAlView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.Ad.Contains(Kişi.KişiAdArama) == true;
            }

            if (e.PropertyName is "KişiSoyadArama")
            {
                KitapGeriAlView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.Soyad.Contains(Kişi.KişiSoyadArama) == true;
            }

            if (e.PropertyName is "KişiTcArama")
            {
                KitapGeriAlView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.TC.Contains(Kişi.KişiTcArama) == true;
            }
        }
    }
}