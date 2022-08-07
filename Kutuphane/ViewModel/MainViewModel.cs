using Extensions;
using Kutuphane.Model;
using Kutuphane.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TwainControl;

namespace Kutuphane.ViewModel
{
    public class MainViewModel : InpcBase
    {
        public static readonly string xmldatapath = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath) + @"\Data.xml";

        static MainViewModel()
        {
            Yıllar = Enumerable.Range(DateTime.Now.Year - 50, 100);
        }

        public MainViewModel()
        {
            TranslationSource.Instance.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");

            Kütüphane = new Kütüphane
            {
                Kitaplar = ExtensionMethods.KitaplarıYükle(),
                Yazarlar = ExtensionMethods.YazarlarıYükle(),
                KitapTürleri = ExtensionMethods.TürleriYükle(),
                Dolaplar = ExtensionMethods.DolaplarıYükle(),
                Kişiler = ExtensionMethods.KişileriYükle()
            };

            KitapGirişViewModel = new KitapGirişViewModel();

            KitapSilViewModel = new KitapSilViewModel();

            KitapVerViewModel = new KitapVerViewModel();

            KitapGüncelleViewModel = new KitapGüncelleViewModel();

            DolapGirişViewModel = new DolapGirişViewModel();

            ReportViewModel = new ReportViewModel();

            KişiGirişViewModel = new KişiGirişViewModel();

            KişiGüncelleViewModel = new KişiGüncelleViewModel();

            KitapKontrolViewModel = new KitapKontrolViewModel();

            KitapTakvimViewModel = new KitapTakvimViewModel();

            KitapGeriAlViewModel = new KitapGeriAlViewModel();

            GecikenKitaplarViewModel = new GecikenKitaplarViewModel();

            GecikenKitaplarSimpleViewModel = new GecikenKitaplarSimpleViewModel();

            DolapDüzenViewModel = new DolapDüzenViewModel();

            AppSettingsViewModel = new AppSettingsViewModel();

            QrCodeMultipleViewModel = new QrCodeMultipleViewModel();

            KitapGirişiEkranı = new RelayCommand<object>(parameter => CurrentView = KitapGirişViewModel, parameter => CurrentView != KitapGirişViewModel);

            KitapSilEkranı = new RelayCommand<object>(parameter => CurrentView = KitapSilViewModel, parameter => CurrentView != KitapSilViewModel);

            GecikenKitaplarEkranı = new RelayCommand<object>(parameter => CurrentView = GecikenKitaplarViewModel, parameter => CurrentView != GecikenKitaplarViewModel);

            KitapVerEkranı = new RelayCommand<object>(parameter => CurrentView = KitapVerViewModel, parameter => CurrentView != KitapVerViewModel);

            KitapTakvimEkranı = new RelayCommand<object>(parameter => CurrentView = KitapTakvimViewModel, parameter => CurrentView != KitapTakvimViewModel);

            KişiGüncelleEkranı = new RelayCommand<object>(parameter => CurrentView = KişiGüncelleViewModel, parameter => CurrentView != KişiGüncelleViewModel);

            KitapGeriAlEkranı = new RelayCommand<object>(parameter => CurrentView = KitapGeriAlViewModel, parameter => CurrentView != KitapGeriAlViewModel);

            KitapKontrolEkranı = new RelayCommand<object>(parameter => CurrentView = KitapKontrolViewModel, parameter => CurrentView != KitapKontrolViewModel);

            KitapGüncelleEkranı = new RelayCommand<object>(parameter =>
            {
                KitapGüncelleViewModel.KişiKitapKonumArama = 4;
                CurrentView = KitapGüncelleViewModel;
            }, parameter => CurrentView != KitapGüncelleViewModel);

            DolapGirişiEkranı = new RelayCommand<object>(parameter => CurrentView = DolapGirişViewModel, parameter => CurrentView != DolapGirişViewModel);

            KişiGirişiEkranı = new RelayCommand<object>(parameter =>
            {
                KişiGirişViewModel.Kişi.SonKaydedilenKişi = null;
                CurrentView = KişiGirişViewModel;
            }, parameter => CurrentView != KişiGirişViewModel);

            DolapDüzeniEkranı = new RelayCommand<object>(parameter => CurrentView = DolapDüzenViewModel, parameter => CurrentView != DolapDüzenViewModel);

            DatabaseSave = new RelayCommand<object>(parameter => Kütüphane.Serialize());

            UygulamadanÇık = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Uygulamadan çıkmak istiyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Application.Current.MainWindow.Close();
                }
            });

            VeritabanınıAç = new RelayCommand<object>(parameter =>
            {
                if (File.Exists(xmldatapath) && MessageBox.Show("Veritabanı dosyasını düzenlemek istiyor musun? Dikkat yanlış düzenleme programın açılmamasına neden olabilir. Devam edilsin mi?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    _ = Process.Start(xmldatapath);
                }
            });

            DefaultScreen = new Dictionary<int, InpcBase>
            {
                [0] = KişiGirişViewModel,
                [1] = KitapVerViewModel,
                [2] = KitapKontrolViewModel,
                [3] = KişiGüncelleViewModel,
                [4] = KitapGeriAlViewModel,
                [5] = KitapTakvimViewModel,
            };

            if (Settings.Default.KişiGirişEkranıVarsayılan)
            {
                Fold = 0;
                CurrentView = DefaultScreen[Settings.Default.VarsayılanEkran];
            }

            if (Settings.Default.OtomatikYedek)
            {
                AppSettingsViewModel.OtomatikYedekle();
            }

            WebAdreseGit = new RelayCommand<object>(parameter => Process.Start(parameter as string), parameter => true);

            CloseView = new RelayCommand<object>(parameter => CurrentView = null);

            ArşivAç = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Zip Dosyaları (*.zip)|*.zip" };
                if (openFileDialog.ShowDialog() == true)
                {
                    ArşivYolu = openFileDialog.FileName;
                }
            });

            Yedekle = new RelayCommand<object>(parameter =>
            {
                SaveFileDialog saveFileDialog = new()
                {
                    Title = "SAKLA",
                    Filter = "Zip Dosyası (*.zip)|*.zip",
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }
                    if (Compress)
                    {
                        ZipFile.CreateFromDirectory(Path.GetDirectoryName(xmldatapath), saveFileDialog.FileName, CompressionLevel.Fastest, false);
                    }
                    else
                    {
                        ZipFile.CreateFromDirectory(Path.GetDirectoryName(xmldatapath), saveFileDialog.FileName, CompressionLevel.NoCompression, false);
                    }
                }
            }, parameter => File.Exists(xmldatapath));

            Settings.Default.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is "OtomatikYedek" && Settings.Default.OtomatikYedek)
                {
                    AppSettingsViewModel.OtomatikYedekle();
                }
                Settings.Default.Save();
            };

            PropertyChanged += MainViewModel_PropertyChanged;
        }

        public static ICommand DatabaseSave { get; set; }

        public static Dictionary<int, InpcBase> DefaultScreen { get; set; }

        public static ICommand KitapCezaEkranı { get; set; }

        public static ICommand KitapKontrolEkranı { get; set; }

        public static ICommand UygulamadanÇık { get; set; }

        public static ICommand VeritabanınıAç { get; set; }

        public static IEnumerable<int> Yıllar { get; }

        public AppSettingsViewModel AppSettingsViewModel { get; set; }

        public ICommand ArşivAç { get; }

        public string ArşivYolu
        {
            get => arşivYolu;

            set
            {
                if (arşivYolu != value)
                {
                    arşivYolu = value;
                    OnPropertyChanged(nameof(ArşivYolu));
                }
            }
        }

        public ICommand CloseView { get; }

        public bool Compress
        {
            get => compress;

            set
            {
                if (compress != value)
                {
                    compress = value;
                    OnPropertyChanged(nameof(Compress));
                }
            }
        }

        public InpcBase CurrentView
        {
            get => currentView;

            set
            {
                if (currentView != value)
                {
                    currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public ICommand DolapDüzeniEkranı { get; }

        public DolapDüzenViewModel DolapDüzenViewModel { get; set; }

        public ICommand DolapGirişiEkranı { get; }

        public DolapGirişViewModel DolapGirişViewModel { get; set; }

        public double Fold
        {
            get => fold;

            set
            {
                if (fold != value)
                {
                    fold = value;
                    OnPropertyChanged(nameof(Fold));
                }
            }
        }

        public ICommand GecikenKitaplarEkranı { get; }

        public GecikenKitaplarSimpleViewModel GecikenKitaplarSimpleViewModel { get; set; }

        public GecikenKitaplarViewModel GecikenKitaplarViewModel { get; set; }

        public ICommand KişiGirişiEkranı { get; }

        public KişiGirişViewModel KişiGirişViewModel { get; set; }

        public ICommand KişiGüncelleEkranı { get; }

        public KişiGüncelleViewModel KişiGüncelleViewModel { get; set; }

        public ICommand KitapGeriAlEkranı { get; }

        public KitapGeriAlViewModel KitapGeriAlViewModel { get; set; }

        public ICommand KitapGirişiEkranı { get; }

        public KitapGirişViewModel KitapGirişViewModel { get; set; }

        public ICommand KitapGüncelleEkranı { get; }

        public KitapGüncelleViewModel KitapGüncelleViewModel { get; set; }

        public KitapKontrolViewModel KitapKontrolViewModel { get; set; }

        public ICommand KitapSilEkranı { get; }

        public KitapSilViewModel KitapSilViewModel { get; set; }

        public ICommand KitapTakvimEkranı { get; }

        public KitapTakvimViewModel KitapTakvimViewModel { get; set; }

        public ICommand KitapVerEkranı { get; }

        public KitapVerViewModel KitapVerViewModel { get; set; }

        public Kütüphane Kütüphane { get; set; }

        public QrCodeMultipleViewModel QrCodeMultipleViewModel { get; set; }

        public ReportViewModel ReportViewModel { get; set; }

        public ICommand WebAdreseGit { get; }

        public ICommand Yedekle { get; }

        private string arşivYolu;

        private bool compress;

        private InpcBase currentView;

        private double fold = 0.5;

        private DispatcherTimer timer;

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "CurrentView")
            {
                timer = new(DispatcherPriority.Normal) { Interval = TimeSpan.FromMilliseconds(15) };
                Fold = 0.5;
                timer.Tick += OnTick;
                timer.Start();
            }

            void OnTick(object sender, EventArgs e)
            {
                Fold -= 0.01;
                if (Fold <= 0)
                {
                    Fold = 0;
                    timer.Stop();
                    timer.Tick -= OnTick;
                }
            }
        }
    }
}