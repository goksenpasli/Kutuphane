using Extensions;
using Kutuphane.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Kutuphane.ViewModel
{
    public class AppSettingsViewModel : InpcBase
    {
        private static readonly SpeechSynthesizer synthesizer = new() { Volume = 100 };

        static AppSettingsViewModel()
        {
            TtsDilleri = synthesizer.GetInstalledVoices().Select(z => z.VoiceInfo.Name);
        }

        public AppSettingsViewModel()
        {
            AyarlarıSıfırla = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Ayarlar varsayılana döndürülecek devam edilsin mi?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Settings.Default.Reset();
                }
            });

            TtsRegImport = new RelayCommand<object>(parameter =>
            {
                try
                {
                    string path = $"{Path.GetTempPath()}{Guid.NewGuid()}.reg";
                    File.WriteAllText(path, Resources.TtsReg);
                    _ = Process.Start("regedit.exe", path);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message);
                }
            }, parameter => true);

            KimlikArkaplanResimGüncelle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = $"{Guid.NewGuid()}{Path.GetExtension(openFileDialog.FileName)}";
                    File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{filename}");
                    Settings.Default.KimlikArkaPlanResim = filename;
                }
            }, parameter => true);

            MetinOku = new RelayCommand<object>(parameter =>
            {
                if (parameter is string metin)
                {
                    synthesizer.SelectVoice(Settings.Default.SeçiliTts);
                    _ = synthesizer.SpeakAsync(metin);
                }
            }, parameter => !string.IsNullOrEmpty(Settings.Default.SeçiliTts));
        }

        public static IEnumerable<string> TtsDilleri { get; }

        public ICommand AyarlarıSıfırla { get; }

        public ICommand KimlikArkaplanResimGüncelle { get; }

        public ICommand MetinOku { get; }

        public ICommand TtsRegImport { get; }

        public void OtomatikYedekle()
        {
            string yedekdosya = $"KütüphaneYedek{DateTime.Today.ToShortDateString()}.zip";
            DispatcherTimer timer = new(DispatcherPriority.SystemIdle) { Interval = TimeSpan.FromSeconds(60) };
            timer.Tick += (s, e) =>
            {
                if (IdleTimeFinder.IdleTimeSecond() > 600)
                {
                    ZipFile.CreateFromDirectory(Path.GetDirectoryName(MainViewModel.xmldatapath), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), yedekdosya), CompressionLevel.NoCompression, false);
                    for (int i = Settings.Default.EskiYedekGün; i > 7; i--)
                    {
                        string eskiyedekdosya = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"KütüphaneYedek{DateTime.Today.AddDays(-i).ToShortDateString()}.zip");
                        if (File.Exists(eskiyedekdosya))
                        {
                            File.Delete(eskiyedekdosya);
                        }
                    }

                    timer.Stop();
                }
            };

            if (File.Exists(MainViewModel.xmldatapath) && !File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), yedekdosya)))
            {
                timer.Start();
            }
        }
    }
}