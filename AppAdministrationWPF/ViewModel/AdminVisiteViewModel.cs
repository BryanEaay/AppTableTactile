using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;

namespace AppAdministrationWPF.ViewModel
{
    public class AdminVisiteViewModel : ViewModelBase
    {
        #region Private Fields

        private Visite _selected;
        private ObservableCollection<Visite> _visites;
        private string chemin = ConfigurationManager.AppSettings["cheminVisitesVirtuelles"];

        #endregion Private Fields

        #region Public Constructors

        public AdminVisiteViewModel()
        {
            _visites = DAOVisite.Instance.Visites;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The selected place holder Watchout, it could be null...
        /// </summary>
        public Visite Selected
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

        public ObservableCollection<Visite> Visites
        {
            get { return _visites; }
            set { _visites = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public void Save()
        {
            DAOVisite.Save();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DAOVisite.Save();
        }

        #endregion Private Methods
    }
}