﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kutuphane.ViewModel
{
    internal class ShadowedImage : Image
    {
        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Point), typeof(ShadowedImage), new PropertyMetadata(new Point(2.5, 2.5)));

        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register("ShadowColor", typeof(SolidColorBrush), typeof(ShadowedImage), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(70, 128, 128, 128))));

        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.Register("ShowShadow", typeof(bool), typeof(ShadowedImage), new PropertyMetadata(false));

        public Point Location
        {
            get => (Point)GetValue(LocationProperty);
            set => SetValue(LocationProperty, value);
        }

        public SolidColorBrush ShadowColor
        {
            get => (SolidColorBrush)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public bool ShowShadow
        {
            get => (bool)GetValue(ShowShadowProperty);
            set => SetValue(ShowShadowProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (ShowShadow)
            {
                dc.DrawRectangle(ShadowColor, null, new Rect(Location, new Size(ActualWidth, ActualHeight)));
            }

            base.OnRender(dc);
        }
    }
}