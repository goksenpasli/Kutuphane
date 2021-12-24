using Extensions;
using Kutuphane.Model;
using System;
using System.Linq;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class DolapGirişViewModel : InpcBase
    {
        public DolapGirişViewModel()
        {
            Dolap = new Dolap();

            DolapEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Kütüphane kütüphane)
                {
                    Dolap dolap = new() { Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue), Açıklama = Dolap.Açıklama, Kod = Dolap.Kod };
                    kütüphane.Dolaplar?.Add(dolap);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is Kütüphane kütüphane && kütüphane.Dolaplar?.Any(z => z.Kod == Dolap.Kod) == false && !string.IsNullOrWhiteSpace(Dolap.Açıklama) && !double.IsNaN(Dolap.Kod));
        }

        public Dolap Dolap
        {
            get => dolap;

            set
            {
                if (dolap != value)
                {
                    dolap = value;
                    OnPropertyChanged(nameof(Dolap));
                }
            }
        }

        public ICommand DolapEkle { get; }

        private Dolap dolap;
    }
}