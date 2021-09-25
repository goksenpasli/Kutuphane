using Extensions;
using Kutuphane.Model;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapGirişViewModel : InpcBase
    {
        private ObservableCollection<KitapTürü> seçiliKitapTürleri = new();

        private ObservableCollection<Yazar> seçiliYazarlar = new();

        public KitapGirişViewModel()
        {
            Kitap = new Kitap();

            KitapEkle = new RelayCommand<object>(parameter =>
            {
                Kitap kitap;
                if (parameter is Kütüphane kütüphane)
                {
                    kitap = KitapOluştur();

                    if (Kitap.TopluKitapGirişi)
                    {
                        for (var i = 0; i < Kitap.TopluKitapSayısı; i++)
                        {
                            kitap = KitapOluştur();
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

                    Kitap.Ad = null;
                    Kitap.Resim = null;
                    Kitap.Barkod = null;
                    Kitap.Açıklama = null;
                    Kitap.TopluKitapSayısı = 1;
                }
            }, parameter => Kitap.DolapId != 0 && !string.IsNullOrWhiteSpace(Kitap?.Ad));

            KitapTürü tür = null;
            KitapTürEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    tür = new KitapTürü() { Açıklama = Kitap.Tür, Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue) };
                    kütüphane?.KitapTürleri.Add(tür);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kütüphane kütüphane && !kütüphane.KitapTürleri.Any(z => z.Açıklama == Kitap.Açıklama) && !string.IsNullOrWhiteSpace(Kitap?.Tür));

            Yazar yazar = null;
            KitapYazarEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    yazar = new Yazar() { Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue), Ad = Kitap.Yazar };
                    kütüphane?.Yazarlar.Add(yazar);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kütüphane kütüphane && !kütüphane.Yazarlar.Any(z => z.Ad == Kitap.Yazar) && !string.IsNullOrWhiteSpace(Kitap?.Yazar));

            KitapResimEkle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                    (parameter as Kitap).Resim = filename;
                }
            }, parameter => true);

            Kitap.PropertyChanged += Kitap_PropertyChanged;
        }

        public Kitap Kitap { get; set; }

        public ICommand KitapEkle { get; }

        public ICommand KitapResimEkle { get; }

        public ICommand KitapTürEkle { get; }

        public ICommand KitapYazarEkle { get; }

        public ObservableCollection<KitapTürü> SeçiliKitapTürleri
        {
            get => seçiliKitapTürleri;

            set
            {
                if (seçiliKitapTürleri != value)
                {
                    seçiliKitapTürleri = value;
                    OnPropertyChanged(nameof(SeçiliKitapTürleri));
                }
            }
        }

        public ObservableCollection<Yazar> SeçiliYazarlar
        {
            get => seçiliYazarlar;

            set
            {
                if (seçiliYazarlar != value)
                {
                    seçiliYazarlar = value;
                    OnPropertyChanged(nameof(SeçiliYazarlar));
                }
            }
        }

        private void Kitap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "TopluKitapGirişi" && !Kitap.TopluKitapGirişi)
            {
                Kitap.TopluKitapSayısı = 1;
            }
        }

        private Kitap KitapOluştur()
        {
            return new Kitap
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
                Yazarlar = SeçiliYazarlar,
                Türler = SeçiliKitapTürleri,
                Resim = Kitap.Resim
            };
        }
    }
}