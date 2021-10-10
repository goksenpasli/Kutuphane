using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapGüncelleViewModel : InpcBase
    {
        private int kişiDolapIdArama;

        private string kişiKitapAdArama;

        private string kişiKitapBarkodArama;

        private int kişiKitapKonumArama = 4;

        private int kişiKitapYılArama;

        public KitapGüncelleViewModel()
        {
            KitapGüncelle = new RelayCommand<object>(parameter => MainViewModel.DatabaseSave.Execute(null), parameter =>
            {
                if (parameter is ObservableCollection<Kitap> kitaplar && kitaplar.Any())
                {
                    foreach (var item in kitaplar.Where(item => item.KitapDurumId is ((int)KitapDurumu.Kütüphanede) or ((int)KitapDurumu.Okuyucuda)))
                    {
                        item.KitapDurum = false;
                    }

                    return kitaplar.All(z => !string.IsNullOrWhiteSpace(z.Barkod) && !string.IsNullOrWhiteSpace(z.Ad));
                }
                return false;
            });

            PropertyChanged += KitapGüncelleViewModel_PropertyChanged;
        }

        public int KişiDolapIdArama
        {
            get => kişiDolapIdArama;

            set
            {
                if (kişiDolapIdArama != value)
                {
                    kişiDolapIdArama = value;
                    OnPropertyChanged(nameof(KişiDolapIdArama));
                }
            }
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

        private void KitapGüncelleViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Ad.Contains(KişiKitapAdArama) == true;
            }
            if (e.PropertyName is "KişiKitapBarkodArama")
            {
                KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.Barkod.Contains(KişiKitapBarkodArama) == true;
            }
            if (e.PropertyName is "KişiDolapIdArama")
            {
                KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.DolapId == KişiDolapIdArama;
            }
            if (e.PropertyName is "KişiKitapYılArama")
            {
                KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.BasımYılı == KişiKitapYılArama;
            }
            if (e.PropertyName is "KişiKitapKonumArama")
            {
                if (KişiKitapKonumArama != 4)
                {
                    KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted &= (e.Item as Kitap)?.KitapDurumId == KişiKitapKonumArama;
                }
                else
                {
                    KitapGüncelleView.cvs.View.Filter = null;
                }
            }
        }
    }
}