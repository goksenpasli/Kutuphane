using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using Microsoft.Win32;
using System;
using System.IO;
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
                    if (!Kişi.TC.TcGeçerli())
                    {
                        _ = MessageBox.Show("Bu TC Yanlıştır.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
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
                    Kişi kişi = new()
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        Ad = Kişi.Ad,
                        TC = Kişi.TC,
                        Soyad = Kişi.Soyad,
                        Telefon = Kişi.Telefon,
                        Cinsiyet = Kişi.Cinsiyet,
                        DoğumTarihi = Kişi.DoğumTarihi,
                        Adres = Kişi.Adres,
                        KitapAlabilir = Kişi.KitapAlabilir,
                        KayıtTarihi = DateTime.Now,
                        Resim = Kişi.Resim
                    };
                    mainViewModel.Kütüphane.Kişiler.Add(kişi);
                    MainViewModel.DatabaseSave.Execute(null);
                    Kişi.SonKaydedilenKişi = kişi;

                    ResetKişi();
                }
            }, parameter => !string.IsNullOrWhiteSpace(Kişi.TC) && !string.IsNullOrWhiteSpace(Kişi.Ad) && !string.IsNullOrWhiteSpace(Kişi.Soyad));

            KişiResimYükle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                    Kişi.Resim = filename;
                }
            }, parameter => true);

            Kişi.PropertyChanged += KişiGirişViewModel_PropertyChanged;
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

        public ICommand KişiEkle { get; }

        public ICommand KişiGüncelle { get; }

        public ICommand KişiResimYükle { get; }

        public override string ToString()
        {
            return "KİŞİ GİRİŞ";
        }

        private Kişi kişi;

        private void KişiGirişViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KişiGirişView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Ad.Contains(Kişi.KişiKitapAdArama) == true;
            }

            if (e.PropertyName is "KişiKitapBarkodArama")
            {
                KişiGirişView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Barkod.Contains(Kişi.KişiKitapBarkodArama) == true;
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
            Kişi.Resim = null;
        }
    }
}