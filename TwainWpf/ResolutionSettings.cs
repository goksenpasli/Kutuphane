using System.ComponentModel;

namespace TwainWpf
{
    public class ResolutionSettings : INotifyPropertyChanged
    {
        private int? _dpi;

        /// <summary>
        /// The DPI to scan at. Set to null to use the current default setting.
        /// </summary>
        public int? Dpi
        {
            get => _dpi;
            set
            {
                if (value != _dpi)
                {
                    _dpi = value;
                    OnPropertyChanged(nameof(Dpi));
                }
            }
        }

        private ColourSetting _colourSettings;

        /// <summary>
        /// The colour settings to use.
        /// </summary>
        public ColourSetting ColourSetting
        {
            get => _colourSettings;
            set
            {
                if (value != _colourSettings)
                {
                    _colourSettings = value;
                    OnPropertyChanged(nameof(ColourSetting));
                }
            }
        }

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        /// <summary>
        /// Fax quality resolution.
        /// </summary>
        public static readonly ResolutionSettings Fax = new ResolutionSettings()
        {
            Dpi = 300,
            ColourSetting = ColourSetting.BlackAndWhite
        };

        /// <summary>
        /// Photocopier quality resolution.
        /// </summary>
        public static readonly ResolutionSettings Photocopier = new ResolutionSettings()
        {
            Dpi = 300,
            ColourSetting = ColourSetting.GreyScale
        };

        /// <summary>
        /// Colour photocopier quality resolution.
        /// </summary>
        public static readonly ResolutionSettings ColourPhotocopier = new ResolutionSettings()
        {
            Dpi = 300,
            ColourSetting = ColourSetting.Colour
        };
    }

    public enum ColourSetting
    {
        BlackAndWhite = 0,

        GreyScale = 1,

        Colour = 2
    }
}