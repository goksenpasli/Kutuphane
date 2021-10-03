using NGS.Templater;
using System;
using System.Diagnostics;
using System.IO;

namespace Kutuphane.ViewModel
{
    public static class Reporting
    {
        public static void CreateDocReport(this object dbset, string docxfilepath)
        {
            using Stream stream = File.Open(docxfilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var dosyaismi = Path.GetTempPath() + Guid.NewGuid() + ".docx";
            using (FileStream output = new(dosyaismi, FileMode.Create))
            {
                using var document = Configuration.Factory.Open(stream, output, "docx");
                _ = document.Process(new { Context = dbset });
            }
            _ = Process.Start(dosyaismi);
        }

        public static void CreateXlsReport(this object dbset, string xlsxfilepath)
        {
            using Stream stream = File.Open(xlsxfilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var dosyaismi = Path.GetTempPath() + Guid.NewGuid() + ".xlsx";
            using (FileStream output = new(dosyaismi, FileMode.Create))
            {
                using var document = Configuration.Factory.Open(stream, output, "xlsx");
                _ = document.Process(new { Context = dbset });
            }
            _ = Process.Start(dosyaismi);
        }
    }
}