using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KişiGirişViewModel : InpcBase
    {
        public KişiGirişViewModel()
        {
            Kişi = new Kişi();

            KişiEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is MainViewModel mainViewModel)
                {
                    if (mainViewModel.Kütüphane.Kişiler.Any(z => z.TC == Kişi.TC))
                    {
                        if (Properties.Settings.Default.Aktarma)
                        {
                            _ = MessageBox.Show("Bu TC İle Giriş Yapılmıştır. Kitap Ver Ekranına Yönlendirileceksiniz.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            mainViewModel.KitapVerViewModel.Kişi.AktarmaTC = Kişi.TC;
                            mainViewModel.KitapVerEkranı.Execute(null);
                            return;
                        }
                        _ = MessageBox.Show("Bu TC İle Giriş Yapılmıştır.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    var kişi = new Kişi
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        Ad = Kişi.Ad,
                        TC = Kişi.TC,
                        Soyad = Kişi.Soyad,
                        Telefon = Kişi.Telefon,
                        Cinsiyet = Kişi.Cinsiyet,
                        DoğumTarihi = Kişi.DoğumTarihi,
                        Adres = Kişi.Adres,
                    };
                    mainViewModel.Kütüphane.Kişiler.Add(kişi);
                    MainViewModel.DatabaseSave.Execute(null);
                    Kişi.SonKaydedilenKişi = kişi;

                    ResetKişi();
                }
            }, parameter => !string.IsNullOrWhiteSpace(Kişi.TC) && !string.IsNullOrWhiteSpace(Kişi.Ad) && !string.IsNullOrWhiteSpace(Kişi.Soyad));

            Kişi.PropertyChanged += KişiGirişViewModel_PropertyChanged;
        }

        public Kişi Kişi { get; set; }

        public ICommand KişiEkle { get; }

        private void KişiGirişViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KişiGirişView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap).Ad.Contains(Kişi.KişiKitapAdArama);
            }

            if (e.PropertyName is "KişiKitapBarkodArama")
            {
                KişiGirişView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap).Barkod.Contains(Kişi.KişiKitapBarkodArama);
            }
        }

        private void ResetKişi()
        {
            Kişi.TC = null;
            Kişi.Ad = null;
            Kişi.Soyad = null;
            Kişi.Telefon = null;
            Kişi.Adres = null;
            Kişi.Cinsiyet = -1;
            Kişi.AktarmaTC = null;
        }
    }
}