using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace AppPalaisRois.ViewModel
{
    public class VisiteViewModel : ViewModelBase
    {
        #region Private Fields

        private Panorama _actualPanorama;
        private Visite _selected;
        private ObservableCollection<Visite> _visites;
        private string chemin = ConfigurationManager.AppSettings["cheminVisitesVirtuelles"];

        #endregion Private Fields

        #region Public Constructors

        public VisiteViewModel()
        {
            _visites = FindVisites();
        }

        #endregion Public Constructors

        #region Private Destructors

        ~VisiteViewModel()
        {
            _actualPanorama = null;
            _selected = null;
            chemin = null;
            _visites = null;
            DAOVisite.Instance.Dispose();
        }

        #endregion Private Destructors

        #region Public Methods

        /// <summary>
        /// Trouver le panorama actuel
        /// </summary>
        /// <param name="scene"></param>
        public void FindActualPanorama(string scene)
        {
            _actualPanorama = null;

            foreach (Panorama p in _selected.Panoramas)
            {
                if (p.Scene == scene)
                {
                    _actualPanorama = p;
                    break;
                }
            }

            OnPropertyChanged("ActualPanorama");
        }

        #endregion Public Methods

        #region Private Methods

        private ObservableCollection<Visite> FindVisites()
        {
            List<Visite> visites = new List<Visite>(DAOVisite.Instance.Visites);
            visites.Sort((a, b) => (a.ID.CompareTo(b.ID)));
            return new ObservableCollection<Visite>(visites);
        }

        #endregion Private Methods

        #region Getter & Setter

        public Panorama ActualPanorama
        {
            get { return _actualPanorama; }
        }

        public Visite Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public ObservableCollection<Visite> Visites
        {
            get { return _visites; }
            set { _visites = value; }
        }

        #endregion Getter & Setter
    }
}