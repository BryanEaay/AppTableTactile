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
        private static AdminFriseViewModel _adminFriseViewModel;
        private static AdminBanqueImagesViewModel _adminBanqueImagesViewModel;
        private static AdminVisiteViewModel _adminVisiteViewModel;
        private static MainPageViewModel _mainPageViewModel;

        #endregion Private Fields

        #region Public Properties

        public static AdminMediathequeViewModel AdminMediathequeViewModel
        {
            get
            {
                if (_adminMediathequeViewModel == null) _adminMediathequeViewModel = new AdminMediathequeViewModel();
                return _adminMediathequeViewModel;
            }
        }

        public static AdminMemoryViewModel AdminMemoryViewModel
        {
            get
            {
                if (_adminMemoryViewModel == null) _adminMemoryViewModel = new AdminMemoryViewModel();
                return _adminMemoryViewModel;
            }
        }

        public static AdminMenuViewModel AdminMenuViewModel
        {
            get
            {
                if (_adminMenuViewModel == null) _adminMenuViewModel = new AdminMenuViewModel();
                return _adminMenuViewModel;
            }
        }

        public static AdminPuzzleViewModel AdminPuzzleViewModel
        {
            get
            {
                if (_adminPuzzleViewModel == null) _adminPuzzleViewModel = new AdminPuzzleViewModel();
                return _adminPuzzleViewModel;
            }
        }

        public static AdminRegionViewModel AdminRegionViewModel
        {
            get
            {
                if (_adminRegionViewModel == null) _adminRegionViewModel = new AdminRegionViewModel();
                return _adminRegionViewModel;
            }
        }

        public static AdminFriseViewModel AdminFriseViewModel
        {
            get
            {
                if (_adminFriseViewModel == null) _adminFriseViewModel = new AdminFriseViewModel();
                return _adminFriseViewModel;
            }
        }

        public static AdminBanqueImagesViewModel AdminBanqueImagesViewModel
        {
            get
            {
                if (_adminBanqueImagesViewModel == null) _adminBanqueImagesViewModel = new AdminBanqueImagesViewModel();
                return _adminBanqueImagesViewModel;
            }
        }

        public static AdminVisiteViewModel AdminVisiteViewModel
        {
            get
            {
                if (_adminVisiteViewModel == null) _adminVisiteViewModel = new AdminVisiteViewModel();
                return _adminVisiteViewModel;
            }
        }

        public static MainPageViewModel MainPageViewModel
        {
            get
            {
                if (_mainPageViewModel == null) _mainPageViewModel = new MainPageViewModel();
                return _mainPageViewModel;
            }
        }

        #endregion Public Properties
    }
}