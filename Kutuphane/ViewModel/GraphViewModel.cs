using Extensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;
using static Extensions.GraphControl;

namespace Kutuphane.ViewModel
{
    public enum Durum
    {
        Aylık = 0,

        Yıllık = 1,
    }

    public class GraphViewModel : InpcBase
    {
        private string seçiliVeri;

        private ObservableCollection<Chart> veriler;

        public GraphViewModel()
        {
            PropertyChanged += GraphViewModel_PropertyChanged;
        }

        public string SeçiliVeri
        {
            get => seçiliVeri;

            set
            {
                if (seçiliVeri != value)
                {
                    seçiliVeri = value;
                    OnPropertyChanged(nameof(SeçiliVeri));
                }
            }
        }

        public ObservableCollection<Chart> Veriler
        {
            get => veriler;

            set
            {
                if (veriler != value)
                {
                    veriler = value;
                    OnPropertyChanged(nameof(Veriler));
                }
            }
        }

        private void GraphViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "SeçiliVeri" && File.Exists(MainViewModel.xmldatapath))
            {
                Random rand = new();
                if (SeçiliVeri == nameof(Durum.Aylık))
                {
                    Veriler = new ObservableCollection<Chart>(XDocument.Load(MainViewModel.xmldatapath).Descendants("İşlem").Where(z => ((DateTime)z.Attribute("BaşlangıçTarihi")).Year == DateTime.Now.Year).
                    GroupBy(z => ((DateTime)z.Attribute("BaşlangıçTarihi")).Month).
                    Select(z => new Chart() { ChartBrush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256))), ChartValue = z.Count(), Description = ((DateTime)z.FirstOrDefault()?.Attribute("BaşlangıçTarihi")).ToString("MMMM") }));
                }
                if (SeçiliVeri == nameof(Durum.Yıllık))
                {
                    Veriler = new ObservableCollection<Chart>(XDocument.Load(MainViewModel.xmldatapath).Descendants("İşlem").
                    GroupBy(z => ((DateTime)z.Attribute("BaşlangıçTarihi")).Year).
                    Select(z => new Chart() { ChartBrush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256))), ChartValue = z.Count(), Description = ((DateTime)z.FirstOrDefault()?.Attribute("BaşlangıçTarihi")).ToString("yyyy") }));
                }
            }
        }
    }
}