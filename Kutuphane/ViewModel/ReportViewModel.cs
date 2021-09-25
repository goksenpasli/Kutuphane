using Extensions;
using System.Diagnostics;
using System.IO;
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
        }

        public ICommand KitapListesiRaporu { get; }

        private string ExeFolder { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }
}