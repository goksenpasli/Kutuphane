using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System.ComponentModel;

namespace Kutuphane.ViewModel
{
    public class GecikenKitaplarViewModel : InpcBase
    {
        public GecikenKitaplarViewModel()
        {
            Kişi = new Kişi();
            Kişi.PropertyChanged += Kişi_PropertyChanged;
        }

        public Kişi Kişi { get; set; }

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