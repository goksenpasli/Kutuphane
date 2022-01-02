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
            CsvData = new CsvData();
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
                                kitap.Barkod = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue);
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
                        Kitap.Barkod = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue);
                    }
                }
            }, parameter => Kitap.DolapId != 0 && !string.IsNullOrWhiteSpace(Kitap?.Ad) && Kitap?.Barkod != 0);

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
                            BitmapSource image;
                            switch (Settings.Default.Biçim)
                            {
                                case (int)YenidenBoyutlandırma.Oran:
                                    {
                                        image = new BitmapImage(new Uri(openFileDialog.FileName));
                                        File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", image.Resize(Settings.Default.ResimKüçültmeOranı).ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Jpg));
                                        break;
                                    }
                                case (int)YenidenBoyutlandırma.Boyut:
                                    {
                                        image = new BitmapImage(new Uri(openFileDialog.FileName));
                                        File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", image.Resize(Settings.Default.En, Settings.Default.Boy).ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Jpg));
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                        }

                        kitap.Resim = filename;
                    }
                }
            }, parameter => true);

            KitapVideoEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kitap kitap)
                {
                    OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Video Dosyaları (*.mp4;*.mpeg;*.mpg;*.wmv;*.avi)|*.mp4;*.mpeg;*.mpg;*.wmv;*.avi" };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                        File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                        kitap.Video = filename;
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

            KitapVideoSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kitap kitap)
                {
                    kitap.Video = null;
                }
            }, parameter => parameter is Kitap kitap && kitap.Video is not null);

            TopluKitapEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Microsoft Excel Virgülle Ayrılmış Değerler Dosyası (*.csv)|*.csv" };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            IEnumerable<Kitap> kitaplistesi = openFileDialog.FileName.CsvKitapListesi(CsvData.Fiyat, CsvData.Yıl, CsvData.Dil);
                            if (kitaplistesi != null)
                            {
                                foreach (Kitap xlskitap in kitaplistesi)
                                {
                                    Kitap kitap = new()
                                    {
                                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                                        Ad = xlskitap.Ad,
                                        Barkod = xlskitap.Barkod,
                                        DolapId = Kitap.DolapId,
                                        Fiyat = xlskitap.Fiyat,
                                        BasımYılı = xlskitap.BasımYılı,
                                        KitapDili = xlskitap.KitapDili
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

        public CsvData CsvData
        {
            get => csvData;

            set
            {
                if (csvData != value)
                {
                    csvData = value;
                    OnPropertyChanged(nameof(CsvData));
                }
            }
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

        public ICommand KitapVideoEkle { get; }

        public ICommand KitapVideoSil { get; }

        public ICommand KitapYazarEkle { get; }

        public ICommand TopluKitapEkle { get; }

        private CsvData csvData;

        private Kitap kitap;

        private void Kitap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "TopluKitapGirişi" && !Kitap.TopluKitapGirişi)
            {
                Kitap.TopluKitapSayısı = 1;
            }
            if (e.PropertyName is "OtomatikBarkod" && Kitap.OtomatikBarkod)
            {
                Kitap.Barkod = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue);
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
                Renk = Kitap.Renk,
                Video = Kitap.Video,
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
            Kitap.Barkod = 0;
            Kitap.Açıklama = null;
            Kitap.Renk = "Transparent";
            Kitap.TopluKitapSayısı = 1;
            Kitap.TopluKitapGirişi = false;
        }
    }
}