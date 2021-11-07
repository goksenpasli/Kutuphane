using Extensions;
using Kutuphane.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Kutuphane.ViewModel
{
    public class KitapSilViewModel : InpcBase
    {
        private ObservableCollection<Kitap> seçiliKitaplar = new();

        public KitapSilViewModel()
        {
            KitapSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Kitap seçilikitap && data[1] is ObservableCollection<Kitap> liste && MessageBox.Show($"{seçilikitap.Ad} Adlı Seçili {(KitapDurumu)seçilikitap.KitapDurumId} Kitabı Silmek İstiyor Musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    _ = liste.Remove(seçilikitap);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter =>
            {
                if (parameter is object[] data && data[0] is Kitap seçilikitap)
                {
                    switch (seçilikitap.KitapDurumId)
                    {
                        case (int)KitapDurumu.Okuyucuda:
                        case (int)KitapDurumu.Kütüphanede:
                            return false;

                        case (int)KitapDurumu.Kayıp:
                        case (int)KitapDurumu.Yıpranmış:
                            return true;
                    }
                }
                return false;
            });
        }

        public ICommand KitapSil { get; }

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
    }
}