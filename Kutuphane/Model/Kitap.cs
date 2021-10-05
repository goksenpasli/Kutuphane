﻿using Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "Kitap")]
    public class Kitap : InpcBase, IDataErrorInfo
    {
        private string açıklama;

        private string ad;

        private string barkod;

        private int basımYılı = DateTime.Now.Year;

        private bool demirbaş;

        private IEnumerable<string> diller;

        private int dolapAltKod = 1;

        private int dolapId;

        private bool favori;

        private double fiyat = 1;

        private int ıd;

        private ObservableCollection<Kişi> kişiler = new();

        private string kitapDili;

        private bool kitapDurum = true;

        private int kitapDurumId;

        private bool kitapSayıOtomatikArttır;

        private bool otomatikBarkod;

        private bool ödünçVerilebilir = true;

        private string resim;

        private ObservableCollection<KitapTürü> seçiliKitapTürleri = new();

        private ObservableCollection<Yazar> seçiliYazarlar = new();

        private DateTime sistemKayıtTarihi = DateTime.Now;

        private bool topluKitapGirişi;

        private double topluKitapSayısı = 1;

        private bool tutanak;

        private string tür;

        private ObservableCollection<KitapTürü> türler = new();

        private string yazar;

        private ObservableCollection<Yazar> yazarlar = new();

        [XmlAttribute(AttributeName = "Açıklama")]
        public string Açıklama
        {
            get => açıklama;

            set
            {
                if (açıklama != value)
                {
                    açıklama = value;
                    OnPropertyChanged(nameof(Açıklama));
                }
            }
        }

        [XmlAttribute(AttributeName = "Ad")]
        public string Ad
        {
            get => ad;

            set
            {
                if (ad != value)
                {
                    ad = value;
                    OnPropertyChanged(nameof(Ad));
                }
            }
        }

        [XmlAttribute(AttributeName = "Barkod")]
        public string Barkod
        {
            get => barkod;

            set
            {
                if (barkod != value)
                {
                    barkod = value;
                    OnPropertyChanged(nameof(Barkod));
                }
            }
        }

        [XmlAttribute(AttributeName = "BasımYılı")]
        public int BasımYılı
        {
            get => basımYılı;

            set
            {
                if (basımYılı != value)
                {
                    basımYılı = value;
                    OnPropertyChanged(nameof(BasımYılı));
                }
            }
        }

        [XmlAttribute(AttributeName = "Demirbaş")]
        public bool Demirbaş
        {
            get => demirbaş;

            set
            {
                if (demirbaş != value)
                {
                    demirbaş = value;
                    OnPropertyChanged(nameof(Demirbaş));
                }
            }
        }

        [XmlIgnore]
        public IEnumerable<string> Diller
        {
            get
            {
                diller = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(z => !z.DisplayName.Contains('(')).Select(z => z.DisplayName);
                return diller;
            }

            set => diller = value;
        }

        [XmlAttribute(AttributeName = "DolapAltKod")]
        public int DolapAltKod
        {
            get => dolapAltKod;

            set
            {
                if (dolapAltKod != value)
                {
                    dolapAltKod = value;
                    OnPropertyChanged(nameof(DolapAltKod));
                }
            }
        }

        [XmlAttribute(AttributeName = "DolapId")]
        public int DolapId
        {
            get => dolapId;

            set
            {
                if (dolapId != value)
                {
                    dolapId = value;
                    OnPropertyChanged(nameof(DolapId));
                }
            }
        }

        public string Error => string.Empty;

        [XmlAttribute(AttributeName = "Favori")]
        public bool Favori
        {
            get => favori;

            set
            {
                if (favori != value)
                {
                    favori = value;
                    OnPropertyChanged(nameof(Favori));
                }
            }
        }

        [XmlAttribute(AttributeName = "Fiyat")]
        public double Fiyat
        {
            get => fiyat;

            set
            {
                if (fiyat != value)
                {
                    fiyat = value;
                    OnPropertyChanged(nameof(Fiyat));
                }
            }
        }

        [XmlAttribute(AttributeName = "Id")]
        public int Id
        {
            get => ıd;

            set
            {
                if (ıd != value)
                {
                    ıd = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        [XmlElement(ElementName = "Kişiler")]
        public ObservableCollection<Kişi> Kişiler
        {
            get => kişiler;

            set
            {
                if (kişiler != value)
                {
                    kişiler = value;
                    OnPropertyChanged(nameof(Kişiler));
                }
            }
        }

        [XmlAttribute(AttributeName = "KitapDili")]
        public string KitapDili
        {
            get => kitapDili;

            set
            {
                if (kitapDili != value)
                {
                    kitapDili = value;
                    OnPropertyChanged(nameof(KitapDili));
                }
            }
        }

        [XmlIgnore]
        public bool KitapDurum
        {
            get => kitapDurum;

            set
            {
                if (kitapDurum != value)
                {
                    kitapDurum = value;
                    OnPropertyChanged(nameof(KitapDurum));
                }
            }
        }

        [XmlAttribute(AttributeName = "KitapDurumId")]
        public int KitapDurumId
        {
            get => kitapDurumId;

            set
            {
                if (kitapDurumId != value)
                {
                    kitapDurumId = value;
                    OnPropertyChanged(nameof(KitapDurumId));
                }
            }
        }

        [XmlIgnore]
        public bool KitapSayıOtomatikArttır
        {
            get => kitapSayıOtomatikArttır;

            set
            {
                if (kitapSayıOtomatikArttır != value)
                {
                    kitapSayıOtomatikArttır = value;
                    OnPropertyChanged(nameof(KitapSayıOtomatikArttır));
                }
            }
        }

        [XmlIgnore]
        public bool OtomatikBarkod
        {
            get => otomatikBarkod;

            set
            {
                if (otomatikBarkod != value)
                {
                    otomatikBarkod = value;
                    OnPropertyChanged(nameof(OtomatikBarkod));
                }
            }
        }

        [XmlAttribute(AttributeName = "ÖdünçVerilebilir")]
        public bool ÖdünçVerilebilir
        {
            get => ödünçVerilebilir;

            set
            {
                if (ödünçVerilebilir != value)
                {
                    ödünçVerilebilir = value;
                    OnPropertyChanged(nameof(ÖdünçVerilebilir));
                }
            }
        }

        [XmlAttribute(AttributeName = "Resim")]
        public string Resim
        {
            get => resim;

            set
            {
                if (resim != value)
                {
                    resim = value;
                    OnPropertyChanged(nameof(Resim));
                }
            }
        }

        [XmlIgnore]
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

        [XmlIgnore]
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

        [XmlAttribute(AttributeName = "SistemKayıtTarihi")]
        public DateTime SistemKayıtTarihi
        {
            get => sistemKayıtTarihi;

            set
            {
                if (sistemKayıtTarihi != value)
                {
                    sistemKayıtTarihi = value;
                    OnPropertyChanged(nameof(SistemKayıtTarihi));
                }
            }
        }

        [XmlIgnore]
        public bool TopluKitapGirişi

        {
            get => topluKitapGirişi;

            set
            {
                if (topluKitapGirişi != value)
                {
                    topluKitapGirişi = value;
                    OnPropertyChanged(nameof(TopluKitapGirişi));
                }
            }
        }

        [XmlIgnore]
        public double TopluKitapSayısı
        {
            get => topluKitapSayısı;

            set
            {
                if (topluKitapSayısı != value)
                {
                    topluKitapSayısı = value;
                    OnPropertyChanged(nameof(TopluKitapSayısı));
                }
            }
        }

        [XmlAttribute(AttributeName = "Tutanak")]
        public bool Tutanak
        {
            get => tutanak;

            set
            {
                if (tutanak != value)
                {
                    tutanak = value;
                    OnPropertyChanged(nameof(Tutanak));
                }
            }
        }

        [XmlIgnore]
        public string Tür
        {
            get => tür;

            set
            {
                if (tür != value)
                {
                    tür = value;
                    OnPropertyChanged(nameof(Tür));
                }
            }
        }

        [XmlElement(ElementName = "Türler")]
        public ObservableCollection<KitapTürü> Türler
        {
            get => türler;

            set
            {
                if (türler != value)
                {
                    türler = value;
                    OnPropertyChanged(nameof(Türler));
                }
            }
        }

        [XmlIgnore]
        public string Yazar
        {
            get => yazar;

            set
            {
                if (yazar != value)
                {
                    yazar = value;
                    OnPropertyChanged(nameof(Yazar));
                }
            }
        }

        [XmlElement(ElementName = "Yazarlar")]
        public ObservableCollection<Yazar> Yazarlar
        {
            get => yazarlar;

            set
            {
                if (yazarlar != value)
                {
                    yazarlar = value;
                    OnPropertyChanged(nameof(Yazarlar));
                }
            }
        }

        [XmlIgnore]
        public IEnumerable<int> Yıllar { get; set; } = Enumerable.Range(DateTime.Now.Year - 50, 100);

        public string this[string columnName] => columnName switch
        {
            "Ad" when string.IsNullOrWhiteSpace(Ad) => "Adı Boş Geçmeyin.",
            "Barkod" when string.IsNullOrWhiteSpace(Barkod) => "Barkod Boş Geçmeyin.",
            _ => null
        };
    }
}