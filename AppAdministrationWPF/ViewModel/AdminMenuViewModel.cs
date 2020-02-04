using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace AppAdministrationWPF.ViewModel
{
    public class AdminMenuViewModel : ViewModelBase
    {
        #region Private Fields

        private string _background;
        private Credits _credits;
        private ObservableCollection<Icon> _icons;
        private Icon _selected;

        #endregion Private Fields

        #region Public Constructors

        public AdminMenuViewModel()
        {
            this.Load();

            // On vérifie que les chemins existent
            foreach (Icon icon in _icons)
            {
                if (!File.Exists(icon.Source))
                {
                    // Image d'erreur
                    icon.Source = "pack://application:,,,/CommonSurface;component/Resources/ballError.png";
                }
            }

            // On vérifie pour l'icone des crédits
            if (!File.Exists(_credits.Source))
            {
                _credits.Source = "pack://application:,,,/CommonSurface;component/Resources/ballError.png";
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public string Background
        {
            get { return _background; }
            set { _background = value; }
        }

        public Credits Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }

        public ObservableCollection<Icon> Icons
        {
            get { return _icons; }
            set { _icons = value; }
        }

        /// <summary>
        /// The selected place holder Watchout, it could be null...
        /// </summary>
        public Icon Selected
        {
            get { return _selected; }
            set
            {
                // setting value
                _selected = value;
                if (value != null)
                {
                    _selected.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
                }
                OnPropertyChanged("Selected");
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Refresh()
        {
            DAOMenu.Refresh();
            this.Load();
        }

        #endregion Public Methods

        #region Private Methods

        private void Load()
        {
            _icons = DAOMenu.Instance.Icons;
            _credits = DAOMenu.Instance.Credits;
            _background = DAOMenu.Instance.Background;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DAOMenu.Save();
        }

        #endregion Private Methods
    }
}