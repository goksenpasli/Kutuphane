using Extensions;
using Kutuphane.Model;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class QrCodeViewModel : Barkod
    {
        private Barkod barkod;

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
                if (parameter is BitmapImage bitmapimage)
                {
                    SaveFileDialog saveFileDialog = new()
                    {
                        Title = "SAKLA",
                        Filter = "Resim Dosyaları (*.png)|*.png",
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        byte[] bytes = bitmapimage.ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Png);
                        using FileStream imageFile = new(saveFileDialog.FileName, FileMode.Create);
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        bytes = null;
                    }
                }
            }, parameter => true);

            Barkod.PropertyChanged += Barkod_PropertyChanged;
        }

        public Barkod Barkod
        {
            get => barkod;

            set
            {
                if (barkod != value)
                {
                    barkod = value;
                    OnPropertyChanged(nameof(Barkod));
                }
            }
        }

        public ICommand KareKodSakla { get; }

        public ICommand KareKodYazdır { get; }

        private void Barkod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "BarcodeFormat" or "PureBarcode")
            {
                Barkod.BarkodImage = Barkod.GenerateBarCodeImage(Barkod.BarcodeFormat, Barkod.PureBarcode);
            }
        }
    }
}