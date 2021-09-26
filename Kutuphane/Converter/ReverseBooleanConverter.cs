﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Kutuphane
{
    public class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null && !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
    }
}