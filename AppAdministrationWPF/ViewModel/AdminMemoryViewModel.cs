using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System.Collections.ObjectModel;

namespace AppAdministrationWPF.ViewModel
{
    public class AdminMemoryViewModel : ViewModelBase
    {
        #region Private Fields

        private ObservableCollection<Picture> _pictures;
        private Picture _selected;

        #endregion Private Fields

        #region Public Constructors

        public AdminMemoryViewModel()
        {
        }

        #endregion Public Constructors

        #region PROPERTIES

        public ObservableCollection<Picture> Pictures
        {
            get { return _pictures; }
            set { _pictures = value; OnPropertyChanged("Pictures"); }
        }

        public Picture Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        #endregion PROPERTIES

        #region Public Methods

        public void filterPictures(string type)
        {
            if (type != "Background")
            {
                Pictures = DAOMemory.Instance.FindPictures(type);
            }
            else
            {
                Pictures = DAOMemory.Instance.Backgrounds;
            }
        }

        public void Refresh()
        {
            DAOMemory.Refresh();
            var a = DAOMemory.Instance;
        }

        public void Save()
        {
            DAOMemory.Save();
        }

        #endregion Public Methods
    }
}