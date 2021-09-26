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
        private string kişiKitapAdArama;

        private string kişiKitapBarkodArama;

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

                    return kitaplar.All(z => !string.IsNullOrWhiteSpace(z.Ad));
                }
                return false;
            });

            PropertyChanged += KitapGüncelleViewModel_PropertyChanged;
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

        public ICommand KitapGüncelle { get; }

        private void KitapGüncelleViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiKitapAdArama")
            {
                KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap).Ad.Contains(KişiKitapAdArama);
            }
            if (e.PropertyName is "KişiKitapBarkodArama")
            {
                KitapGüncelleView.cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap).Barkod.Contains(KişiKitapBarkodArama);
            }
        }
    }
}