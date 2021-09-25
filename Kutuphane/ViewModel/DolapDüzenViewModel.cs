using Extensions;
using Kutuphane.Model;
using Kutuphane.View;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class DolapDüzenViewModel : InpcBase
    {
        private string dolapAdı;

        private Dolap seçiliDolap;

        public DolapDüzenViewModel()
        {
            DolapGüncelle = new RelayCommand<object>(parameter => MainViewModel.DatabaseSave.Execute(null), parameter => parameter is Dolap dolap && !string.IsNullOrWhiteSpace(dolap.Açıklama) && !double.IsNaN(dolap.Kod));

            PropertyChanged += DolapDüzenViewModel_PropertyChanged;
        }

        public string DolapAdı
        {
            get => dolapAdı;

            set
            {
                if (dolapAdı != value)
                {
                    dolapAdı = value;
                    OnPropertyChanged(nameof(DolapAdı));
                }
            }
        }

        public ICommand DolapGüncelle { get; }

        public Dolap SeçiliDolap
        {
            get => seçiliDolap;

            set
            {
                if (seçiliDolap != value)
                {
                    seçiliDolap = value;
                    OnPropertyChanged(nameof(SeçiliDolap));
                }
            }
        }

        private void DolapDüzenViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "DolapAdı")
            {
                DolapDüzenView.cvsdolaplar.Filter += (s, e) => e.Accepted = (e.Item as Dolap)?.Açıklama.Contains(DolapAdı) == true || (e.Item as Dolap)?.Kod.ToString().Contains(DolapAdı) == true;
            }

            if (e.PropertyName is "SeçiliDolap" && SeçiliDolap is not null)
            {
                DolapDüzenView.cvs.Filter += (s, e) => e.Accepted = (e.Item as Kitap)?.DolapId == SeçiliDolap.Id;
            }
        }
    }
}