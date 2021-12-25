using Extensions;
using Kutuphane.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapSilViewModel : InpcBase
    {
        public KitapSilViewModel()
        {
            KitapSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is ObservableCollection<Kitap> liste && MessageBox.Show($"{SeçiliKitap?.Ad} Adlı Seçili {(KitapDurumu)SeçiliKitap?.KitapDurumId} Kitabı Silmek İstiyor Musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    _ = liste.Remove(SeçiliKitap);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter =>
            {
                return (SeçiliKitap?.KitapDurumId) switch
                {
                    (int)KitapDurumu.Okuyucuda or (int)KitapDurumu.Kütüphanede => false,
                    (int)KitapDurumu.Kayıp or (int)KitapDurumu.Yıpranmış => true,
                    _ => false,
                };
            });
        }

        public ICommand KitapSil { get; }

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

        public ObservableCollection<Kitap> SeçiliKitaplar
        {
            get => seçiliKitaplar;

            set
            {
                if (seçiliKitaplar != value)
                {
                    seçiliKitaplar = value;
                    OnPropertyChanged(nameof(SeçiliKitaplar));
                }
            }
        }

        private Kitap seçiliKitap;

        private ObservableCollection<Kitap> seçiliKitaplar = new();
    }
}