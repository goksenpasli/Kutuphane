﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Extensions
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty SelectedItemsMaxCountProperty = DependencyProperty.RegisterAttached("SelectedItemsMaxCount", typeof(int), typeof(ListBoxHelper), new PropertyMetadata(int.MaxValue, OnSelectedItemsChanged));

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListBoxHelper), new PropertyMetadata(default(IList), OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        public static int GetSelectedItemsMaxCount(DependencyObject obj)
        {
            return (int)obj.GetValue(SelectedItemsMaxCountProperty);
        }

        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        public static void SetSelectedItemsMaxCount(DependencyObject obj, int value)
        {
            obj.SetValue(SelectedItemsMaxCountProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = (ListBox)d;
            ReSetSelectedItems(listBox);
            listBox.SelectionChanged += delegate
            {
                if (listBox.SelectedItems.Count > GetSelectedItemsMaxCount(listBox))
                {
                    listBox.SelectedItems.Clear();
                    MessageBox.Show($"En Fazla {GetSelectedItemsMaxCount(listBox)} Adet Seçim Yapabilirsiniz.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                ReSetSelectedItems(listBox);
            };
        }

        private static void ReSetSelectedItems(ListBox listBox)
        {
            IList selectedItems = GetSelectedItems(listBox);
            selectedItems?.Clear();
            if (listBox.SelectedItems != null)
            {
                foreach (object item in listBox.SelectedItems)
                {
                    _ = selectedItems?.Add(item);
                }
            }
        }
    }
}