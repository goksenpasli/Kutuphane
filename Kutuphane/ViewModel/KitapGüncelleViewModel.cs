﻿using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapGüncelleViewModel : InpcBase
    {
        private string kişiKitapAdArama;

        private string kişiKitapBarkodArama;

        private int kişiKitapKonumArama = 4;

        private int kişiKitapYılArama;

        private string kitapTopluAd;

        private string kitapTopluBarkod;

        private int? kitapTopluBasımYılı;

        private bool? kitapTopluDemirbaş;

        private int? kitapTopluDolapId;

        private bool? kitapTopluÖdünç;

        private ObservableCollection<Dolap> seçiliDolaplar = new();

        private Kitap seçiliKitap;

        private List<Kitap> seçiliKitaplar = new();

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
            }, parameter => true);

            KitapGit = new RelayCommand<object>(parameter =>
            {
                if (parameter is ListView lv && SeçiliKitap is not null)
                {
                    lv.ScrollIntoView(SeçiliKitap);
                }
            }, parameter => true);

            DolapAra = new RelayCommand<object>(parameter => KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= SeçiliDolaplar.Any(z => z.Seçili && z.Id == (e.Item as Kitap)?.DolapId), parameter => SeçiliDolaplar?.Any(z => z.Seçili) == true);

            PropertyChanged += KitapGüncelleViewModel_PropertyChanged;
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

        public List<Kitap> SeçiliKitaplar
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

                case "KişiKitapKonumArama":
                    if (KişiKitapKonumArama != 4)
                    {
                        KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.KitapDurumId == KişiKitapKonumArama;
                    }
                    else
                    {
                        KitapGüncelleView.cvs.View.Filter = null;
                    }
                    break;

                case "KitapTopluAd":
                    foreach (Kitap kitap in SeçiliKitaplar)
                    {
                        kitap.Ad = KitapTopluAd;
                    }
                    break;

                case "KitapTopluBarkod":
                    foreach (Kitap kitap in SeçiliKitaplar)
                    {
                        kitap.Barkod = KitapTopluBarkod;
                    }
                    break;

                case "KitapTopluBasımYılı":
                    foreach (Kitap kitap in SeçiliKitaplar)
                    {
                        if (KitapTopluBasımYılı is not null)
                        {
                            kitap.BasımYılı = KitapTopluBasımYılı ?? DateTime.Now.Year;
                        }
                    }
                    break;

                case "KitapTopluDemirbaş":
                    foreach (Kitap kitap in SeçiliKitaplar)
                    {
                        kitap.Demirbaş = KitapTopluDemirbaş == true;
                    }
                    break;

                case "KitapTopluÖdünç":
                    foreach (Kitap kitap in SeçiliKitaplar)
                    {
                        kitap.ÖdünçVerilebilir = KitapTopluÖdünç == true;
                    }
                    break;

                case "KitapTopluDolapId":
                    foreach (Kitap kitap in SeçiliKitaplar)
                    {
                        if (KitapTopluDolapId is not null)
                        {
                            kitap.DolapId = KitapTopluDolapId ?? 0;
                        }
                    }
                    break;
            }
        }
    }
}