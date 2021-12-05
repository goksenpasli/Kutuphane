using Extensions;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "KitapTürü")]
    public class KitapTürü : InpcBase
    {
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

        private string açıklama;

        private int ıd;
    }
}