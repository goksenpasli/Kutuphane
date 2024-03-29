﻿using Extensions;
using Freeware;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
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

        public static readonly DependencyProperty PdfFilePathProperty = DependencyProperty.Register("PdfFilePath", typeof(string), typeof(PdfViewer), new PropertyMetadata(null, PdfFilePathChanged));

        public static readonly DependencyProperty PdfFileStreamProperty = DependencyProperty.Register("PdfFileStream", typeof(FileStream), typeof(PdfViewer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.NotDataBindable, PdfStreamChanged));

        public PdfViewer()
        {
            OpenFileDialog openFileDialog = null;
            DosyaAç = new RelayCommand<object>(parameter =>
            {
                openFileDialog = new() { Multiselect = false, Filter = "Pdf Dosyaları (*.pdf)|*.pdf" };
                if (openFileDialog.ShowDialog() == true)
                {
                    PdfFilePath = openFileDialog.FileName;
                }
            });

            ViewerBack = new RelayCommand<object>(parameter =>
            {
                Sayfa--;
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(PdfFileStream, Sayfa, Dpi));
            }, parameter => Source is not null && Sayfa > 1 && Sayfa <= ToplamSayfa);

            ViewerNext = new RelayCommand<object>(parameter =>
            {
                Sayfa++;
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(PdfFileStream, Sayfa, Dpi));
            }, parameter => Source is not null && Sayfa >= 1 && Sayfa < ToplamSayfa);

            OrijinalPdfDosyaAç = new RelayCommand<object>(parameter => _ = Process.Start(parameter as string), parameter => !DesignerProperties.GetIsInDesignMode(new DependencyObject()) && File.Exists(parameter as string));

            PropertyChanged += PdfViewer_PropertyChanged;
        }

        public new ICommand DosyaAç { get; }

        public int Dpi
        {
            get => (int)GetValue(DpiProperty);
            set => SetValue(DpiProperty, value);
        }

        public int[] DpiList { get; set; } = new int[] { 96, 150, 225, 300, 600 };

        public ICommand OrijinalPdfDosyaAç { get; }

        public string PdfFilePath
        {
            get => (string)GetValue(PdfFilePathProperty);
            set => SetValue(PdfFilePathProperty, value);
        }

        public FileStream PdfFileStream
        {
            get => (FileStream)GetValue(PdfFileStreamProperty);
            set => SetValue(PdfFileStreamProperty, value);
        }

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

        public static BitmapImage BitmapSourceFromByteArray(byte[] buffer)
        {
            if (buffer != null)
            {
                BitmapImage bitmap = new();
                using MemoryStream stream = new(buffer);
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
                    PdfFileStream = null;
                }
                disposedValue = true;
            }
        }

        private bool disposedValue;

        private int toplamSayfa;

        private static void DpiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PdfViewer pdfViewer)
            {
                pdfViewer.Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdfViewer.PdfFileStream, pdfViewer.Sayfa, (int)e.NewValue));
            }
        }

        private static void PdfFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PdfViewer pdfViewer)
            {
                if (e.NewValue is not null)
                {
                    pdfViewer.PdfFileStream = new FileStream(e.NewValue as string, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                else
                {
                    pdfViewer.Source = null;
                }
            }
        }

        private static void PdfStreamChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PdfViewer pdfViewer)
            {
                pdfViewer.ToplamSayfa = Pdf2Png.ConvertAllPages(pdfViewer.PdfFileStream, 0).Count;
                pdfViewer.TifNavigasyonButtonEtkin = pdfViewer.ToplamSayfa > 1 ? Visibility.Visible : Visibility.Collapsed;
                pdfViewer.Pages = Enumerable.Range(1, pdfViewer.ToplamSayfa);
                pdfViewer.Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdfViewer.PdfFileStream, pdfViewer.Sayfa, pdfViewer.Dpi));
            }
        }

        private void PdfViewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Sayfa" && sender is PdfViewer pdfViewer)
            {
                Source = BitmapSourceFromByteArray(Pdf2Png.Convert(pdfViewer.PdfFileStream, Sayfa, pdfViewer.Dpi));
            }
        }
    }
}