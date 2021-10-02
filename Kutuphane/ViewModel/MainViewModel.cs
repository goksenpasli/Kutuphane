using Extensions;
using Kutuphane.Model;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class MainViewModel : InpcBase
    {
        public static readonly string xmldatapath = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath) + @"\Data.xml";

        private object currentView;

        public MainViewModel()
        {
            Kütüphane = new Kütüphane
            {
                Kitaplar = ExtensionMethods.KitaplarıYükle(),
                Yazarlar = ExtensionMethods.YazarlarıYükle(),
                KitapTürleri = ExtensionMethods.TürleriYükle(),
                Dolaplar = ExtensionMethods.DolaplarıYükle(),
                Kişiler = ExtensionMethods.KişileriYükle(),
                İşlemler = ExtensionMethods.İşlemleriYükle()
            };

            KitapGirişViewModel = new KitapGirişViewModel();

            KitapSilViewModel = new KitapSilViewModel();

            KitapCezaDurumViewModel = new KitapCezaDurumViewModel();

            KitapVerViewModel = new KitapVerViewModel();

            KitapGüncelleViewModel = new KitapGüncelleViewModel();

            DolapGirişViewModel = new DolapGirişViewModel();

            ReportViewModel = new ReportViewModel();

            KişiGirişViewModel = new KişiGirişViewModel();

            KitapKontrolViewModel = new KitapKontrolViewModel();

            KitapGeriAlViewModel = new KitapGeriAlViewModel();

            GecikenKitaplarViewModel = new GecikenKitaplarViewModel();

            DolapDüzenViewModel = new DolapDüzenViewModel();

            QrCodeViewModel = new QrCodeViewModel();

            QrCodeMultipleViewModel = new QrCodeMultipleViewModel();

            KitapGirişiEkranı = new RelayCommand<object>(parameter => CurrentView = KitapGirişViewModel, parameter => CurrentView != KitapGirişViewModel);

            KitapSilEkranı = new RelayCommand<object>(parameter => CurrentView = KitapSilViewModel, parameter => CurrentView != KitapSilViewModel);

            GecikenKitaplarEkranı = new RelayCommand<object>(parameter => CurrentView = GecikenKitaplarViewModel, parameter => CurrentView != GecikenKitaplarViewModel);

            KitapVerEkranı = new RelayCommand<object>(parameter => CurrentView = KitapVerViewModel, parameter => CurrentView != KitapVerViewModel);

            KitapCezaEkranı = new RelayCommand<object>(parameter => CurrentView = KitapCezaDurumViewModel, parameter => CurrentView != KitapCezaDurumViewModel);

            KitapGeriAlEkranı = new RelayCommand<object>(parameter => CurrentView = KitapGeriAlViewModel, parameter => CurrentView != KitapGeriAlViewModel);

            KitapKontrolEkranı = new RelayCommand<object>(parameter => CurrentView = KitapKontrolViewModel, parameter => CurrentView != KitapKontrolViewModel);

            KitapGüncelleEkranı = new RelayCommand<object>(parameter => CurrentView = KitapGüncelleViewModel, parameter => CurrentView != KitapGüncelleViewModel);

            DolapGirişiEkranı = new RelayCommand<object>(parameter => CurrentView = DolapGirişViewModel, parameter => CurrentView != DolapGirişViewModel);

            KişiGirişiEkranı = new RelayCommand<object>(parameter =>
            {
                KişiGirişViewModel.Kişi.SonKaydedilenKişi = null;
                CurrentView = KişiGirişViewModel;
            }, parameter => CurrentView != KişiGirişViewModel);

            DolapDüzeniEkranı = new RelayCommand<object>(parameter => CurrentView = DolapDüzenViewModel, parameter => CurrentView != DolapDüzenViewModel);

            DatabaseSave = new RelayCommand<object>(parameter => Kütüphane.Serialize());

            UygulamadanÇık = new RelayCommand<object>(parameter => App.Current.MainWindow.Close());

            VeritabanınıAç = new RelayCommand<object>(parameter =>
            {
                if (File.Exists(xmldatapath))
                {
                    _ = Process.Start(xmldatapath);
                }
            });

            if (Properties.Settings.Default.KişiGirişEkranıVarsayılan)
            {
                CurrentView = KişiGirişViewModel;
            }

            AyarlarıSıfırla = new RelayCommand<object>(parameter => Properties.Settings.Default.Reset());

            Properties.Settings.Default.PropertyChanged += (s, e) => Properties.Settings.Default.Save();
        }

        public static ICommand AyarlarıSıfırla { get; set; }

        public static ICommand DatabaseSave { get; set; }

        public static ICommand KitapCezaEkranı { get; set; }

        public static ICommand KitapKontrolEkranı { get; set; }

        public static ICommand UygulamadanÇık { get; set; }

        public static ICommand VeritabanınıAç { get; set; }

        public object CurrentView
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

        public ICommand GecikenKitaplarEkranı { get; }

        public GecikenKitaplarViewModel GecikenKitaplarViewModel { get; set; }

        public ICommand KişiGirişiEkranı { get; }

        public KişiGirişViewModel KişiGirişViewModel { get; set; }

        public KitapCezaDurumViewModel KitapCezaDurumViewModel { get; set; }

        public ICommand KitapGeriAlEkranı { get; }

        public KitapGeriAlViewModel KitapGeriAlViewModel { get; set; }

        public ICommand KitapGirişiEkranı { get; }

        public KitapGirişViewModel KitapGirişViewModel { get; set; }

        public ICommand KitapGüncelleEkranı { get; }

        public KitapGüncelleViewModel KitapGüncelleViewModel { get; set; }

        public KitapKontrolViewModel KitapKontrolViewModel { get; set; }

        public ICommand KitapSilEkranı { get; }

        public KitapSilViewModel KitapSilViewModel { get; set; }

        public ICommand KitapVerEkranı { get; }

        public KitapVerViewModel KitapVerViewModel { get; set; }

        public Kütüphane Kütüphane { get; set; }

        public QrCodeMultipleViewModel QrCodeMultipleViewModel { get; set; }

        public QrCodeViewModel QrCodeViewModel { get; set; }

        public ReportViewModel ReportViewModel { get; set; }
    }
}