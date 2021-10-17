using Extensions;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KimlikKartıView.xaml
    /// </summary>
    public partial class KimlikKartıView : UserControl, INotifyPropertyChanged
    {
        private string _Renk;

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

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}