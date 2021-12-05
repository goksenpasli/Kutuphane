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
                    KitapAlanlarListesi().CreateXlsReport(ExeFolder + filepath);
                }
            }, parameter => true);

            KitapTutanakRaporu = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\TUTANAK.docx";
                if (File.Exists(ExeFolder + filepath) && parameter is object[] data && data[0] is Kişi kişi && data[1] is İşlem işlem)
                {
                    Data kişidata = GenerateData(kişi, işlem);
                    kişidata.CreateDocReport(ExeFolder + filepath);
                }
            }, parameter => true);

            KitapTutanakEkle = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\TUTANAK.docx";
                if (File.Exists(ExeFolder + filepath) && parameter is object[] data && data[0] is Kişi kişi && data[1] is İşlem işlem)
                {
                    Data kişidata = GenerateData(kişi, işlem);
                    string tempfilepath = kişidata.ProcessReportDoc(ExeFolder + filepath, out _, out _);
                    string filename = Path.GetFileName(tempfilepath);
                    File.Copy(tempfilepath, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                    kişi.TutanakYolu.Add(filename);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => true);
        }

        public ICommand KitapAlanlarRaporu { get; }

        public ICommand KitapListesiRaporu { get; }

        public ICommand KitapTutanakEkle { get; }

        public ICommand KitapTutanakRaporu { get; }

        internal class Data : Kişi
        {
            public double Ceza { get; internal set; }

            public double Gün { get; internal set; }

            public string KitapAdı { get; internal set; }

            public DateTime Tarih { get; internal set; }
        }

        private static string ExeFolder { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        private static Data GenerateData(Kişi kişi, İşlem işlem)
        {
            return new()
            {
                Ad = $"{kişi.Ad} {kişi.Soyad}",
                KitapAdı = işlem.SeçiliKitap.Ad,
                Gün = işlem.KitapGün,
                Tarih = işlem.GeriGetirmeTarihi,
                Ceza = Properties.Settings.Default.GünlükGecikmeBedeli
            };
        }

        private static ObservableCollection<Data> KitapAlanlarListesi()
        {
            ObservableCollection<Data> data = new();
            ObservableCollection<Kitap> Kitaplar = ExtensionMethods.KitaplarıYükle();
            ObservableCollection<Kişi> Kişiler = ExtensionMethods.KişileriYükle();
            foreach ((Kişi kişi, Kitap kitap) in Kişiler.SelectMany(kişi => kişi.İşlem.SelectMany(işlem => Kitaplar.Where(kitap => kitap.Id == işlem.KitapId)).Select(kitap => (kişi, kitap))))
            {
                data.Add(new Data() { Ad = kişi.Ad, Soyad = kişi.Soyad, KitapAdı = kitap.Ad });
            }
            return data;
        }
    }
}