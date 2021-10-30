using Extensions;
using Kutuphane.Model;
using Kutuphane.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class MainViewModel : InpcBase
    {
        public static readonly string xmldatapath = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath) + @"\Data.xml";

        private static readonly SpeechSynthesizer synthesizer = new() { Volume = 100 };

        private InpcBase currentView;

        public MainViewModel()
        {
            Kütüphane = new Kütüphane
            {
                Kitaplar = ExtensionMethods.KitaplarıYükle(),
                Yazarlar = ExtensionMethods.YazarlarıYükle(),
                KitapTürleri = ExtensionMethods.TürleriYükle(),
                Dolaplar = ExtensionMethods.DolaplarıYükle(),
                Kişiler = ExtensionMethods.KişileriYükle(),
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

            QrCodeViewModel = new QrCodeViewModel();

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

            MetinOku = new RelayCommand<object>(parameter =>
            {
                if (parameter is string metin)
                {
                    synthesizer.SelectVoice(Settings.Default.SeçiliTts);
                    _ = synthesizer.SpeakAsync(metin);
                }
            }, parameter => !string.IsNullOrEmpty(Settings.Default.SeçiliTts));

            KimlikArkaplanResimGüncelle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(xmldatapath)}\\{filename}");
                    Settings.Default.KimlikArkaPlanResim = filename;
                }
            }, parameter => true);

            if (Settings.Default.KişiGirişEkranıVarsayılan)
            {
                CurrentView = KişiGirişViewModel;
            }

            WebAdreseGit = new RelayCommand<object>(parameter => Process.Start(parameter as string), parameter => true);

            TtsRegImport = new RelayCommand<object>(parameter =>
            {
                try
                {
                    var path = $"{Path.GetTempPath()}\\{Guid.NewGuid() + ".reg"}";
                    File.WriteAllLines(path, (parameter as StringCollection).Cast<string>());
                    _ = Process.Start("regedit.exe", "/s" + path);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message);
                }
            }, parameter => true);

            AyarlarıSıfırla = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Ayarlar varsayılana döndürülecek devam edilsin mi?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Settings.Default.Reset();
                }
            });

            CloseView = new RelayCommand<object>(parameter => CurrentView = null);

            Settings.Default.PropertyChanged += (s, e) => Settings.Default.Save();
        }

        public static ICommand AyarlarıSıfırla { get; set; }

        public static ICommand DatabaseSave { get; set; }

        public static ICommand KitapCezaEkranı { get; set; }

        public static ICommand KitapKontrolEkranı { get; set; }

        public static ICommand UygulamadanÇık { get; set; }

        public static ICommand VeritabanınıAç { get; set; }

        public ICommand CloseView { get; }

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

        public ICommand GecikenKitaplarEkranı { get; }

        public GecikenKitaplarSimpleViewModel GecikenKitaplarSimpleViewModel { get; set; }

        public GecikenKitaplarViewModel GecikenKitaplarViewModel { get; set; }

        public ICommand KimlikArkaplanResimGüncelle { get; }

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

        public ICommand MetinOku { get; }

        public QrCodeMultipleViewModel QrCodeMultipleViewModel { get; set; }

        public QrCodeViewModel QrCodeViewModel { get; set; }

        public ReportViewModel ReportViewModel { get; set; }

        public IEnumerable<string> TtsDilleri { get; set; } = synthesizer.GetInstalledVoices().Select(z => z.VoiceInfo.Name);

        public ICommand TtsRegImport { get; }

        public ICommand WebAdreseGit { get; }
    }
}