using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System.ComponentModel;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class GecikenKitaplarViewModel : InpcBase
    {
        private İşlem işlem;

        private Kişi kişi;

        public GecikenKitaplarViewModel()
        {
            Kişi = new Kişi();
            İşlem = new İşlem();

            UzatmaGir = new RelayCommand<object>(parameter =>
            {
                İşlem.Uzatıldı = true;
                İşlem.UzatmaSayısı++;
                İşlem.GeriGetirmeTarihi = İşlem.UzatılmaTarihi;
                MainViewModel.DatabaseSave.Execute(null);
            }, parameter => İşlem.UzatılmaTarihi > İşlem.GeriGetirmeTarihi);

            Kişi.PropertyChanged += Kişi_PropertyChanged;
        }

        public İşlem İşlem
        {
            get => işlem;

            set
            {
                if (işlem != value)
                {
                    işlem = value;
                    OnPropertyChanged(nameof(İşlem));
                }
            }
        }

        public Kişi Kişi
        {
            get => kişi;

            set
            {
                if (kişi != value)
                {
                    kişi = value;
                    OnPropertyChanged(nameof(Kişi));
                }
            }
        }

        public ICommand UzatmaGir { get; }

        private void Kişi_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "KişiAdArama")
            {
                GecikenKitaplarView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Ad.Contains(Kişi.KişiAdArama);
            }
            if (e.PropertyName is "KişiSoyadArama")
            {
                GecikenKitaplarView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).Soyad.Contains(Kişi.KişiSoyadArama);
            }
            if (e.PropertyName is "KişiTcArama")
            {
                GecikenKitaplarView.cvskişi.Filter += (s, e) => e.Accepted &= (e.Item as Kişi).TC.Contains(Kişi.KişiTcArama);
            }
        }
    }
}