using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Extensions
{
    public class ImageViewer : Control, INotifyPropertyChanged
    {
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(ImageViewer), new PropertyMetadata(0.0));

        public static readonly DependencyProperty DecodeHeightProperty = DependencyProperty.Register("DecodeHeight", typeof(int), typeof(ImageViewer), new PropertyMetadata(300));

        public static readonly DependencyProperty ImageFilePathProperty = DependencyProperty.Register("ImageFilePath", typeof(string), typeof(ImageViewer), new PropertyMetadata(null, ImageFilePathChanged));

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(null, SourceChanged));

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(ImageViewer), new PropertyMetadata(1.0));

        private TiffBitmapDecoder decoder;

        private Visibility openButtonVisibility = Visibility.Collapsed;

        private IEnumerable<int> pages;

        private Visibility printButtonVisibility = Visibility.Collapsed;

        private int sayfa = 1;

        private Visibility tifNavigasyonButtonEtkin = Visibility.Collapsed;

        private Visibility toolBarVisibility;

        static ImageViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageViewer), new FrameworkPropertyMetadata(typeof(ImageViewer)));
        }

        public ImageViewer()
        {
            DosyaAç = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    ImageFilePath = openFileDialog.FileName;
                }
            });

            ViewerBack = new RelayCommand<object>(parameter =>
            {
                Sayfa--;
                Source = Decoder.Frames[Sayfa - 1];
            }, parameter => Decoder != null && Sayfa > 1 && Sayfa <= Decoder.Frames.Count);

            ViewerNext = new RelayCommand<object>(parameter =>
            {
                Sayfa++;
                Source = Decoder.Frames[Sayfa - 1];
            }, parameter => Decoder != null && Sayfa >= 1 && Sayfa < Decoder.Frames.Count);

            Resize = new RelayCommand<object>(parameter => Zoom = ActualWidth == 0 ? 1 : ActualWidth / Source.Width, parameter => Source is not null);

            OrijinalResimDosyaAç = new RelayCommand<object>(parameter => _ = Process.Start(parameter as string), parameter => !DesignerProperties.GetIsInDesignMode(new DependencyObject()) && File.Exists(parameter as string));

            Yazdır = new RelayCommand<object>(parameter =>
            {
                var pd = new PrintDialog();
                var dv = new DrawingVisual();
                if (Decoder == null)
                {
                    if (pd.ShowDialog() == true)
                    {
                        using (var dc = dv.RenderOpen())
                        {
                            var imagesource = Source.Width > Source.Height
                                ? ((BitmapSource)Source)?.Resize((int)pd.PrintableAreaHeight, (int)pd.PrintableAreaWidth, 90, 300, 300)
                                : ((BitmapSource)Source)?.Resize((int)pd.PrintableAreaWidth, (int)pd.PrintableAreaHeight, 0, 300, 300);
                            imagesource.Freeze();
                            dc.DrawImage(imagesource, new Rect(0, 0, pd.PrintableAreaWidth, pd.PrintableAreaHeight));
                        }

                        pd.PrintVisual(dv, "");
                    }
                }
                else
                {
                    pd.PageRangeSelection = PageRangeSelection.AllPages;
                    pd.UserPageRangeEnabled = true;
                    pd.MaxPage = (uint)Decoder.Frames.Count;
                    pd.MinPage = 1;
                    if (pd.ShowDialog() == true)
                    {
                        int başlangıç;
                        int bitiş;
                        if (pd.PageRangeSelection == PageRangeSelection.AllPages)
                        {
                            başlangıç = 0;
                            bitiş = Decoder.Frames.Count - 1;
                        }
                        else
                        {
                            başlangıç = pd.PageRange.PageFrom - 1;
                            bitiş = pd.PageRange.PageTo - 1;
                        }

                        for (var i = başlangıç; i <= bitiş; i++)
                        {
                            using (var dc = dv.RenderOpen())
                            {
                                var imagesource = Source.Width > Source.Height
                                    ? Decoder.Frames[i]?.Resize((int)pd.PrintableAreaHeight, (int)pd.PrintableAreaWidth, 90, 300, 300)
                                    : Decoder.Frames[i]?.Resize((int)pd.PrintableAreaWidth, (int)pd.PrintableAreaHeight, 0, 300, 300);
                                imagesource.Freeze();
                                dc.DrawImage(imagesource, new Rect(0, 0, pd.PrintableAreaWidth, pd.PrintableAreaHeight));
                            }

                            pd.PrintVisual(dv, "");
                        }
                    }
                }
            }, parameter => Source is not null);

            PropertyChanged += ImageViewer_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public int DecodeHeight
        {
            get => (int)GetValue(DecodeHeightProperty);
            set => SetValue(DecodeHeightProperty, value);
        }

        public TiffBitmapDecoder Decoder
        {
            get => decoder;

            set
            {
                if (decoder != value)
                {
                    decoder = value;
                    OnPropertyChanged(nameof(Decoder));
                }
            }
        }

        public ICommand DosyaAç { get; }

        public string ImageFilePath
        {
            get => (string)GetValue(ImageFilePathProperty);
            set => SetValue(ImageFilePathProperty, value);
        }

        public Visibility OpenButtonVisibility
        {
            get => openButtonVisibility;

            set
            {
                if (openButtonVisibility != value)
                {
                    openButtonVisibility = value;
                    OnPropertyChanged(nameof(OpenButtonVisibility));
                }
            }
        }

        public ICommand OrijinalResimDosyaAç { get; }

        public IEnumerable<int> Pages
        {
            get => pages;

            set
            {
                if (pages != value)
                {
                    pages = value;
                    OnPropertyChanged(nameof(Pages));
                }
            }
        }

        public Visibility PrintButtonVisibility
        {
            get => printButtonVisibility;

            set
            {
                if (printButtonVisibility != value)
                {
                    printButtonVisibility = value;
                    OnPropertyChanged(nameof(PrintButtonVisibility));
                }
            }
        }

        public ICommand Resize { get; }

        public int Sayfa
        {
            get => sayfa;

            set
            {
                if (sayfa != value)
                {
                    sayfa = value;
                    OnPropertyChanged(nameof(Sayfa));
                }
            }
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Visibility TifNavigasyonButtonEtkin
        {
            get => tifNavigasyonButtonEtkin;

            set
            {
                if (tifNavigasyonButtonEtkin != value)
                {
                    tifNavigasyonButtonEtkin = value;
                    OnPropertyChanged(nameof(TifNavigasyonButtonEtkin));
                }
            }
        }

        public Visibility ToolBarVisibility
        {
            get => toolBarVisibility;

            set
            {
                if (toolBarVisibility != value)
                {
                    toolBarVisibility = value;
                    OnPropertyChanged(nameof(ToolBarVisibility));
                }
            }
        }

        public ICommand ViewerBack { get; }

        public ICommand ViewerNext { get; }

        public ICommand Yazdır { get; }

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        protected virtual void OnPropertyChanged(string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static void ImageFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageViewer imageViewer && e.NewValue is not null)
            {
                switch (Path.GetExtension(e.NewValue as string).ToLower())
                {
                    case ".tiff":
                    case ".tif":
                        imageViewer.Sayfa = 1;
                        imageViewer.Decoder = new TiffBitmapDecoder(new Uri(e.NewValue as string), BitmapCreateOptions.None, BitmapCacheOption.None);
                        imageViewer.TifNavigasyonButtonEtkin = Visibility.Visible;
                        imageViewer.Source = imageViewer.Decoder.Frames[0];
                        imageViewer.Pages = Enumerable.Range(1, imageViewer.Decoder.Frames.Count);
                        break;

                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                        imageViewer.TifNavigasyonButtonEtkin = Visibility.Collapsed;
                        BitmapImage image = new();
                        image.BeginInit();
                        image.DecodePixelHeight = imageViewer.DecodeHeight;
                        image.CacheOption = BitmapCacheOption.None;
                        image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        image.UriSource = new Uri(e.NewValue as string);
                        image.EndInit();
                        if (!image.IsFrozen && image.CanFreeze)
                        {
                            image.Freeze();
                        }
                        imageViewer.Source = image;
                        break;
                }
            }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageViewer imageViewer && imageViewer.Source is not null)
            {
                imageViewer.Zoom = !double.IsNaN(imageViewer.Width)
                    ? imageViewer.Width == 0 ? 1 : imageViewer.Width / imageViewer.Source.Width
                    : imageViewer.ActualWidth == 0 ? 1 : imageViewer.ActualWidth / imageViewer.Source.Width;
            }
        }

        private void ImageViewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Sayfa" && Decoder is not null)
            {
                Source = Decoder.Frames[Sayfa - 1];
            }
        }
    }
}