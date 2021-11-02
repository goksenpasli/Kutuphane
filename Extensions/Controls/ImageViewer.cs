using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
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

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(null, SourceChanged));

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(ImageViewer), new PropertyMetadata(1.0));

        private TiffBitmapDecoder decoder;

        private Visibility openButtonVisibility = Visibility.Collapsed;

        private int sayfa = 1;

        private Visibility tifNavigasyonButtonEtkin = Visibility.Collapsed;

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
                    Decoder = null;
                    switch (Path.GetExtension(openFileDialog.FileName).ToLower())
                    {
                        case ".tiff":
                        case ".tif":
                            Sayfa = 1;
                            Decoder = new TiffBitmapDecoder(new Uri(openFileDialog.FileName), BitmapCreateOptions.None, BitmapCacheOption.None);
                            TifNavigasyonButtonEtkin = Visibility.Visible;
                            Source = Decoder.Frames[0];
                            break;

                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            TifNavigasyonButtonEtkin = Visibility.Collapsed;
                            BitmapImage image = new();
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.None;
                            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            image.UriSource = new Uri(openFileDialog.FileName);
                            image.EndInit();
                            if (!image.IsFrozen && image.CanFreeze)
                            {
                                image.Freeze();
                            }
                            Source = image;
                            break;
                    }
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
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

        public ICommand DosyaAç { get; }

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

        public ICommand ViewerBack { get; }

        public ICommand ViewerNext { get; }

        public ICommand Yazdır { get; }

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        protected virtual void OnPropertyChanged(string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageViewer imageViewer && imageViewer.Source is not null)
            {
                imageViewer.Zoom = imageViewer.ActualWidth / imageViewer.Source.Width;
            }
        }
    }
}