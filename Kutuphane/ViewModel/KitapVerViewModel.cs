﻿using Extensions;
using Kutuphane.Model;
using Kutuphane.Properties;
using Kutuphane.View;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class KitapVerViewModel : InpcBase
    {
        private İşlem işlem;

        private Kişi kişi;

        private Kişi seçiliKişi;

        public KitapVerViewModel()
        {
            Kişi = new Kişi();
            İşlem = new İşlem();

            KitapVer = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap && MessageBox.Show($"{kitap.Ad} Adlı Kitap {kişi.Ad} {kişi.Soyad} Adlı Kişiye {İşlem.KitapGün} Günlüğüne Verilecek Onaylıyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (Settings.Default.KişiKitapKritikKontrol && kişi.KitapCezasıOranı > Settings.Default.KişiKitapKritikOran)
                    {
                        _ = MessageBox.Show("Bu Kişinin Kitap Geri Verme Durumu Problemli Bu Kişiye Kitap Verilmez.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    İşlem işlem = new()
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        KitapGün = İşlem.KitapGün,
                        GeriGetirmeTarihi = İşlem.GeriGetirmeTarihi,
                        KitapId = kitap.Id,
                        KişiId = kişi.Id,
                        BaşlangıçTarihi = İşlem.BaşlangıçTarihi,
                    };
                    kişi.İşlem.Add(işlem);
                    kitap.KitapDurumId = (int)KitapDurumu.Okuyucuda;
                    MainViewModel.DatabaseSave.Execute(null);

                    İşlem.KitapGün = 1;
                    İşlem.BaşlangıçTarihi = DateTime.Today;

                    if (Settings.Default.OtomatikTutanak)
                    {
                        işlem.SeçiliKitap = kitap;
                        new ReportViewModel().KitapTutanakRaporu.Execute(new object[] { kişi, işlem });
                    }
                }
            }, parameter => KitapAlabilir(parameter));

            HızlıKitapVer = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap)
                {
                    if (Settings.Default.KişiKitapKritikKontrol && kişi.KitapCezasıOranı > Settings.Default.KişiKitapKritikOran)
                    {
                        _ = MessageBox.Show("Bu Kişinin Kitap Geri Verme Durumu Problemli Bu Kişiye Kitap Verilmez.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    İşlem işlem = new()
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        KitapGün = Settings.Default.HızlıKitapGirişGünSüresi,
                        GeriGetirmeTarihi = Settings.Default.KitapVermeİşGünüSay ? DateTime.Today.İşGünüEkle(Settings.Default.HızlıKitapGirişGünSüresi) : DateTime.Today.AddDays(Settings.Default.HızlıKitapGirişGünSüresi),
                        KitapId = kitap.Id,
                        KişiId = kişi.Id,
                        BaşlangıçTarihi = DateTime.Today,
                    };
                    kişi.İşlem.Add(işlem);
                    kitap.KitapDurumId = (int)KitapDurumu.Okuyucuda;
                    MainViewModel.DatabaseSave.Execute(null);

                    if (Settings.Default.OtomatikTutanak)
                    {
                        işlem.SeçiliKitap = kitap;
                        new ReportViewModel().KitapTutanakRaporu.Execute(new object[] { kişi, işlem });
                    }
                }
            }, parameter => KitapAlabilir(parameter));

            KitapSeçiliEvrakAktar = new RelayCommand<object>(parameter =>
            {
                if (SeçiliKişi is not null)
                {
                    OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Dosyalar (*.jpg;*.jpeg;*.tif;*.tiff;*.png;*.pdf;*.xps;*.zip)|*.jpg;*.jpeg;*.tif;*.tiff;*.png;*.pdf;*.xps;*.zip" };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                        File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                        SeçiliKişi.TutanakYolu.Add(filename);
                        MainViewModel.DatabaseSave.Execute(null);
                        _ = MessageBox.Show("Seçili Evrak Eklendi.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }, parameter => SeçiliKişi is not null);

            KitapTarananEvrakAktar = new RelayCommand<object>(parameter =>
            {
                if (parameter is ObservableCollection<BitmapFrame> seçiliResimler && SeçiliKişi is not null)
                {
                    foreach (BitmapFrame resim in seçiliResimler)
                    {
                        string filename = null;
                        if (resim.Format == PixelFormats.Bgr32)
                        {
                            filename = Guid.NewGuid() + ".jpg";
                            File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", resim.ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Jpg));
                        }
                        else if (resim.Format == PixelFormats.Bgra32)
                        {
                            filename = Guid.NewGuid() + ".tif";
                            File.WriteAllBytes($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}", resim.ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Tiff));
                        }

                        SeçiliKişi.TutanakYolu.Add(filename);
                    }
                    MainViewModel.DatabaseSave.Execute(null);
                    _ = MessageBox.Show("Taranan Evrak Eklendi.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }, parameter => parameter is ObservableCollection<BitmapFrame> seçiliResimler && seçiliResimler.Any() && SeçiliKişi is not null);

            İşlem.GeriGetirmeTarihi = Settings.Default.KitapVermeİşGünüSay ? İşlem.BaşlangıçTarihi.İşGünüEkle(İşlem.KitapGün) : İşlem.BaşlangıçTarihi.AddDays(İşlem.KitapGün);
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

        public ICommand KitapSeçiliEvrakAktar { get; }

        public ICommand KitapTarananEvrakAktar { get; }

        public ICommand KitapVer { get; }

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

        private static bool KitapAlabilir(object parameter)
        {
            return parameter is object[] data && data[0] is Kişi kişi && data[1] is Kitap kitap && kişi.KitapAlabilir && kitap.ÖdünçVerilebilir;
        }

        private void İşlem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "BaşlangıçTarihi" or "KitapGün")
            {
                İşlem.GeriGetirmeTarihi = Settings.Default.KitapVermeİşGünüSay ? İşlem.BaşlangıçTarihi.İşGünüEkle(İşlem.KitapGün) : İşlem.BaşlangıçTarihi.AddDays(İşlem.KitapGün);
            }
        }

        private void Kişi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KitapVerView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Ad.Contains(Kişi.KişiKitapAdArama) == true;
            }

            if (e.PropertyName is "KişiKitapBarkodArama")
            {
                KitapVerView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Barkod.Contains(Kişi.KişiKitapBarkodArama) == true;
            }

            if (e.PropertyName is "KişiAdArama")
            {
                KitapVerView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.Ad.Contains(Kişi.KişiAdArama) == true;
            }

            if (e.PropertyName is "KişiSoyadArama")
            {
                KitapVerView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.Soyad.Contains(Kişi.KişiSoyadArama) == true;
            }

            if (e.PropertyName is "KişiTcArama")
            {
                KitapVerView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi)?.TC.Contains(Kişi.KişiTcArama) == true;
            }
        }
    }
}