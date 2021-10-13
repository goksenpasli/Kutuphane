using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System;
using System.ComponentModel;
using System.Linq;

namespace Kutuphane.ViewModel
{
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