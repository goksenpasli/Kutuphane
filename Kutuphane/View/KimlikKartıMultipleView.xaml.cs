using Extensions;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kutuphane.View
{
    /// <summary>
    /// Interaction logic for KimlikKartıMultipleView.xaml
    /// </summary>
    public partial class KimlikKartıMultipleView : UserControl, INotifyPropertyChanged
    {
        private ICommand imzaSil;

        private ICommand kareKodYazdır;

        public KimlikKartıMultipleView()
        {
            InitializeComponent();

            İmzaSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is InkCanvas ınkCanvas)
                {
                    ınkCanvas.Strokes.Clear();
                }
            }, parameter => true);

            KareKodYazdır = new RelayCommand<object>(parameter =>
            {
                PrintDialog printDlg = new();
                if (printDlg.ShowDialog() == true)
                {
                    printDlg.PrintVisual(parameter as Visual, "KareKod Yazdır.");
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

        public ICommand KareKodYazdır
        {
            get => kareKodYazdır;

            set
            {
                if (kareKodYazdır != value)
                {
                    kareKodYazdır = value;
                    OnPropertyChanged(nameof(KareKodYazdır));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}