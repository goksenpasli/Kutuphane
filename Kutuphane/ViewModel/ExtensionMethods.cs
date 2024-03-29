﻿using Kutuphane.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZXing;

namespace Kutuphane.ViewModel
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Kitap> CsvKitapListesi(this string csvfile, bool fiyat, bool basımyılı, bool dil)
        {
            try
            {
                string[] satırlar = File.ReadAllLines(csvfile, Encoding.Default);
                if (satırlar.Length != 0)
                {
                    CultureInfo culture = new(CultureInfo.CurrentCulture.Name);
                    return satırlar.Select(satır =>
                    {
                        string[] veri = satır.Split(culture.TextInfo.ListSeparator.ToCharArray());
                        Kitap kitap = new()
                        {
                            Ad = veri[0],
                            Barkod =int.Parse(veri[1])
                        };
                        if (fiyat)
                        {
                            kitap.Fiyat = double.Parse(veri[2]);
                        }
                        if (basımyılı)
                        {
                            kitap.BasımYılı = int.Parse(veri[3]);
                        }
                        if (dil)
                        {
                            kitap.KitapDili = veri[4];
                        }
                        return kitap;
                    });
                }

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public static T DeSerialize<T>(this string xmldatapath) where T : class, new()
        {
            XmlSerializer serializer = new(typeof(T));
            using StreamReader stream = new(xmldatapath);
            return serializer.Deserialize(stream) as T;
        }

        public static T DeSerialize<T>(this XElement xElement) where T : class, new()
        {
            XmlSerializer serializer = new(typeof(T));
            return serializer.Deserialize(xElement.CreateReader()) as T;
        }

        public static ObservableCollection<Dolap> DolaplarıYükle()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (File.Exists(MainViewModel.xmldatapath))
            {
                return MainViewModel.xmldatapath.DeSerialize<Kütüphane>().Dolaplar;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(MainViewModel.xmldatapath));
            return new ObservableCollection<Dolap>();
        }

        public static BitmapImage GenerateBarCodeImage(this Barkod barkod, BarcodeFormat format = BarcodeFormat.QR_CODE, bool pure = false)
        {
            try
            {
                BarcodeWriter writer = new()
                {
                    Format = format,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        PureBarcode = pure,
                        Height = barkod.QrHeight,
                        Width = barkod.QrWidth,
                        Margin = 0
                    }
                };
                barkod.BarkodError = "";
                return !string.IsNullOrWhiteSpace(barkod.Metin) ? Extensions.ExtensionMethods.ToBitmapImage(writer.Write(barkod.Metin), ImageFormat.Png) : null;
            }
            catch (Exception ex)
            {
                barkod.BarkodError = ex.Message;
                return null;
            }
        }

        public static DateTime İşGünüEkle(this DateTime date, double addDays)
        {
            while (addDays != 0)
            {
                date = date.AddDays(Math.Sign(addDays));
                if (date.DayOfWeek switch
                {
                    DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday or DayOfWeek.Thursday or DayOfWeek.Friday => true,
                    _ => false,
                })
                {
                    addDays -= Math.Sign(addDays);
                }
            }
            return date;
        }

        public static ObservableCollection<Kişi> KişileriYükle()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (File.Exists(MainViewModel.xmldatapath))
            {
                return MainViewModel.xmldatapath.DeSerialize<Kütüphane>().Kişiler;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(MainViewModel.xmldatapath));
            return new ObservableCollection<Kişi>();
        }

        public static ObservableCollection<Kitap> KitaplarıYükle()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (File.Exists(MainViewModel.xmldatapath))
            {
                return MainViewModel.xmldatapath.DeSerialize<Kütüphane>().Kitaplar;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(MainViewModel.xmldatapath));
            return new ObservableCollection<Kitap>();
        }

        public static void Serialize<T>(this T dataToSerialize) where T : class
        {
            XmlSerializer serializer = new(typeof(T));
            using TextWriter stream = new StreamWriter(MainViewModel.xmldatapath);
            serializer.Serialize(stream, dataToSerialize);
        }

        public static bool TcGeçerli(this string tcKimlikNo)
        {
            int tekler = 0;
            int ciftler = 0;
            if (string.IsNullOrWhiteSpace(tcKimlikNo) || tcKimlikNo.Length != 11 || !tcKimlikNo.All(z => char.IsNumber(z)) || tcKimlikNo.Substring(0, 1) == "0")
            {
                return false;
            }

            for (int i = 0; i < 9; i += 2)
            {
                tekler += int.Parse(tcKimlikNo[i].ToString());
            }

            for (int i = 1; i < 8; i += 2)
            {
                ciftler += int.Parse(tcKimlikNo[i].ToString());
            }

            string k10 = (((tekler * 7) - ciftler) % 10).ToString();
            string k11 = ((tekler + ciftler + int.Parse(tcKimlikNo[9].ToString())) % 10).ToString();
            return k10 == tcKimlikNo[9].ToString() && k11 == tcKimlikNo[10].ToString();
        }

        public static ObservableCollection<KitapTürü> TürleriYükle()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (File.Exists(MainViewModel.xmldatapath))
            {
                return MainViewModel.xmldatapath.DeSerialize<Kütüphane>().KitapTürleri;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(MainViewModel.xmldatapath));
            return new ObservableCollection<KitapTürü>();
        }

        public static ObservableCollection<Yazar> YazarlarıYükle()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (File.Exists(MainViewModel.xmldatapath))
            {
                return MainViewModel.xmldatapath.DeSerialize<Kütüphane>().Yazarlar;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(MainViewModel.xmldatapath));
            return new ObservableCollection<Yazar>();
        }
    }
}