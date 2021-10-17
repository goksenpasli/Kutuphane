using Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "Kişi")]
    public class Kişi : InpcBase
    {
        private string _KimlikYazıRenk = "Black";

        private string ad;

        private string adres;

        private int cinsiyet = -1;

        private DateTime doğumTarihi = DateTime.Today;

        private int ıd;

        private ObservableCollection<İşlem> işlem = new();

        private DateTime kayıtTarihi = DateTime.Now;

        private string kişiAdArama;

        private string kişiKitapAdArama;

        private string kişiKitapBarkodArama;

        private string kişiSoyadArama;

        private string kişiTcArama;

        private bool kitapAlabilir = true;

        private int kitapCezasıAdeti;

        private double kitapCezasıOranı;

        private string resim;

        private Kişi sonKaydedilenKişi;

        private string soyad;

        private string tC;

        private string telefon;

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

        [XmlAttribute(AttributeName = "Adres")]
        public string Adres
        {
            get => adres;

            set
            {
                if (adres != value)
                {
                    adres = value;
                    OnPropertyChanged(nameof(Adres));
                }
            }
        }

        [XmlIgnore]
        public string AktarmaTC { get; set; }

        [XmlAttribute(AttributeName = "Cinsiyet")]
        public int Cinsiyet
        {
            get => cinsiyet;

            set
            {
                if (cinsiyet != value)
                {
                    cinsiyet = value;
                    OnPropertyChanged(nameof(Cinsiyet));
                }
            }
        }

        [XmlAttribute(AttributeName = "DoğumTarihi")]
        public DateTime DoğumTarihi
        {
            get => doğumTarihi;

            set
            {
                if (doğumTarihi != value)
                {
                    doğumTarihi = value;
                    OnPropertyChanged(nameof(DoğumTarihi));
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

        [XmlElement(ElementName = "İşlem")]
        public ObservableCollection<İşlem> İşlem
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

        [XmlAttribute(AttributeName = "KayıtTarihi")]
        public DateTime KayıtTarihi
        {
            get => kayıtTarihi;

            set
            {
                if (kayıtTarihi != value)
                {
                    kayıtTarihi = value;
                    OnPropertyChanged(nameof(KayıtTarihi));
                }
            }
        }

        [XmlIgnore]
        public string KimlikYazıRenk
        {
            get => _KimlikYazıRenk;

            set
            {
                if (_KimlikYazıRenk != value)
                {
                    _KimlikYazıRenk = value;
                    OnPropertyChanged(nameof(KimlikYazıRenk));
                }
            }
        }

        [XmlIgnore]
        public string KişiAdArama
        {
            get => kişiAdArama;

            set
            {
                if (kişiAdArama != value)
                {
                    kişiAdArama = value;
                    OnPropertyChanged(nameof(KişiAdArama));
                }
            }
        }

        [XmlIgnore]
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

        [XmlIgnore]
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

        [XmlIgnore]
        public string KişiSoyadArama
        {
            get => kişiSoyadArama;

            set
            {
                if (kişiSoyadArama != value)
                {
                    kişiSoyadArama = value;
                    OnPropertyChanged(nameof(KişiSoyadArama));
                }
            }
        }

        [XmlIgnore]
        public string KişiTcArama
        {
            get => kişiTcArama;

            set
            {
                if (kişiTcArama != value)
                {
                    kişiTcArama = value;
                    OnPropertyChanged(nameof(KişiTcArama));
                }
            }
        }

        [XmlAttribute(AttributeName = "KitapAlabilir")]
        public bool KitapAlabilir
        {
            get => kitapAlabilir;

            set
            {
                if (kitapAlabilir != value)
                {
                    kitapAlabilir = value;
                    OnPropertyChanged(nameof(KitapAlabilir));
                }
            }
        }

        [XmlIgnore]
        public int KitapCezasıAdeti
        {
            get
            {
                kitapCezasıAdeti = İşlem?.Count(z => z.Ceza) ?? 0;
                return kitapCezasıAdeti;
            }

            set
            {
                if (kitapCezasıAdeti != value)
                {
                    kitapCezasıAdeti = value;
                    OnPropertyChanged(nameof(KitapCezasıAdeti));
                    OnPropertyChanged(nameof(KitapCezasıOranı));
                }
            }
        }

        [XmlIgnore]
        public double KitapCezasıOranı
        {
            get
            {
                if (İşlem?.Any() == true)
                {
                    kitapCezasıOranı = 100 * (double)İşlem?.Count(z => z.Ceza) / İşlem?.Count ?? 0;
                }
                return kitapCezasıOranı;
            }

            set
            {
                if (kitapCezasıOranı != value)
                {
                    kitapCezasıOranı = value;
                    OnPropertyChanged(nameof(KitapCezasıOranı));
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
        public Kişi SonKaydedilenKişi
        {
            get => sonKaydedilenKişi;

            set
            {
                if (sonKaydedilenKişi != value)
                {
                    sonKaydedilenKişi = value;
                    OnPropertyChanged(nameof(SonKaydedilenKişi));
                }
            }
        }

        [XmlAttribute(AttributeName = "Soyad")]
        public string Soyad
        {
            get => soyad;

            set
            {
                if (soyad != value)
                {
                    soyad = value;
                    OnPropertyChanged(nameof(Soyad));
                }
            }
        }

        [XmlAttribute(AttributeName = "TC")]
        public string TC
        {
            get => tC;

            set
            {
                if (tC != value)
                {
                    tC = value;
                    OnPropertyChanged(nameof(TC));
                }
            }
        }

        [XmlAttribute(AttributeName = "Telefon")]
        public string Telefon
        {
            get => telefon;

            set
            {
                if (telefon != value)
                {
                    telefon = value;
                    OnPropertyChanged(nameof(Telefon));
                }
            }
        }
    }
}