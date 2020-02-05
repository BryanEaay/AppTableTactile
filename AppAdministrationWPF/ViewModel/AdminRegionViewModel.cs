using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppAdministrationWPF.ViewModel
{
    public class AdminRegionViewModel : ViewModelBase
    {
        #region Public Fields

        /// <summary>
        /// The dispatch diameter to dispatch media around the placeholder point
        /// </summary>
        public static double DISPATCH_DIAM = 150;

        #endregion Public Fields

        #region Private Fields

        private ObservableCollection<Map> _maps;

        private Map _selectedMap;
        private PlaceHolder _selectedPlaceholder;

        #endregion Private Fields

        //150

        #region Public Constructors

        public AdminRegionViewModel()
        {
            _maps = DAORegion.Instance.Maps;
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Map> Maps
        {
            get { return _maps; }
            set { _maps = value; }
        }

        /// <summary>
        /// The selected place holder Watchout, it could be null...
        /// </summary>
        public Map SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                // setting value
                _selectedMap = value;
                if (value != null)
                {
                    _selectedMap.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
                }
                OnPropertyChanged("SelectedMap");
            }
        }

        /// <summary>
        /// The selected place holder Watchout, it could be null...
        /// </summary>
        public PlaceHolder SelectedPlaceholder
        {
            get { return _selectedPlaceholder; }
            set
            {
                // setting value
                _selectedPlaceholder = value;
                if (value != null)
                {
                    _selectedPlaceholder.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
                }
                OnPropertyChanged("SelectedPlaceholder");
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Refresh()
        {
            DAORegion.Refresh();
            _maps = DAORegion.Instance.Maps;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DAORegion.Save();
        }

        public static implicit operator AdminRegionViewModel(AdminFriseViewModel v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator AdminRegionViewModel(AdminBanqueImagesViewModel v)
        {
            throw new NotImplementedException();
        }

        #endregion Private Methods
    }
}