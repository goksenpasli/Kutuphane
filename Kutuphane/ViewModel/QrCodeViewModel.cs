using Extensions;
using Kutuphane.Model;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing;

namespace Kutuphane.ViewModel
{
    public class QrCodeViewModel : InpcBase
    {
        public QrCodeViewModel()
        {
            Barkod = new Barkod();

            KareKodYazdır = new RelayCommand<object>(parameter =>
            {
                PrintDialog printDlg = new();
                if (printDlg.ShowDialog() == true)
                {
                    printDlg.PrintVisual(parameter as Visual, "KareKod Yazdır.");
                }
            }, parameter => true);

            KareKodSakla = new RelayCommand<object>(parameter =>
            {
                if (parameter is WriteableBitmap writeableBitmap)
                {
                    SaveFileDialog saveFileDialog = new()
                    {
                        Title = "SAKLA",
                        Filter = "Resim Dosyaları (*.png)|*.png",
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var bytes = writeableBitmap.ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Png);
                        using FileStream imageFile = new(saveFileDialog.FileName, FileMode.Create);
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        bytes = null;
                    }
                }
            }, parameter => true);

            Barkod.PropertyChanged += QrCodeViewModel_PropertyChanged;
        }

        public Barkod Barkod { get; set; }

        public ICommand KareKodSakla { get; }

        public ICommand KareKodYazdır { get; }

        public WriteableBitmap GenerateBarCodeImage(BarcodeFormat format = BarcodeFormat.QR_CODE)
        {
            try
            {
                BarcodeWriter writer = new()
                {
                    Format = format,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = Barkod.QrHeight,
                        Width = Barkod.QrWidth,
                        Margin = 0
                    }
                };
                if (!string.IsNullOrWhiteSpace(Barkod.Metin))
                {
                    return writer.WriteAsWriteableBitmap(Barkod.Metin);
                }
                Barkod.BarkodError = "";
                return null;
            }
            catch (Exception ex)
            {
                Barkod.BarkodError = ex.Message;
                return null;
            }
        }

        private void QrCodeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "BarcodeFormat")
            {
                Barkod.BarkodImage = GenerateBarCodeImage(Barkod.BarcodeFormat);
            }
        }
    }
}