using Extensions;
using System;
using System.Collections.Generic;

namespace Kutuphane.Model
{
    public class CsvData : InpcBase
    {
        public CsvData()
        {
            SampleData = new List<Kitap> { new Kitap() { Ad = "Kitap Adı", Barkod = "Kitap Barkod", Fiyat = 35, BasımYılı = DateTime.Now.Year, KitapDili = "Türkçe" } };
        }

        public bool Dil
        {
            get => dil;

            set
            {
                if (dil != value)
                {
                    dil = value;
                    OnPropertyChanged(nameof(Dil));
                }
            }
        }

        public bool Fiyat
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

        public List<Kitap> SampleData { get; }

        public bool Yıl
        {
            get => yıl;

            set
            {
                if (yıl != value)
                {
                    yıl = value;
                    OnPropertyChanged(nameof(Yıl));
                }
            }
        }

        private bool dil;

        private bool fiyat;

        private bool yıl;
    }
}