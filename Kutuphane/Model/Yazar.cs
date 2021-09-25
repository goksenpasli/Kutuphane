using Extensions;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [XmlRoot(ElementName = "Yazar")]
    public class Yazar : InpcBase
    {
        private string ad;

        private int ıd;

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
    }
}