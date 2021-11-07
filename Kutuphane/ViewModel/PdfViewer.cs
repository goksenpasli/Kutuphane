using Extensions;
using Freeware;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class PdfViewer : ImageViewer, IDisposable
    {
        public static readonly DependencyProperty DpiProperty = DependencyProperty.Register("Dpi", typeof(int), typeof(PdfViewer), new PropertyMetadata(150, DpiChanged));

        private static FileStream pdf;

        private bool disposedValue;

        private int toplamSayfa;

        public PdfViewer()
        {
            OpenFileDialog openFileDialog = null;
            DosyaAç = new RelayCommand<object>(parameter =>
            {
                openFileDialog = new() { Multiselect = false, Filter = "Pdf Dosyaları (*.pdf)|*.pdf" };
                if (openFileDialog.ShowDialog() == true)
                {
                    pdf = new FileStream(openFileDialog.FileName, FileMode.Open);
                    ToplamSayfa = Pdf2Png.ConvertAllPages(pdf, 0).Count;
                    Pages = Enumerable.Range(1, ToplamSayfa);
                    Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, Dpi));
                    if (ToplamSayfa > 1)
                    {
                        TifNavigasyonButtonEtkin = Visibility.Visible;
                    }
                }
            });

            ViewerBack = new RelayCommand<object>(parameter =>
            {
                Sayfa--;
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, Dpi));
            }, parameter => Source is not null && Sayfa > 1 && Sayfa <= ToplamSayfa);

            ViewerNext = new RelayCommand<object>(parameter =>
            {
                Sayfa++;
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, Dpi));
            }, parameter => Source is not null && Sayfa >= 1 && Sayfa < ToplamSayfa);

            PropertyChanged += PdfViewer_PropertyChanged;
        }

        public new ICommand DosyaAç { get; }

        public int Dpi
        {
            get => (int)GetValue(DpiProperty);
            set => SetValue(DpiProperty, value);
        }

        public int[] DpiList { get; set; } = new int[] { 96, 150, 225, 300, 600 };

        public int ToplamSayfa
        {
            get => toplamSayfa;

            set
            {
                if (toplamSayfa != value)
                {
                    toplamSayfa = value;
                    OnPropertyChanged(nameof(ToplamSayfa));
                }
            }
        }

        public new ICommand ViewerBack { get; }

        public new ICommand ViewerNext { get; }

        public static BitmapSource BitmapSourceFromByteArray(byte[] buffer)
        {
            if (buffer != null)
            {
                var bitmap = new BitmapImage();
                using var stream = new MemoryStream(buffer);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            return null;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    pdf = null;
                }
                disposedValue = true;
            }
        }

        private static void DpiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PdfViewer pdfViewer)
            {
                if (pdf is null)
                {
                    pdf = new FileStream($"{Path.GetDirectoryName(MainViewModel.xmldatapath)}\\{pdfViewer.DataContext as string}", FileMode.Open);
                }
                pdfViewer.Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, pdfViewer.Sayfa, (int)e.NewValue));
            }
        }

        private void PdfViewer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Sayfa")
            {
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, Dpi));
            }
        }
    }
}