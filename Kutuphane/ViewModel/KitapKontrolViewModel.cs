using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kutuphane.ViewModel
{
    public class KitapKontrolViewModel : InpcBase
    {
        private string kişiKitapAdArama;

        private string kitapBarkodArama;

        private bool okunanKitapRenkGöster;

        private Kişi seçiliKişi;

        private Kitap seçiliKitap;

        public KitapKontrolViewModel()
        {
            KitapGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kitap kitap)
                {
                    kitap.KitapDurumId = (int)KitapDurumu.Kayıp;
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kitap kitap && kitap.Tutanak);

            KitapKayıpTutanakOluştur = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kitap kitap)
                {
                    if (kitap.KitapDurumId == (int)KitapDurumu.Okuyucuda)
                    {
                        _ = MessageBox.Show($"İlgili Kitabın Bedeli Olan {kitap.Fiyat:C} yi Okuyucudan Almayı Unutmayın.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                    if (kitap.KitapDurumId == (int)KitapDurumu.Kütüphanede)
                    {
                        _ = MessageBox.Show($"İlgili Kitabın Bedeli Olan {kitap.Fiyat:C} yi Kütüphane Görevlisinden Almayı Unutmayın.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                    if (MessageBox.Show($"{kitap.Ad} Adlı Kitaba Tutanak Oluşturulacak Onaylıyor musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        kitap.Tutanak = true;
                    }
                }
            }, parameter => parameter is Kitap kitap && kitap.KitapDurumId != (int)KitapDurumu.Kayıp);

            PropertyChanged += KitapKontrolViewModel_PropertyChanged;
        }

        public string KişiKitapAdArama
        {
            get => kişiKitapAdArama;

            set
            {
                if (kişiKitapAdArama != value)
                {
                    kişiKitapAdArama = value;
                    OnPropertyChanged(nameof(KişiKitapAdArama));
                }
            }
        }

        public string KitapBarkodArama
        {
            get => kitapBarkodArama;

            set
            {
                if (kitapBarkodArama != value)
                {
                    kitapBarkodArama = value;
                    OnPropertyChanged(nameof(KitapBarkodArama));
                }
            }
        }

        public ICommand KitapGüncelle { get; }

        public ICommand KitapKayıpTutanakOluştur { get; }

        public bool OkunanKitapRenkGöster
        {
            get => okunanKitapRenkGöster;

            set
            {
                if (okunanKitapRenkGöster != value)
                {
                    okunanKitapRenkGöster = value;
                    OnPropertyChanged(nameof(OkunanKitapRenkGöster));
                }
            }
        }

        public Kişi SeçiliKişi
        {
            get => seçiliKişi;

            set
            {
                if (seçiliKişi != value)
                {
                    seçiliKişi = value;
                    OnPropertyChanged(nameof(SeçiliKişi));
                }
            }
        }

        public Kitap SeçiliKitap
        {
            get => seçiliKitap;

            set
            {
                if (seçiliKitap != value)
                {
                    seçiliKitap = value;
                    OnPropertyChanged(nameof(SeçiliKitap));
                }
            }
        }

        public override string ToString()
        {
            return "KİTAP KONTROL";
        }

        private void KitapKontrolViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KitapKontrolView.cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap)?.Ad.Contains(KişiKitapAdArama) ?? false;
            }

            if (e.PropertyName is "KitapBarkodArama")
            {
                KitapKontrolView.cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap)?.Barkod.Contains(KitapBarkodArama) ?? false;
            }

            if (e.PropertyName is "SeçiliKitap" && SeçiliKitap is not null)
            {
                KitapKontrolView.cvskişi.Filter += (s, e) => e.Accepted = (e.Item as Kişi)?.İşlem?.Any(z => z.KitapId == SeçiliKitap.Id) == true;
            }

            if (e.PropertyName is "SeçiliKişi" or "OkunanKitapRenkGöster" && SeçiliKişi is not null)
            {
                foreach (Kitap kitap in KitapKontrolView.cvs.View.Cast<Kitap>())
                {
                    if (kitap.KitapListeArkaPlanRengi != Brushes.Transparent)
                    {
                        kitap.KitapListeArkaPlanRengi = Brushes.Transparent;
                    }
                    foreach (İşlem _ in SeçiliKişi.İşlem.Where(işlem => işlem.KitapId == kitap.Id))
                    {
                        kitap.KitapListeArkaPlanRengi = OkunanKitapRenkGöster ? Brushes.Yellow : Brushes.Transparent;
                    }
                }
            }
        }
    }
}