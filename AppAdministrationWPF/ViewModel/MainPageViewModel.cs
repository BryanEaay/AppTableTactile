using AppAdministrationWPF.Utils;
using CommonSurface.Utils;
using CommonSurface.ViewModel;
using System.Windows.Input;

namespace AppAdministrationWPF.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private Fields

        private ICommand _showAdminMediathequeView;
        private ICommand _showAdminMemoryView;
        private ICommand _showAdminMenuView;
        private ICommand _showAdminPuzzleView;
        private ICommand _showAdminRegionView;
        private ICommand _showAdminVisiteView;
        private ViewModelBase centralElement;

        #endregion Private Fields

        #region Public Constructors

        public MainPageViewModel()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public ViewModelBase CentralElement
        {
            get { return centralElement; }
            set
            {
                centralElement = value;
                OnPropertyChanged("CentralElement");
            }
        }

        #endregion Public Properties

        #region Command

        public ICommand AdminMediathequeView
        {
            get
            {
                if (this._showAdminMediathequeView == null)
                {
                    this._showAdminMediathequeView = new DelegateCommand(ShowMediathequeView, CanShowMediathequeView);
                }
                return this._showAdminMediathequeView;
            }
        }

        public ICommand AdminMemoryView
        {
            get
            {
                if (this._showAdminMemoryView == null)
                {
                    this._showAdminMemoryView = new DelegateCommand(ShowMemoryView, CanShowMemoryView);
                }
                return this._showAdminMemoryView;
            }
        }

        public ICommand AdminMenuView
        {
            get
            {
                if (this._showAdminMenuView == null)
                {
                    this._showAdminMenuView = new DelegateCommand(ShowMenuView, CanShowMenuView);
                }
                return this._showAdminMenuView;
            }
        }

        public ICommand AdminPuzzleView
        {
            get
            {
                if (this._showAdminPuzzleView == null)
                {
                    this._showAdminPuzzleView = new DelegateCommand(ShowPuzzleView, CanShowPuzzleView);
                }
                return this._showAdminPuzzleView;
            }
        }

        public ICommand AdminRegionView
        {
            get
            {
                if (this._showAdminRegionView == null)
                {
                    this._showAdminRegionView = new DelegateCommand(ShowAdminRegionView, CanShowAdminRegionView);
                }
                return this._showAdminRegionView;
            }
        }

        public ICommand AdminVisiteView
        {
            get
            {
                if (this._showAdminVisiteView == null)
                {
                    this._showAdminVisiteView = new DelegateCommand(ShowVisiteView, CanShowVisiteView);
                }
                return this._showAdminVisiteView;
            }
        }

        #endregion Command

        #region delegateCommand

        public bool CanShowAdminRegionView(object param)
        {
            return true;
        }

        public bool CanShowMediathequeView(object param)
        {
            return true;
        }

        public bool CanShowMemoryView(object param)
        {
            return true;
        }

        public bool CanShowMenuView(object param)
        {
            return true;
        }

        public bool CanShowPuzzleView(object param)
        {
            return true;
        }

        public bool CanShowStatistiqueView(object param)
        {
            return true;
        }

        public bool CanShowTestRomain(object param)
        {
            return true;
        }

        public bool CanShowVision360View(object param)
        {
            return true;
        }

        public bool CanShowVisiteView(object param)
        {
            return true;
        }

        public void ShowAdminRegionView(object param)
        {
            this.CentralElement = ServiceLocator.AdminRegionViewModel;
        }

        //ADMIN MEDIATHEQUE
        public void ShowMediathequeView(object param)
        {
            this.CentralElement = ServiceLocator.AdminMediathequeViewModel;
        }

        //ADMIN MEMORY
        public void ShowMemoryView(object param)
        {
            this.CentralElement = ServiceLocator.AdminMemoryViewModel;
        }

        //ADMIN MENU
        public void ShowMenuView(object param)
        {
            this.CentralElement = ServiceLocator.AdminMenuViewModel;
        }

        //ADMIN PUZZLE
        public void ShowPuzzleView(object param)
        {
            this.CentralElement = ServiceLocator.AdminPuzzleViewModel;
        }

        //ADMIN DEPARTEMENT
        public void ShowTestRomain(object param)
        {
            this.CentralElement = ServiceLocator.AdminRegionViewModel;
        }

        //ADMIN VISITE
        public void ShowVisiteView(object param)
        {
            this.CentralElement = ServiceLocator.AdminVisiteViewModel;
        }

        #endregion delegateCommand
    }
}