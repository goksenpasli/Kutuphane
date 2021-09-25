using Kutuphane.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kutuphane.ViewModel
{
    public static class ExtensionMethods
    {
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

        public static string HarfeDöndür(this int counter)
        {
            Stack<char> stack = new();
            if (counter == 0)
            {
                return "A";
            }
            while (counter > 0)
            {
                stack.Push((char)('A' + counter % 26));
                counter /= 26;
            }

            return new string(stack.ToArray());
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
            var tekler = 0;
            var ciftler = 0;
            if (string.IsNullOrWhiteSpace(tcKimlikNo))
            {
                return false;
            }

            if (tcKimlikNo.Length != 11)
            {
                return false;
            }

            if (!tcKimlikNo.All(z => char.IsNumber(z)))
            {
                return false;
            }

            if (tcKimlikNo.Substring(0, 1) == "0")
            {
                return false;
            }

            for (var i = 0; i < 9; i += 2)
            {
                tekler += int.Parse(tcKimlikNo[i].ToString());
            }

            for (var i = 1; i < 8; i += 2)
            {
                ciftler += int.Parse(tcKimlikNo[i].ToString());
            }

            var k10 = (((tekler * 7) - ciftler) % 10).ToString();
            var k11 = ((tekler + ciftler + int.Parse(tcKimlikNo[9].ToString())) % 10).ToString();
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