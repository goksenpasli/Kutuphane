using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Specialized;

namespace Kutuphane.ViewModel
{
    public class KitapGüncelleViewModel : InpcBase
    {
        public KitapGüncelleViewModel()
        {
            KitapGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is ObservableCollection<Kitap> kitaplar)
                {
                    if (kitaplar.All(z => !string.IsNullOrWhiteSpace(z.Barkod) && !string.IsNullOrWhiteSpace(z.Ad)))
                    {
                        MainViewModel.DatabaseSave.Execute(null);
                    }
                    else
                    {
                        MessageBox.Show("Hatalı Girişleri Düzeltin.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }, parameter => parameter is ObservableCollection<Kitap> kitaplar && kitaplar?.Any(z=>z.Seçili) == true);

            KitapGit = new RelayCommand<object>(parameter =>
            {
                if (parameter is ListView lv && SeçiliKitap is not null)
                {
                    lv.ScrollIntoView(SeçiliKitap);
                }
            }, parameter => true);

            DolapAra = new RelayCommand<object>(parameter => KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= SeçiliDolaplar.Any(z => z.Seçili && z.Id == (e.Item as Kitap)?.DolapId), parameter => SeçiliDolaplar?.Any(z => z.Seçili) == true);

            ResetFilter = new RelayCommand<object>(parameter => KitapGüncelleView.cvs.View.Filter = null, parameter => true);

            KitapTopluResimGüncelle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                    foreach (Kitap kitap in SeçiliKitaplar.ToList())
                    {
                        kitap.Resim = filename;
                    }
                }
            }, parameter => true);

            PropertyChanged += KitapGüncelleViewModel_PropertyChanged;
            KitapTopluYazarlar.CollectionChanged += KitapTopluYazarlar_CollectionChanged;
            KitapTopluTürler.CollectionChanged += KitapTopluTürler_CollectionChanged;
        }

        public ICommand DolapAra { get; }

        public string KişiKitapAdArama
        {
            get => kişiKitapAdArama;

            set
            {
                if (kişiKitapAdArama != value)
                {
                    kişiKitapAdArama = value;
                    OnPropertyChanged(nameof(KişiKitapAdArama));
                }
            }
        }

        public string KişiKitapBarkodArama
        {
            get => kişiKitapBarkodArama;

            set
            {
                if (kişiKitapBarkodArama != value)
                {
                    kişiKitapBarkodArama = value;
                    OnPropertyChanged(nameof(KişiKitapBarkodArama));
                }
            }
        }

        public int KişiKitapKonumArama
        {
            get => kişiKitapKonumArama;

            set
            {
                if (kişiKitapKonumArama != value)
                {
                    kişiKitapKonumArama = value;
                    OnPropertyChanged(nameof(KişiKitapKonumArama));
                }
            }
        }

        public string KişiKitapRenkArama
        {
            get => kişiKitapRenkArama;

            set
            {
                if (kişiKitapRenkArama != value)
                {
                    kişiKitapRenkArama = value;
                    OnPropertyChanged(nameof(KişiKitapRenkArama));
                }
            }
        }

        public int KişiKitapYılArama
        {
            get => kişiKitapYılArama;

            set
            {
                if (kişiKitapYılArama != value)
                {
                    kişiKitapYılArama = value;
                    OnPropertyChanged(nameof(KişiKitapYılArama));
                }
            }
        }

        public ICommand KitapGit { get; }

        public ICommand KitapGüncelle { get; }

        public string KitapTopluAd
        {
            get => kitapTopluAd;

            set
            {
                if (kitapTopluAd != value)
                {
                    kitapTopluAd = value;
                    OnPropertyChanged(nameof(KitapTopluAd));
                }
            }
        }

        public string KitapTopluBarkod
        {
            get => kitapTopluBarkod;

            set
            {
                if (kitapTopluBarkod != value)
                {
                    kitapTopluBarkod = value;
                    OnPropertyChanged(nameof(KitapTopluBarkod));
                }
            }
        }

        public int? KitapTopluBasımYılı
        {
            get => kitapTopluBasımYılı;

            set
            {
                if (kitapTopluBasımYılı != value)
                {
                    kitapTopluBasımYılı = value;
                    OnPropertyChanged(nameof(KitapTopluBasımYılı));
                }
            }
        }

        public bool? KitapTopluDemirbaş
        {
            get => kitapTopluDemirbaş;

            set
            {
                if (kitapTopluDemirbaş != value)
                {
                    kitapTopluDemirbaş = value;
                    OnPropertyChanged(nameof(KitapTopluDemirbaş));
                }
            }
        }

        public string KitapTopluDili
        {
            get => kitapTopluDili;

            set
            {
                if (kitapTopluDili != value)
                {
                    kitapTopluDili = value;
                    OnPropertyChanged(nameof(KitapTopluDili));
                }
            }
        }

        public int? KitapTopluDolapId
        {
            get => kitapTopluDolapId;

            set
            {
                if (kitapTopluDolapId != value)
                {
                    kitapTopluDolapId = value;
                    OnPropertyChanged(nameof(KitapTopluDolapId));
                }
            }
        }

        public bool? KitapTopluFavori
        {
            get => kitapTopluFavori;

            set
            {
                if (kitapTopluFavori != value)
                {
                    kitapTopluFavori = value;
                    OnPropertyChanged(nameof(KitapTopluFavori));
                }
            }
        }

        public double KitapTopluFiyat
        {
            get => kitapTopluFiyat;

            set
            {
                if (kitapTopluFiyat != value)
                {
                    kitapTopluFiyat = value;
                    OnPropertyChanged(nameof(KitapTopluFiyat));
                }
            }
        }

        public bool? KitapTopluÖdünç
        {
            get => kitapTopluÖdünç;

            set
            {
                if (kitapTopluÖdünç != value)
                {
                    kitapTopluÖdünç = value;
                    OnPropertyChanged(nameof(KitapTopluÖdünç));
                }
            }
        }

        public string KitapTopluRenk
        {
            get => kitapTopluRenk;

            set
            {
                if (kitapTopluRenk != value)
                {
                    kitapTopluRenk = value;
                    OnPropertyChanged(nameof(KitapTopluRenk));
                }
            }
        }

        public ICommand KitapTopluResimGüncelle { get; }

        public ObservableCollection<KitapTürü> KitapTopluTürler
        {
            get => kitapTopluTürler;

            set
            {
                if (kitapTopluTürler != value)
                {
                    kitapTopluTürler = value;
                    OnPropertyChanged(nameof(KitapTopluTürler));
                }
            }
        }

        public ObservableCollection<Yazar> KitapTopluYazarlar
        {
            get => kitapTopluYazarlar;

            set
            {
                if (kitapTopluYazarlar != value)
                {
                    kitapTopluYazarlar = value;
                    OnPropertyChanged(nameof(KitapTopluYazarlar));
                }
            }
        }

        public ICommand ResetFilter { get; }

        public ObservableCollection<Dolap> SeçiliDolaplar
        {
            get => seçiliDolaplar;

            set
            {
                if (seçiliDolaplar != value)
                {
                    seçiliDolaplar = value;
                    OnPropertyChanged(nameof(SeçiliDolaplar));
                }
            }
        }

        public Kitap SeçiliKitap
        {
            get => seçiliKitap;

            set
            {
                if (seçiliKitap != value)
                {
                    seçiliKitap = value;
                    OnPropertyChanged(nameof(SeçiliKitap));
                }
            }
        }

        public ObservableCollection<Kitap> SeçiliKitaplar
        {
            get => seçiliKitaplar;

            set
            {
                if (seçiliKitaplar != value)
                {
                    seçiliKitaplar = value;
                    OnPropertyChanged(nameof(SeçiliKitaplar));
                }
            }
        }

        private string kişiKitapAdArama;

        private string kişiKitapBarkodArama;

        private int kişiKitapKonumArama = 4;

        private string kişiKitapRenkArama;

        private int kişiKitapYılArama;

        private string kitapTopluAd;

        private string kitapTopluBarkod;

        private int? kitapTopluBasımYılı;

        private bool? kitapTopluDemirbaş;

        private string kitapTopluDili;

        private int? kitapTopluDolapId;

        private bool? kitapTopluFavori;

        private double kitapTopluFiyat;

        private bool? kitapTopluÖdünç;

        private string kitapTopluRenk;

        private ObservableCollection<KitapTürü> kitapTopluTürler = new();

        private ObservableCollection<Yazar> kitapTopluYazarlar = new();

        private ObservableCollection<Dolap> seçiliDolaplar = new();

        private Kitap seçiliKitap;

        private ObservableCollection<Kitap> seçiliKitaplar = new();

        private void KitapGüncelleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "KişiKitapAdArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Ad.Contains(KişiKitapAdArama) == true;
                    break;

                case "KişiKitapBarkodArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Barkod.Contains(KişiKitapBarkodArama) == true;
                    break;

                case "KişiKitapYılArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.BasımYılı == KişiKitapYılArama;
                    break;

                case "KişiKitapRenkArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Renk == KişiKitapRenkArama;
                    break;

                case "KişiKitapKonumArama":
                    if (KişiKitapKonumArama != 4)
                    {
                        KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.KitapDurumId == KişiKitapKonumArama;
                    }
                    else
                    {
                        ResetFilter.Execute(null);
                    }
                    break;

                case "KitapTopluAd":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.Ad = KitapTopluAd;
                    }
                    break;

                case "KitapTopluBarkod":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.Barkod = KitapTopluBarkod;
                    }
                    break;

                case "KitapTopluBasımYılı":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        if (KitapTopluBasımYılı is not null)
                        {
                            kitap.BasımYılı = KitapTopluBasımYılı ?? DateTime.Now.Year;
                        }
                    }
                    break;

                case "KitapTopluFiyat":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        if (KitapTopluFiyat is not 0)
                        {
                            kitap.Fiyat = KitapTopluFiyat;
                        }
                    }
                    break;

                case "KitapTopluDemirbaş":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.Demirbaş = KitapTopluDemirbaş == true;
                    }
                    break;

                case "KitapTopluFavori":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.Favori = KitapTopluFavori == true;
                    }
                    break;

                case "KitapTopluÖdünç":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.ÖdünçVerilebilir = KitapTopluÖdünç == true;
                    }
                    break;

                case "KitapTopluDolapId":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        if (KitapTopluDolapId is not null)
                        {
                            kitap.DolapId = KitapTopluDolapId ?? 0;
                        }
                    }
                    break;

                case "KitapTopluRenk":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.Renk = KitapTopluRenk;
                    }
                    break;

                case "KitapTopluDili":
                    foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                    {
                        kitap.KitapDili = KitapTopluDili;
                    }
                    break;
            }
        }

        private void KitapTopluTürler_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Add && e.NewItems is not null)
            {
                foreach ((Kitap kitap, KitapTürü tür) in from Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList() from KitapTürü tür in e.NewItems select (kitap, tür))
                {
                    kitap.Türler.Add(tür);
                }
            }

            if (e.Action is NotifyCollectionChangedAction.Reset)
            {
                foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                {
                    kitap.Türler.Clear();
                }
            }
        }

        private void KitapTopluYazarlar_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Add && e.NewItems is not null)
            {
                foreach ((Kitap kitap, Yazar yazar) in from Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList() from Yazar yazar in e.NewItems select (kitap, yazar))
                {
                    kitap.Yazarlar.Add(yazar);
                }
            }

            if (e.Action is NotifyCollectionChangedAction.Reset)
            {
                foreach (Kitap kitap in SeçiliKitaplar.Where(z => z.Seçili).ToList())
                {
                    kitap.Yazarlar.Clear();
                }
            }
        }
    }
}