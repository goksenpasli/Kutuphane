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
                if (parameter is object[] data && data[0] is Kitap seçilikitap && data[1] is ObservableCollection<Kitap> liste && MessageBox.Show($"{seçilikitap.Ad} Adlı Seçili {(KitapDurumu)seçilikitap.KitapDurumId} Kitabı Silmek İstiyor Musun?", "KÜTÜPHANE", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    _ = liste.Remove(seçilikitap);
                    MainViewModel.DatabaseSave.Execute(null);
                }
            }, parameter =>
            {
                if (parameter is object[] data && data[0] is Kitap seçilikitap)
                {
                    if (seçilikitap.KitapDurumId == (int)KitapDurumu.Okuyucuda)
                    {
                        seçilikitap.KitapDurum = false;
                        return false;
                    }

                    if (seçilikitap.KitapDurumId == (int)KitapDurumu.Kütüphanede)
                    {
                        seçilikitap.KitapDurum = false;
                        return false;
                    }

                    if (seçilikitap.KitapDurumId == (int)KitapDurumu.Kayıp)
                    {
                        return true;
                    }

                    if (seçilikitap.KitapDurumId == (int)KitapDurumu.Yıpranmış)
                    {
                        return true;
                    }
                    seçilikitap.KitapDurum = true;
                }
                return false;
            });
        }

        public ICommand KitapSil { get; }
    }
}