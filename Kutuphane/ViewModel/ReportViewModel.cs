using Extensions;
using Kutuphane.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            KitapListesiRaporu = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\KitapListesiRaporu.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    ExtensionMethods.KitaplarıYükle().CreateXlsReport(ExeFolder + filepath);
                }
            }, parameter => true);

            KitapAlanlarRaporu = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\KitapAlanlarRaporu.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    var data = new ObservableCollection<Data>();
                    foreach (var (kitap, kişi) in ExtensionMethods.KitaplarıYükle().SelectMany(kitap => ExtensionMethods.KişileriYükle().SelectMany(kişi => kişi.İşlem.Where(işlem => işlem.KitapId == kitap.Id).Select(işlem => (kitap, kişi)))))
                    {
                        data.Add(new Data() { Ad = kişi.Ad, Soyad = kişi.Soyad, KitapAdı = kitap.Ad });
                    }

                    data.CreateXlsReport(ExeFolder + filepath);
                }
            }, parameter => true);

            KitapTutanakRaporu = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\TUTANAK.docx";
                if (File.Exists(ExeFolder + filepath) && parameter is object[] data && data[0] is Kişi kişi && data[1] is İşlem işlem)
                {
                    var kişidata = new Data
                    {
                        Ad = kişi.Ad + " " + kişi.Soyad,
                        KitapAdı = işlem.SeçiliKitap.Ad,
                        Gün = işlem.KitapGün,
                        Tarih = işlem.GeriGetirmeTarihi,
                        Ceza = Properties.Settings.Default.GünlükGecikmeBedeli
                    };
                    kişidata.CreateDocReport(ExeFolder + filepath);
                }
            }, parameter => true);
        }

        public ICommand KitapAlanlarRaporu { get; }

        public ICommand KitapListesiRaporu { get; }

        public ICommand KitapTutanakRaporu { get; }

        private string ExeFolder { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        internal class Data : Kişi
        {
            public double Ceza { get; internal set; }

            public double Gün { get; internal set; }

            public string KitapAdı { get; internal set; }

            public DateTime Tarih { get; internal set; }
        }
    }
}