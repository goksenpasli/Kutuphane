using Extensions;
using Kutuphane.Model;
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
                    ExtensionMethods.KitaplarıYükle().CreateReport(ExeFolder + filepath);
                }
            }, parameter => true);

            KitapAlanlarRaporu = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\KitapAlanlarRaporu.xlsx";
                var data = new ObservableCollection<Data>();
                if (File.Exists(ExeFolder + filepath))
                {
                    foreach (var (kitap, kişi) in ExtensionMethods.KitaplarıYükle().SelectMany(kitap => ExtensionMethods.KişileriYükle().SelectMany(kişi => kişi.İşlem.Where(işlem => işlem.KitapId == kitap.Id).Select(işlem => (kitap, kişi)))))
                    {
                        data.Add(new Data() { Ad = kişi.Ad, Soyad = kişi.Soyad, KitapAdı = kitap.Ad });
                    }

                    data.CreateReport(ExeFolder + filepath);
                }
            }, parameter => true);
        }

        public ICommand KitapAlanlarRaporu { get; }

        public ICommand KitapListesiRaporu { get; }

        private string ExeFolder { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        internal class Data : Kişi
        {
            public string KitapAdı { get; internal set; }
        }
    }
}