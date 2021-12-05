using Extensions;
using Kutuphane.Model;
using Kutuphane.Properties;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class QrCodeMultipleViewModel : InpcBase
    {
        public QrCodeMultipleViewModel()
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
            Settings.Default.PropertyChanged += Default_PropertyChanged;
            PropertyChanged += QrCodeMultipleViewModel_PropertyChanged;
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

        public ObservableCollection<BitmapSource> BarkodResimler
        {
            get => barkodResimler;

            set
            {
                if (barkodResimler != value)
                {
                    barkodResimler = value;
                    OnPropertyChanged(nameof(BarkodResimler));
                }
            }
        }

        public int Boy
        {
            get => boy;

            set
            {
                if (boy != value)
                {
                    boy = value;
                    OnPropertyChanged(nameof(Boy));
                }
            }
        }

        public int En
        {
            get => en;

            set
            {
                if (en != value)
                {
                    en = value;
                    OnPropertyChanged(nameof(En));
                }
            }
        }

        public ICommand KareKodSakla { get; }

        public ICommand KareKodYazdır { get; }

        public bool KitapRenkKullan
        {
            get => kitapRenkKullan;

            set
            {
                if (kitapRenkKullan != value)
                {
                    kitapRenkKullan = value;
                    OnPropertyChanged(nameof(KitapRenkKullan));
                }
            }
        }

        public bool PureBarcode
        {
            get => pureBarcode;

            set
            {
                if (pureBarcode != value)
                {
                    pureBarcode = value;
                    OnPropertyChanged(nameof(PureBarcode));
                }
            }
        }

        private Barkod barkod;

        private ObservableCollection<BitmapSource> barkodResimler = new();

        private int boy = 4;

        private int en = 4;

        private bool kitapRenkKullan;

        private bool pureBarcode;

        private void Barkod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "PureBarcode")
            {
                Barkod.BarkodImage = Barkod.GenerateBarCodeImage(Settings.Default.SeçiliBarkod, PureBarcode);
            }
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "SeçiliBarkod")
            {
                GenerateBarcode();
            }
        }

        private void GenerateBarcode()
        {
            BarkodResimler.Clear();
            Barkod.BarkodImage = Barkod.GenerateBarCodeImage(Settings.Default.SeçiliBarkod, PureBarcode);
            for (int i = 0; i < En * Boy; i++)
            {
                BarkodResimler.Add(Barkod.BarkodImage);
            }
        }

        private void QrCodeMultipleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "En" or "Boy" or "PureBarcode")
            {
                GenerateBarcode();
            }
        }
    }
}