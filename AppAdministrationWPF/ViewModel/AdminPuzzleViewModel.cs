using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System.Collections.ObjectModel;

namespace AppAdministrationWPF.ViewModel
{
    public class AdminPuzzleViewModel : ViewModelBase
    {
        #region Private Fields

        private ObservableCollection<Picture> _pictures;

        private Picture _selected;

        #endregion Private Fields

        #region Public Constructors

        public AdminPuzzleViewModel()
        {
            // Init vars
            _pictures = new ObservableCollection<Picture>();
            _selected = null;
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Picture> Pictures
        {
            get { return _pictures; }
            set
            {
                _pictures = value;
                OnPropertyChanged("Pictures");
            }
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

        #endregion Public Properties

        #region Public Methods

        public void FilterPictures(string difficulty)
        {
            Pictures = DAOPuzzle.Instance.FindPictures(difficulty);
        }

        public void Refresh()
        {
            DAOPuzzle.Instance.Refresh();
        }

        public void Save()
        {
            DAOPuzzle.Instance.Update();
        }

        #endregion Public Methods
    }
}