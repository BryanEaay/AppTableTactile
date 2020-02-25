using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppPalaisRois.ViewModel
{
    public class AppRegionMainViewModel : ViewModelBase
    {
        #region Private Fields

        private ObservableCollection<Map> _maps;

        // Variables
        private string _scatterViewVisibility = "Visible";

        private Map _selected;

        #endregion Private Fields

        #region Public Constructors

        public AppRegionMainViewModel()
        {
            this._maps = FindMaps();
        }

        #endregion Public Constructors

        #region Private Destructors

        ~AppRegionMainViewModel()
        {
            DAORegion.Instance.Dispose();
            _scatterViewVisibility = null;
            _selected = null;
            _maps = null;
        }

        #endregion Private Destructors

        #region Private Methods

        private ObservableCollection<Map> FindMaps()
        {
            List<Map> maps = new List<Map>(DAORegion.Instance.Maps);
            maps.Sort((a, b) => (a.ID.CompareTo(b.ID)));
            return new ObservableCollection<Map>(maps);
        }

        #endregion Private Methods

        #region Getters and Setters

        public ObservableCollection<Map> Maps
        {
            get { return _maps; }
            set { _maps = value; OnPropertyChanged("Maps"); }
        }

        public string ScatterViewVisibility
        {
            get { return _scatterViewVisibility; }
            set { _scatterViewVisibility = value; OnPropertyChanged("ScatterViewVisibility"); }
        }

        public Map Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("Selected"); }
        }

        #endregion Getters and Setters
    }
}