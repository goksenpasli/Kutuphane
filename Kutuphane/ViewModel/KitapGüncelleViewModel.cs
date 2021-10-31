using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapGüncelleViewModel : InpcBase
    {
        private string kişiKitapAdArama;

        private string kişiKitapBarkodArama;

        private int kişiKitapKonumArama = 4;

        private int kişiKitapYılArama;

        private ObservableCollection<Dolap> seçiliDolaplar = new();

        public KitapGüncelleViewModel()
        {
            KitapGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is ObservableCollection<Kitap> kitaplar)
                {
                    if (kitaplar.All(z => !string.IsNullOrWhiteSpace(z.Barkod) && !string.IsNullOrWhiteSpace(z.Ad)))
                    {
                        MainViewModel.DatabaseSave.Execute(null);
                    }
                    else
                    {
                        MessageBox.Show("Hatalı Girişleri Düzeltin.", "KÜTÜPHANE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }, parameter => true);

            DolapAra = new RelayCommand<object>(parameter => KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= SeçiliDolaplar.Any(z => z.Seçili && z.Id == (e.Item as Kitap)?.DolapId), parameter => SeçiliDolaplar?.Any(z => z.Seçili) == true);

            PropertyChanged += KitapGüncelleViewModel_PropertyChanged;
        }

        public ICommand DolapAra { get; }

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

        public string KişiKitapBarkodArama
        {
            get => kişiKitapBarkodArama;

            set
            {
                if (kişiKitapBarkodArama != value)
                {
                    kişiKitapBarkodArama = value;
                    OnPropertyChanged(nameof(KişiKitapBarkodArama));
                }
            }
        }

        public int KişiKitapKonumArama
        {
            get => kişiKitapKonumArama;

            set
            {
                if (kişiKitapKonumArama != value)
                {
                    kişiKitapKonumArama = value;
                    OnPropertyChanged(nameof(KişiKitapKonumArama));
                }
            }
        }

        public int KişiKitapYılArama
        {
            get => kişiKitapYılArama;

            set
            {
                if (kişiKitapYılArama != value)
                {
                    kişiKitapYılArama = value;
                    OnPropertyChanged(nameof(KişiKitapYılArama));
                }
            }
        }

        public ICommand KitapGüncelle { get; }

        public ObservableCollection<Dolap> SeçiliDolaplar
        {
            get => seçiliDolaplar;

            set
            {
                if (seçiliDolaplar != value)
                {
                    seçiliDolaplar = value;
                    OnPropertyChanged(nameof(SeçiliDolaplar));
                }
            }
        }

        private void KitapGüncelleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "KişiKitapAdArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Ad.Contains(KişiKitapAdArama) == true;
                    break;

                case "KişiKitapBarkodArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Barkod.Contains(KişiKitapBarkodArama) == true;
                    break;

                case "KişiKitapYılArama":
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.BasımYılı == KişiKitapYılArama;
                    break;

                case "KişiKitapKonumArama":
                    if (KişiKitapKonumArama != 4)
                    {
                        KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.KitapDurumId == KişiKitapKonumArama;
                    }
                    else
                    {
                        KitapGüncelleView.cvs.View.Filter = null;
                    }
                    break;
            }
        }
    }
}