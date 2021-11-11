using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Kutuphane.ViewModel
{
    public class QrCodeMultipleViewModel : QrCodeViewModel
    {
        private ObservableCollection<BitmapSource> barkodResimler;

        private int boy = 4;

        private int en = 4;

        public QrCodeMultipleViewModel()
        {
            PropertyChanged += QrCodeMultipleViewModel_PropertyChanged;
        }

        public ObservableCollection<BitmapSource> BarkodResimler
        {
            get => barkodResimler;

            set
            {
                if (barkodResimler != value)
                {
                    barkodResimler = value;
                    OnPropertyChanged(nameof(BarkodResimler));
                }
            }
        }

        public int Boy
        {
            get => boy;

            set
            {
                if (boy != value)
                {
                    boy = value;
                    OnPropertyChanged(nameof(Boy));
                }
            }
        }

        public int En
        {
            get => en;

            set
            {
                if (en != value)
                {
                    en = value;
                    OnPropertyChanged(nameof(En));
                }
            }
        }

        private void QrCodeMultipleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BarkodResimler.Clear();
            if (e.PropertyName is "En" or "Boy" or "BarcodeFormat" or "PureBarcode")
            {
                Barkod.BarkodImage = Barkod.GenerateBarCodeImage(BarcodeFormat, PureBarcode);
                for (int i = 0; i < En * Boy; i++)
                {
                    BarkodResimler.Add(Barkod.BarkodImage);
                }
            }
        }
    }
}