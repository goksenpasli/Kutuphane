using Extensions;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "Kütüphane")]
    public class Kütüphane : InpcBase
    {
        private ObservableCollection<Dolap> dolaplar = new();

        private ObservableCollection<İşlem> işlemler = new();

        private ObservableCollection<Kişi> kişiler = new();

        private ObservableCollection<Kitap> kitaplar = new();

        private ObservableCollection<KitapTürü> kitapTürleri = new();

        private ObservableCollection<Yazar> yazarlar = new();

        [XmlElement(ElementName = "Dolap")]
        public ObservableCollection<Dolap> Dolaplar
        {
            get => dolaplar;

            set
            {
                if (dolaplar != value)
                {
                    dolaplar = value;
                    OnPropertyChanged(nameof(Dolaplar));
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<İşlem> İşlemler
        {
            get => işlemler;

            set
            {
                if (işlemler != value)
                {
                    işlemler = value;
                    OnPropertyChanged(nameof(İşlemler));
                }
            }
        }

        [XmlElement(ElementName = "Kişi")]
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

        [XmlElement(ElementName = "Kitap")]
        public ObservableCollection<Kitap> Kitaplar
        {
            get => kitaplar;

            set
            {
                if (kitaplar != value)
                {
                    kitaplar = value;
                    OnPropertyChanged(nameof(Kitaplar));
                }
            }
        }

        [XmlElement(ElementName = "KitapTürü")]
        public ObservableCollection<KitapTürü> KitapTürleri
        {
            get => kitapTürleri;

            set
            {
                if (kitapTürleri != value)
                {
                    kitapTürleri = value;
                    OnPropertyChanged(nameof(KitapTürleri));
                }
            }
        }

        [XmlElement(ElementName = "Yazar")]
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
    }
}