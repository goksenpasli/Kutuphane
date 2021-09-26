using ExcelDataReader;
using Extensions;
using Kutuphane.Model;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapGirişViewModel : InpcBase
    {
        public KitapGirişViewModel()
        {
            Kitap = new Kitap();

            KitapEkle = new RelayCommand<object>(parameter =>
            {
                var kitap = KitapOluştur();
                if (Kitap.TopluKitapGirişi)
                {
                    for (var i = 0; i < Kitap.TopluKitapSayısı; i++)
                    {
                        kitap = KitapOluştur();
                        (parameter as Kütüphane)?.Kitaplar.Add(kitap);
                    }
                }
                else
                {
                    (parameter as Kütüphane)?.Kitaplar.Add(kitap);
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

            }, parameter => Kitap.DolapId != 0 && !string.IsNullOrWhiteSpace(Kitap?.Ad));

            KitapTürEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    var tür = new KitapTürü() { Açıklama = Kitap.Tür, Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue) };
                    kütüphane?.KitapTürleri.Add(tür);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kütüphane kütüphane && !kütüphane.KitapTürleri.Any(z => z.Açıklama == Kitap.Açıklama) && !string.IsNullOrWhiteSpace(Kitap?.Tür));

            KitapYazarEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    var yazar = new Yazar() { Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue), Ad = Kitap.Yazar };
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

            TopluKitapEkle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Excel Dosyaları (*.xls;*.xlsx)|*.xls;*.xlsx" };
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        using var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using var reader = ExcelReaderFactory.CreateReader(stream);
                        do
                        {
                            while (reader.Read())
                            {
                                (parameter as Kütüphane)?.Kitaplar.Add(new Kitap
                                {
                                    Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                                    Ad = reader.GetValue(0).ToString(),
                                    Barkod = reader.GetValue(1).ToString(),
                                    DolapId = Kitap.DolapId
                                });
                            }
                        } while (reader.NextResult());
                        MainViewModel.DatabaseSave.Execute(null);
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show(ex.Message, "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }, parameter => Kitap.DolapId != 0);

            Kitap.PropertyChanged += Kitap_PropertyChanged;
        }

        public Kitap Kitap { get; set; }

        public ICommand KitapEkle { get; }

        public ICommand KitapResimEkle { get; }

        public ICommand KitapTürEkle { get; }

        public ICommand KitapYazarEkle { get; }

        public ICommand TopluKitapEkle { get; }

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
            var kitap = new Kitap
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
                KitapDili = Kitap.KitapDili
            };
            foreach (var item in Kitap.SeçiliYazarlar)
            {
                kitap.Yazarlar.Add(item);
            }
            foreach (var item in Kitap.SeçiliKitapTürleri)
            {
                kitap.Türler.Add(item);
            }
            return kitap;
        }

        private void ResetKitap()
        {
            Kitap.Ad = null;
            Kitap.Resim = null;
            Kitap.Barkod = null;
            Kitap.Açıklama = null;
            Kitap.TopluKitapSayısı = 1;
        }
    }
}