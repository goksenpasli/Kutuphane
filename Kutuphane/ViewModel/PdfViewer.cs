using Extensions;
using Freeware;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class PdfViewer : ImageViewer, IDisposable
    {
        private bool disposedValue;

        private FileStream pdf;

        private int pdfDpi = 150;

        public PdfViewer()
        {
            OpenFileDialog openFileDialog = null;
            DosyaAç = new RelayCommand<object>(parameter =>
            {
                openFileDialog = new() { Multiselect = false, Filter = "Pdf Dosyaları (*.pdf)|*.pdf" };
                if (openFileDialog.ShowDialog() == true)
                {
                    pdf = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, PdfDpi));
                    TifNavigasyonButtonEtkin = Visibility.Visible;
                }
            });

            ViewerBack = new RelayCommand<object>(parameter =>
            {
                Sayfa--;
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, PdfDpi));
                if (Source == null)
                {
                    MessageBox.Show("Öncesinde Sayfa Yok.");
                }
            }, parameter => true);

            ViewerNext = new RelayCommand<object>(parameter =>
            {
                Sayfa++;
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdf, Sayfa, PdfDpi));
                if (Source == null)
                {
                    MessageBox.Show("Sonrasında Sayfa Yok.");
                }
            }, parameter => true);
        }

        public new ICommand DosyaAç { get; }

        public int PdfDpi
        {
            get => pdfDpi;

            set
            {
                if (pdfDpi != value)
                {
                    pdfDpi = value;
                    OnPropertyChanged(nameof(PdfDpi));
                }
            }
        }

        public new ICommand ViewerBack { get; }

        public new ICommand ViewerNext { get; }

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

        private BitmapSource BitmapSourceFromByteArray(byte[] buffer)
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
    }
}