using Extensions;
using System;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "İşlem")]
    public class İşlem : InpcBase
    {
        private DateTime başlangıçTarihi = DateTime.Today;

        private bool ceza;

        private DateTime cezaÖdemeTarihi;

        private double cezaTutar;

        private DateTime geriGetirmeTarihi = DateTime.Today;

        private int ıd;

        private bool işlemBitti;

        private double kitapGün = 1;

        private int kitapId;

        private Kitap seçiliKitap;

        private bool uzatıldı;

        private int uzatmaSayısı;

        [XmlAttribute(AttributeName = "BaşlangıçTarihi")]
        public DateTime BaşlangıçTarihi
        {
            get => başlangıçTarihi;

            set
            {
                if (başlangıçTarihi != value)
                {
                    başlangıçTarihi = value;
                    OnPropertyChanged(nameof(BaşlangıçTarihi));
                    OnPropertyChanged(nameof(GeriGetirmeTarihi));
                }
            }
        }

        [XmlAttribute(AttributeName = "Ceza")]
        public bool Ceza
        {
            get => ceza;

            set
            {
                if (ceza != value)
                {
                    ceza = value;
                    OnPropertyChanged(nameof(Ceza));
                }
            }
        }

        [XmlAttribute(AttributeName = "CezaÖdemeTarihi")]
        public DateTime CezaÖdemeTarihi
        {
            get => cezaÖdemeTarihi;

            set
            {
                if (cezaÖdemeTarihi != value)
                {
                    cezaÖdemeTarihi = value;
                    OnPropertyChanged(nameof(CezaÖdemeTarihi));
                }
            }
        }

        [XmlAttribute(AttributeName = "CezaTutar")]
        public double CezaTutar
        {
            get => cezaTutar;

            set
            {
                if (cezaTutar != value)
                {
                    cezaTutar = value;
                    OnPropertyChanged(nameof(CezaTutar));
                }
            }
        }

        [XmlAttribute(AttributeName = "GeriGetirmeTarihi")]
        public DateTime GeriGetirmeTarihi
        {
            get => geriGetirmeTarihi;

            set
            {
                if (geriGetirmeTarihi != value)
                {
                    geriGetirmeTarihi = value;
                    OnPropertyChanged(nameof(GeriGetirmeTarihi));
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

        [XmlAttribute(AttributeName = "İşlemBitti")]
        public bool İşlemBitti
        {
            get => işlemBitti;

            set
            {
                if (işlemBitti != value)
                {
                    işlemBitti = value;
                    OnPropertyChanged(nameof(İşlemBitti));
                }
            }
        }

        [XmlAttribute(AttributeName = "KitapGün")]
        public double KitapGün
        {
            get => kitapGün;

            set
            {
                if (kitapGün != value)
                {
                    kitapGün = value;
                    OnPropertyChanged(nameof(KitapGün));
                    OnPropertyChanged(nameof(GeriGetirmeTarihi));
                }
            }
        }

        [XmlAttribute(AttributeName = "KitapId")]
        public int KitapId
        {
            get => kitapId;

            set
            {
                if (kitapId != value)
                {
                    kitapId = value;
                    OnPropertyChanged(nameof(KitapId));
                }
            }
        }

        [XmlIgnore]
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

        [XmlAttribute(AttributeName = "Uzatıldı")]
        public bool Uzatıldı
        {
            get => uzatıldı;

            set
            {
                if (uzatıldı != value)
                {
                    uzatıldı = value;
                    OnPropertyChanged(nameof(Uzatıldı));
                }
            }
        }

        [XmlAttribute(AttributeName = "UzatmaSayısı")]
        public int UzatmaSayısı
        {
            get => uzatmaSayısı;

            set
            {
                if (uzatmaSayısı != value)
                {
                    uzatmaSayısı = value;
                    OnPropertyChanged(nameof(UzatmaSayısı));
                }
            }
        }
    }
}