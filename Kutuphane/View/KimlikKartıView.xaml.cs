using Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KimlikKartıView.xaml
    /// </summary>
    public partial class KimlikKartıView : UserControl, INotifyPropertyChanged
    {
        private Brush _BirleşikRenk;

        private bool _RadialBrush;

        private string _Renk = "Transparent";

        private string _Renk1 = "Maroon";

        private string _Renk2 = "Blue";

        private string _YazıRenk = "Black";

        private ICommand imzaSil;

        public KimlikKartıView()
        {
            InitializeComponent();

            İmzaSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is InkCanvas ınkCanvas)
                {
                    ınkCanvas.Strokes.Clear();
                }
            }, parameter => true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Brush BirleşikRenk
        {
            get => RadialBrush
                ? new RadialGradientBrush((Color)ColorConverter.ConvertFromString(Renk1), (Color)ColorConverter.ConvertFromString(Renk2))
                : new LinearGradientBrush((Color)ColorConverter.ConvertFromString(Renk1), (Color)ColorConverter.ConvertFromString(Renk2), new Point(0.5, 0), new Point(0.5, 1));

            set
            {
                if (_BirleşikRenk != value)
                {
                    _BirleşikRenk = value;
                    OnPropertyChanged(nameof(BirleşikRenk));
                }
            }
        }

        public ICommand İmzaSil
        {
            get => imzaSil;

            set
            {
                if (imzaSil != value)
                {
                    imzaSil = value;
                    OnPropertyChanged(nameof(İmzaSil));
                }
            }
        }

        public bool RadialBrush
        {
            get => _RadialBrush;

            set
            {
                if (_RadialBrush != value)
                {
                    _RadialBrush = value;
                    OnPropertyChanged(nameof(RadialBrush));
                    OnPropertyChanged(nameof(BirleşikRenk));
                }
            }
        }

        public string Renk
        {
            get => _Renk;

            set
            {
                if (_Renk != value)
                {
                    _Renk = value;
                    OnPropertyChanged(nameof(Renk));
                }
            }
        }

        public string Renk1
        {
            get => _Renk1;

            set
            {
                if (_Renk1 != value)
                {
                    _Renk1 = value;
                    OnPropertyChanged(nameof(Renk1));
                    OnPropertyChanged(nameof(BirleşikRenk));
                }
            }
        }

        public string Renk2
        {
            get => _Renk2;

            set
            {
                if (_Renk2 != value)
                {
                    _Renk2 = value;
                    OnPropertyChanged(nameof(Renk2));
                    OnPropertyChanged(nameof(BirleşikRenk));
                }
            }
        }

        public string YazıRenk
        {
            get => _YazıRenk;

            set
            {
                if (_YazıRenk != value)
                {
                    _YazıRenk = value;
                    OnPropertyChanged(nameof(YazıRenk));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}