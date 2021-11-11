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
            _ = ProcessReportDoc(dbset, docxfilepath, out _, out string dosyaismi);
            _ = Process.Start(dosyaismi);
        }

        public static void CreateXlsReport(this object dbset, string xlsxfilepath)
        {
            _ = ProcessReportXls(dbset, xlsxfilepath, out _, out string dosyaismi);
            _ = Process.Start(dosyaismi);
        }

        public static string ProcessReportDoc(this object dbset, string docxfilepath, out Stream stream, out string dosyaismi)
        {
            stream = File.Open(docxfilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            dosyaismi = Path.GetTempPath() + Guid.NewGuid() + ".docx";
            using FileStream output = new(dosyaismi, FileMode.Create);
            using ITemplateDocument document = Configuration.Factory.Open(stream, output, "docx");
            _ = document.Process(new { Context = dbset });
            return dosyaismi;
        }

        private static string ProcessReportXls(this object dbset, string xlsxfilepath, out Stream stream, out string dosyaismi)
        {
            stream = File.Open(xlsxfilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            dosyaismi = Path.GetTempPath() + Guid.NewGuid() + ".xlsx";
            using FileStream output = new(dosyaismi, FileMode.Create);
            using ITemplateDocument document = Configuration.Factory.Open(stream, output, "xlsx");
            _ = document.Process(new { Context = dbset });
            return dosyaismi;
        }
    }
}