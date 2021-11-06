using System.ComponentModel;
using TwainWpf.TwainNative;
// ReSharper disable UnusedMember.Local

namespace TwainWpf
{
    public class AreaSettings : INotifyPropertyChanged
    {
        private Units _units;
        public Units Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }

        private float _top;
        public float Top
        {
            get => _top;
            private set
            {
                _top = value;
                OnPropertyChanged(nameof(Top));
            }
        }

        private float _left;
        public float Left
        {
            get => _left;
            private set
            {
                _left = value;
                OnPropertyChanged(nameof(Left));
            }
        }

        private float _bottom;
        public float Bottom
        {
            get => _bottom;
            private set
            {
                _bottom = value;
                OnPropertyChanged(nameof(Bottom));
            }
        }

        private float _right;
        public float Right
        {
            get => _right;
            private set
            {
                _right = value;
                OnPropertyChanged(nameof(Right));
            }
        }

        public AreaSettings(Units units, float top, float left, float bottom, float right)
        {
            _units = units;
            _top = top;
            _left = left;
            _bottom = bottom;
            _right = right;
        }

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }
}