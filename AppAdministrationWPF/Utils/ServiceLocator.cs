using AppAdministrationWPF.ViewModel;

namespace AppAdministrationWPF.Utils
{
    public static class ServiceLocator
    {
        #region Private Fields

        private static AdminMediathequeViewModel _adminMediathequeViewModel;
        private static AdminMemoryViewModel _adminMemoryViewModel;
        private static AdminMenuViewModel _adminMenuViewModel;
        private static AdminPuzzleViewModel _adminPuzzleViewModel;
        private static AdminRegionViewModel _adminRegionViewModel;
        private static AdminVisiteViewModel _adminVisiteViewModel;
        private static MainPageViewModel _mainPageViewModel;

        #endregion Private Fields

        #region Public Properties

        public static AdminMediathequeViewModel AdminMediathequeViewModel
        {
            get
            {
                if (_adminMediathequeViewModel == null) _adminMediathequeViewModel = new AdminMediathequeViewModel();
                return ServiceLocator._adminMediathequeViewModel;
            }
        }

        public static AdminMemoryViewModel AdminMemoryViewModel
        {
            get
            {
                if (_adminMemoryViewModel == null) _adminMemoryViewModel = new AdminMemoryViewModel();
                return ServiceLocator._adminMemoryViewModel;
            }
        }

        public static AdminMenuViewModel AdminMenuViewModel
        {
            get
            {
                if (_adminMenuViewModel == null) _adminMenuViewModel = new AdminMenuViewModel();
                return ServiceLocator._adminMenuViewModel;
            }
        }

        public static AdminPuzzleViewModel AdminPuzzleViewModel
        {
            get
            {
                if (_adminPuzzleViewModel == null) _adminPuzzleViewModel = new AdminPuzzleViewModel();
                return ServiceLocator._adminPuzzleViewModel;
            }
        }

        public static AdminRegionViewModel AdminRegionViewModel
        {
            get
            {
                if (_adminRegionViewModel == null) _adminRegionViewModel = new AdminRegionViewModel();
                return ServiceLocator._adminRegionViewModel;
            }
        }

        public static AdminVisiteViewModel AdminVisiteViewModel
        {
            get
            {
                if (_adminVisiteViewModel == null) _adminVisiteViewModel = new AdminVisiteViewModel();
                return ServiceLocator._adminVisiteViewModel;
            }
        }

        public static MainPageViewModel MainPageViewModel
        {
            get
            {
                if (_mainPageViewModel == null) _mainPageViewModel = new MainPageViewModel();
                return ServiceLocator._mainPageViewModel;
            }
        }

        #endregion Public Properties
    }
}