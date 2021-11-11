using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public static class CalandarHelper
    {
        public static readonly DependencyProperty SingleClickDefocusProperty = DependencyProperty.RegisterAttached("SingleClickDefocus", typeof(bool), typeof(CalandarHelper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(SingleClickDefocusChanged)));

        public static bool GetSingleClickDefocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SingleClickDefocusProperty);
        }

        public static void SetSingleClickDefocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SingleClickDefocusProperty, value);
        }

        private static void SingleClickDefocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Calendar calendar)
            {
                calendar.PreviewMouseDown += (a, b) =>
                {
                    if (Mouse.Captured is Calendar or CalendarItem)
                    {
                        _ = Mouse.Capture(null);
                    }
                };
            }
        }
    }

    public class KitapTakvimViewModel : InpcBase
    {
        private DateTime? seçiliGün;

        public KitapTakvimViewModel()
        {
            PropertyChanged += KitapTakvimViewModel_PropertyChanged;
        }

        public DateTime? SeçiliGün
        {
            get => seçiliGün;

            set
            {
                if (seçiliGün != value)
                {
                    seçiliGün = value;
                    OnPropertyChanged(nameof(SeçiliGün));
                }
            }
        }

        private void KitapTakvimViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "SeçiliGün" && SeçiliGün is not null)
            {
                KitapTakvimView.cvskişi.Filter += (s, e) => e.Accepted = (e.Item as Kişi)?.İşlem.Any(z => z.GeriGetirmeTarihi == SeçiliGün) == true;
            }
        }
    }
}