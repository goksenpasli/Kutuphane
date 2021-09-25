﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Extensions.Controls
{
    public partial class MediaViewer : UserControl
    {
        public static readonly DependencyProperty EndTimeSpanProperty = DependencyProperty.Register("EndTimeSpan", typeof(TimeSpan), typeof(MediaViewer), new PropertyMetadata(TimeSpan.Zero));

        public static readonly DependencyProperty MediaDataFilePathProperty = DependencyProperty.Register("MediaDataFilePath", typeof(string), typeof(MediaViewer), new PropertyMetadata(null, MediaDataFilePathChanged));

        public static readonly DependencyProperty MediaPositionProperty = DependencyProperty.Register("MediaPosition", typeof(TimeSpan), typeof(MediaViewer), new PropertyMetadata(TimeSpan.Zero, MediaPositionChanged));

        public static readonly DependencyProperty MediaVolumeProperty = DependencyProperty.Register("MediaVolume", typeof(double), typeof(MediaViewer), new PropertyMetadata(1d, MediaVolumeChanged));

        public static readonly DependencyProperty ThumbnailsVisibleProperty = DependencyProperty.Register("ThumbnailsVisible", typeof(bool), typeof(MediaViewer), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly Image image = new();

        private static readonly MediaElement mediaElement = new() { UnloadedBehavior = MediaState.Manual, ScrubbingEnabled = true };

        private static readonly ToolTip tooltip = new();

        private static bool dragging;

        private static DispatcherTimer timer;

        public MediaViewer()
        {
            InitializeComponent();
            DataContext = this;
        }

        public TimeSpan EndTimeSpan { get => (TimeSpan)GetValue(EndTimeSpanProperty); set => SetValue(EndTimeSpanProperty, value); }

        public string MediaDataFilePath { get => (string)GetValue(MediaDataFilePathProperty); set => SetValue(MediaDataFilePathProperty, value); }

        public TimeSpan MediaPosition { get => (TimeSpan)GetValue(MediaPositionProperty); set => SetValue(MediaPositionProperty, value); }

        public double MediaVolume
        {
            get => (double)GetValue(MediaVolumeProperty);
            set => SetValue(MediaVolumeProperty, value);
        }

        public bool ThumbnailsVisible { get => (bool)GetValue(ThumbnailsVisibleProperty); set => SetValue(ThumbnailsVisibleProperty, value); }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private static void MediaDataFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
            if (d is MediaViewer viewer && e.NewValue != null)
            {
                try
                {
                    var uriString = (string)e.NewValue;
                    viewer.Player.Source = new Uri(uriString);
                    viewer.Player.MediaOpened += (f, g) =>
                    {
                        if (f is MediaElement mediaelement && mediaelement.NaturalDuration.HasTimeSpan)
                        {
                            viewer.EndTimeSpan = mediaelement.NaturalDuration.TimeSpan;
                            timer = new DispatcherTimer(TimeSpan.FromMilliseconds(1000), DispatcherPriority.Normal, (s, ee) => viewer.MediaPosition = mediaelement.Position, Dispatcher.CurrentDispatcher);
                            timer.Start();
                        }
                    };
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message, "EBYS", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private static void MediaPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MediaViewer viewer && e.NewValue != null && !dragging)
            {
                viewer.Player.Position = (TimeSpan)e.NewValue;
            }
        }

        private static void MediaVolumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MediaViewer viewer && e.NewValue != null)
            {
                viewer.Player.Volume = (double)e.NewValue;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Player.Position = Player.Position.Subtract(new TimeSpan(0, 0, 30));

        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            if (Player.NaturalVideoWidth > 0)
            {
                var picturesfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                var data = Player.ToRenderTargetBitmap().ToTiffJpegByteArray(ExtensionMethods.Format.Jpg);
                File.WriteAllBytes(picturesfolder.SetUniqueFile("Resim", "jpg"), data);
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e) => Player.Position = Player.Position.Add(new TimeSpan(0, 0, 30));

        private void Mute_Checked(object sender, RoutedEventArgs e) => MediaVolume = 0;

        private void Mute_Unchecked(object sender, RoutedEventArgs e) => MediaVolume = 1;

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (Player.CanPause)
            {
                Player.Pause();
            }
        }

        private double PixelsToValue(double pixels, double minValue, double maxValue, double width)
        {
            var range = maxValue - minValue;
            var percentage = pixels / width * 100;
            return (percentage / 100 * range) + minValue;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (MediaDataFilePath != null)
            {
                Player.Play();
            }
        }

        private void Sld_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragging = false;
            Player.Position = TimeSpan.FromSeconds(Sld.Value);
            timer.Start();
        }

        private void Sld_DragStarted(object sender, DragStartedEventArgs e)
        {
            dragging = true;
            timer.Stop();
        }

        private void Sld_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ThumbnailsVisible)
            {
                try
                {
                    tooltip.IsOpen = false;
                    image.Source = null;
                    if (mediaElement.CanPause)
                    {
                        mediaElement.Pause();
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message, "EBYS", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Sld_MouseMove(object sender, MouseEventArgs e)
        {
            if (ThumbnailsVisible && Player.HasVideo)
            {
                try
                {
                    _ = Task.Factory.StartNew(() =>
                      {
                          _ = Dispatcher.BeginInvoke((Action)(() =>
                          {
                              var oran = SystemParameters.PrimaryScreenWidth / SystemParameters.PrimaryScreenHeight;
                              tooltip.Width = 96 * oran;
                              tooltip.Height = 96;
                              tooltip.PlacementTarget = Sld;
                              tooltip.Placement = PlacementMode.Mouse;
                              var predictedValue = PixelsToValue(e.GetPosition(Sld).X, Sld.Minimum, Sld.Maximum, Sld.ActualWidth);
                              mediaElement.IsMuted = true;
                              mediaElement.Source = Player.Source;
                              mediaElement.Height = tooltip.Height;
                              mediaElement.Width = tooltip.Width;
                              mediaElement.Pause();
                              mediaElement.Position = TimeSpan.FromSeconds(predictedValue);
                              image.Source = mediaElement.ToRenderTargetBitmap();
                              tooltip.Content = image;
                              if (!tooltip.IsOpen)
                              {
                                  tooltip.IsOpen = true;
                              }
                          }));
                      }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message, "EBYS", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SlowBackward_Click(object sender, RoutedEventArgs e)
        {
            if (Player.CanPause)
            {
                Player.Play();
                Player.Position = Player.Position.Subtract(new TimeSpan(0, 0, 0, 0, 1000 / 30));
                Player.Pause();
            }
        }

        private void SlowForward_Click(object sender, RoutedEventArgs e)
        {
            if (Player.CanPause)
            {
                Player.Play();
                Player.Position = Player.Position.Add(new TimeSpan(0, 0, 0, 0, 1000 / 30));
                Player.Pause();
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source != null)
            {
                Player.Stop();
            }
        }
    }
}