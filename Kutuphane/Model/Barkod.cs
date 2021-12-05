using Extensions;
using System.Windows.Media.Imaging;

namespace Kutuphane.Model
{
    public class Barkod : InpcBase
    {
        public string BarkodError
        {
            get => barkodError;

            set
            {
                if (barkodError != value)
                {
                    barkodError = value;
                    OnPropertyChanged(nameof(BarkodError));
                }
            }
        }

        public BitmapSource BarkodImage
        {
            get => barkodImage;

            set
            {
                if (barkodImage != value)
                {
                    barkodImage = value;
                    OnPropertyChanged(nameof(BarkodImage));
                }
            }
        }

        public string Metin
        {
            get => metin;

            set
            {
                if (metin != value)
                {
                    metin = value;
                    OnPropertyChanged(nameof(Metin));
                }
            }
        }

        public int QrHeight
        {
            get => qrHeight;

            set
            {
                if (qrHeight != value)
                {
                    qrHeight = value;
                    OnPropertyChanged(nameof(QrHeight));
                }
            }
        }

        public int QrWidth
        {
            get => qrWidth;

            set
            {
                if (qrWidth != value)
                {
                    qrWidth = value;
                    OnPropertyChanged(nameof(QrWidth));
                }
            }
        }

        private string barkodError;

        private BitmapSource barkodImage;

        private string metin;

        private int qrHeight = 75;

        private int qrWidth = 150;
    }
}