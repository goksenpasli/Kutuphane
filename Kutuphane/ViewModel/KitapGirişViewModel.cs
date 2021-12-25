using Extensions;
using Kutuphane.Model;
using Kutuphane.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class KitapGirişViewModel : InpcBase
    {
        public KitapGirişViewModel()
        {
            Kitap = new Kitap();

            KitapEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    Kitap kitap = KitapOluştur();
                    if (Kitap.TopluKitapGirişi)
                    {
                        for (int i = 0; i < Kitap.TopluKitapSayısı; i++)
                        {
                            kitap = KitapOluştur();
                            if (Kitap.OtomatikBarkod)
                            {
                                kitap.Barkod = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue).ToString();
                            }
                            kütüphane?.Kitaplar.Add(kitap);
                        }
                    }
                    else
                    {
                        kütüphane?.Kitaplar.Add(kitap);
                    }

                    if (Kitap.KitapSayıOtomatikArttır)
                    {
                        kitap.DolapAltKod = Kitap.DolapAltKod++;
                    }

                    MainViewModel.DatabaseSave.Execute(null);
                    ResetKitap();

                    if (Kitap.OtomatikBarkod)
                    {
                        Kitap.Barkod = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue).ToString();
                    }
                }
            }, parameter => Kitap.DolapId != 0 && !string.IsNullOrWhiteSpace(Kitap?.Ad) && !string.IsNullOrWhiteSpace(Kitap?.Barkod));

            KitapTürEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    KitapTürü tür = new() { Açıklama = Kitap.Tür, Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue) };
                    kütüphane?.KitapTürleri.Add(tür);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kütüphane kütüphane && !kütüphane.KitapTürleri.Any(z => z.Açıklama == Kitap.Tür) && !string.IsNullOrWhiteSpace(Kitap?.Tür));

            KitapYazarEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    Yazar yazar = new() { Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue), Ad = Kitap.Yazar };
                    kütüphane?.Yazarlar.Add(yazar);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kütüphane kütüphane && !kütüphane.Yazarlar.Any(z => z.Ad == Kitap.Yazar) && !string.IsNullOrWhiteSpace(Kitap?.Yazar));

            KitapResimEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kitap kitap)
                {
                    OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                        if (Settings.Default.ResimKüçült)
                        {
                            BitmapSource image = new BitmapImage(new Uri(openFileDialog.FileName));
                            File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", image.Resize(Settings.Default.ResimKüçültmeOranı).ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Jpg));
                        }
                        else
                        {
                            File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                        }

                        kitap.Resim = filename;
                    }
                }
            }, parameter => true);

            KitapResimSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kitap kitap)
                {
                    kitap.Resim = null;
                }
            }, parameter => parameter is Kitap kitap && kitap.Resim is not null);

            TopluKitapEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Microsoft Excel Virgülle Ayrılmış Değerler Dosyası (*.csv)|*.csv" };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            IEnumerable<Kitap> kitaplistesi = openFileDialog.FileName.CsvKitapListesi(XlsFiyat, XlsYıl);
                            if (kitaplistesi != null)
                            {
                                foreach (Kitap xlskitap in kitaplistesi)
                                {
                                    Kitap kitap = new Kitap
                                    {
                                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                                        Ad = xlskitap.Ad,
                                        Barkod = xlskitap.Barkod,
                                        DolapId = Kitap.DolapId,
                                        Fiyat = xlskitap.Fiyat,
                                        BasımYılı = xlskitap.BasımYılı
                                    };

                                    kütüphane.Kitaplar.Add(kitap);
                                }
                                MainViewModel.DatabaseSave.Execute(null);
                            }
                            else
                            {
                                MessageBox.Show("Dosya Boş Görünüyor.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                        }
                        catch (Exception ex)
                        {
                            _ = MessageBox.Show(ex.Message, "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
            }, parameter => Kitap.DolapId != 0);

            Kitap.PropertyChanged += Kitap_PropertyChanged;
        }

        public Kitap Kitap
        {
            get => kitap;

            set
            {
                if (kitap != value)
                {
                    kitap = value;
                    OnPropertyChanged(nameof(Kitap));
                }
            }
        }

        public ICommand KitapEkle { get; }

        public ICommand KitapResimEkle { get; }

        public ICommand KitapResimSil { get; }

        public ICommand KitapTürEkle { get; }

        public ICommand KitapYazarEkle { get; }

        public ICommand TopluKitapEkle { get; }

        public bool XlsFiyat
        {
            get => xlsFiyat;

            set
            {
                if (xlsFiyat != value)
                {
                    xlsFiyat = value;
                    OnPropertyChanged(nameof(XlsFiyat));
                }
            }
        }

        public bool XlsYıl
        {
            get => xlsYıl; set

            {
                if (xlsYıl != value)
                {
                    xlsYıl = value;
                    OnPropertyChanged(nameof(XlsYıl));
                }
            }
        }

        private Kitap kitap;

        private bool xlsFiyat;

        private bool xlsYıl;

        private void Kitap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "TopluKitapGirişi" && !Kitap.TopluKitapGirişi)
            {
                Kitap.TopluKitapSayısı = 1;
            }
            if (e.PropertyName is "OtomatikBarkod" && Kitap.OtomatikBarkod)
            {
                Kitap.Barkod = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue).ToString();
            }
        }

        private Kitap KitapOluştur()
        {
            Kitap kitap = new()
            {
                Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                Ad = Kitap.Ad,
                Açıklama = Kitap.Açıklama,
                Barkod = Kitap.Barkod,
                BasımYılı = Kitap.BasımYılı,
                Demirbaş = Kitap.Demirbaş,
                DolapAltKod = Kitap.DolapAltKod,
                DolapId = Kitap.DolapId,
                Favori = Kitap.Favori,
                Fiyat = Math.Round(Kitap.Fiyat, 2),
                KitapDurumId = Kitap.KitapDurumId,
                SistemKayıtTarihi = DateTime.Now,
                Resim = Kitap.Resim,
                ÖdünçVerilebilir = Kitap.ÖdünçVerilebilir,
                KitapDili = Kitap.KitapDili,
                Renk = Kitap.Renk
            };
            foreach (Yazar yazar in Kitap.SeçiliYazarlar)
            {
                kitap.Yazarlar.Add(yazar);
            }
            foreach (KitapTürü kitaptürü in Kitap.SeçiliKitapTürleri)
            {
                kitap.Türler.Add(kitaptürü);
            }
            return kitap;
        }

        private void ResetKitap()
        {
            Kitap.Ad = null;
            Kitap.Resim = null;
            Kitap.Barkod = null;
            Kitap.Açıklama = null;
            Kitap.Renk = "Transparent";
            Kitap.TopluKitapSayısı = 1;
            Kitap.TopluKitapGirişi = false;
        }
    }
}