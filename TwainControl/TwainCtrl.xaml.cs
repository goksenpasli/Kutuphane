using Extensions;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TwainWpf;
using TwainWpf.Wpf;

namespace TwainControl
{
    public partial class TwainCtrl : UserControl, INotifyPropertyChanged, IDisposable
    {
        public TwainCtrl()
        {
            InitializeComponent();
            DataContext = this;

            ScanImage = new RelayCommand<object>(parameter =>
            {
                ArayüzEtkin = false;
                _settings = new ScanSettings
                {
                    UseDocumentFeeder = Adf,
                    ShowTwainUi = ShowUi,
                    ShowProgressIndicatorUi = ShowProgress,
                    UseDuplex = Duplex,
                    ShouldTransferAllPages = true,
                    Resolution = new ResolutionSettings { ColourSetting = Bw ?? false ? ColourSetting.BlackAndWhite : ColourSetting.Colour, Dpi = (int)Properties.Settings.Default.Çözünürlük },
                    Rotation = new RotationSettings { AutomaticDeskew = Deskew, AutomaticRotate = AutoRotate, AutomaticBorderDetection = BorderDetect }
                };
                if (Tarayıcılar.Count > 0)
                {
                    twain.SelectSource(SeçiliTarayıcı);
                    twain.StartScanning(_settings);
                }
            }, parameter => !Environment.Is64BitProcess);

            ResimSil = new RelayCommand<object>(parameter => Resimler?.Remove(parameter as BitmapFrame), parameter => true);

            Kaydet = new RelayCommand<object>(parameter =>
            {
                if (parameter is BitmapFrame resim)
                {
                    SaveFileDialog saveFileDialog = new() { Filter = "Tif Resmi (*.tif)|*.tif|Jpg Resmi(*.jpg)|*.jpg" };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        switch (saveFileDialog.FilterIndex)
                        {
                            case 1:
                                switch (Bw)
                                {
                                    case true:
                                        File.WriteAllBytes(saveFileDialog.FileName, resim.ToTiffJpegByteArray(ExtensionMethods.Format.Tiff));
                                        break;

                                    case null:
                                    case false:
                                        File.WriteAllBytes(saveFileDialog.FileName, resim.ToTiffJpegByteArray(ExtensionMethods.Format.TiffRenkli));
                                        break;
                                }
                                break;

                            case 2:
                                File.WriteAllBytes(saveFileDialog.FileName, resim.ToTiffJpegByteArray(ExtensionMethods.Format.Jpg));
                                break;
                        }
                    }
                }
            }, parameter => true);

            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Adf
        {
            get => adf;

            set
            {
                if (adf != value)
                {
                    adf = value;
                    OnPropertyChanged(nameof(Adf));
                }
            }
        }

        public bool ArayüzEtkin
        {
            get => arayüzetkin;

            set
            {
                if (arayüzetkin != value)
                {
                    arayüzetkin = value;
                    OnPropertyChanged(nameof(ArayüzEtkin));
                }
            }
        }

        public bool AutoRotate
        {
            get => autoRotate;

            set
            {
                if (autoRotate != value)
                {
                    autoRotate = value;
                    OnPropertyChanged(nameof(AutoRotate));
                }
            }
        }

        public bool BorderDetect
        {
            get => borderDetect;

            set
            {
                if (borderDetect != value)
                {
                    borderDetect = value;
                    OnPropertyChanged(nameof(BorderDetect));
                }
            }
        }

        public bool? Bw
        {
            get => bw;

            set
            {
                if (bw != value)
                {
                    bw = value;
                    OnPropertyChanged(nameof(Bw));
                }
            }
        }

        public bool Deskew
        {
            get => deskew;

            set
            {
                if (deskew != value)
                {
                    deskew = value;
                    OnPropertyChanged(nameof(Deskew));
                }
            }
        }

        public bool Duplex
        {
            get => duplex;

            set
            {
                if (duplex != value)
                {
                    duplex = value;
                    OnPropertyChanged(nameof(Duplex));
                }
            }
        }

        public double Eşik
        {
            get => eşik;

            set
            {
                if (eşik != value)
                {
                    eşik = value;
                    OnPropertyChanged(nameof(Eşik));
                }
            }
        }

        public ICommand Kaydet { get; }

        public ObservableCollection<BitmapFrame> Resimler
        {
            get => resimler;

            set
            {
                if (resimler != value)
                {
                    resimler = value;
                    OnPropertyChanged(nameof(Resimler));
                }
            }
        }

        public ICommand ResimSil { get; }

        public ICommand ScanImage { get; }

        public ImageSource SeçiliResim
        {
            get => seçiliResim;

            set
            {
                if (seçiliResim != value)
                {
                    seçiliResim = value;
                    OnPropertyChanged(nameof(SeçiliResim));
                }
            }
        }

        public IList SeçiliResimler
        {
            get => seçiliresimler;

            set
            {
                if (seçiliresimler != value)
                {
                    seçiliresimler = value;
                    OnPropertyChanged(nameof(SeçiliResimler));
                }
            }
        }

        public string SeçiliTarayıcı
        {
            get => seçiliTarayıcı;

            set
            {
                if (seçiliTarayıcı != value)
                {
                    seçiliTarayıcı = value;
                    OnPropertyChanged(nameof(SeçiliTarayıcı));
                }
            }
        }

        public bool SeperateSave
        {
            get => seperateSave;

            set
            {
                if (seperateSave != value)
                {
                    seperateSave = value;
                    OnPropertyChanged(nameof(SeperateSave));
                }
            }
        }

        public bool ShowProgress
        {
            get => showProgress;

            set
            {
                if (showProgress != value)
                {
                    showProgress = value;
                    OnPropertyChanged(nameof(ShowProgress));
                }
            }
        }

        public bool ShowUi
        {
            get => showUi;

            set
            {
                if (showUi != value)
                {
                    showUi = value;
                    OnPropertyChanged(nameof(ShowUi));
                }
            }
        }

        public IList<string> Tarayıcılar
        {
            get => tarayıcılar;

            set
            {
                if (tarayıcılar != value)
                {
                    tarayıcılar = value;
                    OnPropertyChanged(nameof(Tarayıcılar));
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Resimler = null;
                    twain = null;
                }

                disposedValue = true;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ScanSettings _settings;

        private bool adf;

        private bool arayüzetkin = true;

        private bool autoRotate;

        private bool borderDetect;

        private bool? bw = false;

        private bool deskew;

        private bool disposedValue;

        private bool duplex;

        private double eşik = 160d;

        private ObservableCollection<BitmapFrame> resimler = new();

        private ImageSource seçiliResim;

        private IList seçiliresimler = new ObservableCollection<BitmapFrame>();

        private string seçiliTarayıcı;

        private bool seperateSave;

        private bool showProgress;

        private bool showUi;

        private IList<string> tarayıcılar;

        private Twain twain;

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                twain = new Twain(new WindowMessageHook(Window.GetWindow(Parent)));
                Tarayıcılar = twain.SourceNames;
                if (twain.SourceNames?.Count > 0)
                {
                    SeçiliTarayıcı = twain.SourceNames[0];
                }

                twain.TransferImage += (s, args) =>
                {
                    if (args.Image != null)
                    {
                        using (System.Drawing.Bitmap bmp = args.Image)
                        {
                            BitmapImage evrak = null;
                            switch (Bw)
                            {
                                case true:
                                    evrak = bmp.ConvertBlackAndWhite((int)Eşik).ToBitmapImage(ImageFormat.Tiff);
                                    break;

                                case null:
                                    evrak = bmp.ConvertBlackAndWhite((int)Eşik, true).ToBitmapImage(ImageFormat.Jpeg);
                                    break;

                                case false:
                                    evrak = bmp.ToBitmapImage(ImageFormat.Jpeg);
                                    break;
                            }

                            evrak.Freeze();
                            BitmapSource önizleme = evrak.Resize(42, 59);
                            önizleme.Freeze();
                            BitmapFrame bitmapFrame = BitmapFrame.Create(evrak, önizleme);
                            bitmapFrame.Freeze();
                            Resimler.Add(bitmapFrame);
                            if (SeperateSave && Bw == true)
                            {
                                File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).SetUniqueFile($"{DateTime.Now.ToShortDateString()}Tarama", "tif"), evrak.ToTiffJpegByteArray(ExtensionMethods.Format.Tiff));
                            }

                            if (SeperateSave && Bw == false)
                            {
                                File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).SetUniqueFile($"{DateTime.Now.ToShortDateString()}Tarama", "jpg"), evrak.ToTiffJpegByteArray(ExtensionMethods.Format.Jpg));
                            }

                            evrak = null;
                            bitmapFrame = null;
                            önizleme = null;
                        }
                        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                    }
                };
                twain.ScanningComplete += delegate { ArayüzEtkin = true; };
            }
            catch (Exception)
            {
                ArayüzEtkin = false;
            }
        }
    }
}