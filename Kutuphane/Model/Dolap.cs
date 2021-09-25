using Extensions;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "Dolap")]
    public class Dolap : InpcBase
    {
        private string açıklama;

        private bool etkin = true;

        private int ıd;

        private double kod = 1;

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

        [XmlAttribute(AttributeName = "Etkin")]
        public bool Etkin
        {
            get => etkin;

            set
            {
                if (etkin != value)
                {
                    etkin = value;
                    OnPropertyChanged(nameof(Etkin));
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

        [XmlAttribute(AttributeName = "Kod")]
        public double Kod
        {
            get => kod;

            set
            {
                if (kod != value)
                {
                    kod = value;
                    OnPropertyChanged(nameof(Kod));
                }
            }
        }
    }
}