using Extensions;
using Kutuphane.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Kutuphane.ViewModel
{
    public class GecikenKitaplarSimpleViewModel : InpcBase
    {
        private ObservableCollection<Kişi> yaklaşanİşlemler;

        public ObservableCollection<Kişi> Yaklaşanİşlemler
        {
            get
            {
                if (File.Exists(MainViewModel.xmldatapath))
                {
                    yaklaşanİşlemler = new();
                    foreach (XElement kişi in XDocument.Load(MainViewModel.xmldatapath)?.Descendants("İşlem")?.Where(z => !(bool)z.Attribute("İşlemBitti") && ((DateTime)z.Attribute("GeriGetirmeTarihi")).AddDays(-Properties.Settings.Default.YaklaşanİşlemlerGünSayısı) < DateTime.Now)?.Select(z => z.Parent).Distinct())
                    {
                        yaklaşanİşlemler.Add(kişi.DeSerialize<Kişi>());
                    }
                    return yaklaşanİşlemler;
                }
                return null;
            }

            set
            {
                if (yaklaşanİşlemler != value)
                {
                    yaklaşanİşlemler = value;
                    OnPropertyChanged(nameof(Yaklaşanİşlemler));
                }
            }
        }
    }
}