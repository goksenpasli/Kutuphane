using System;
using System.Xml.Serialization;

namespace Kutuphane.Model
{
    [Serializable]
    public enum KitapDurumu
    {
        [XmlEnum(Name = "Kütüphanede")]
        Kütüphanede = 0,

        [XmlEnum(Name = "Okuyucuda")]
        Okuyucuda = 1,

        [XmlEnum(Name = "Kayıp")]
        Kayıp = 2,

        [XmlEnum(Name = "Yıpranmış")]
        Yıpranmış = 3,
    }
}