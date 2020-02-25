using CommonSurface.Model;
using CommonSurface.ViewModel;

namespace AppAdministrationWPF.ViewModel
{
    public class MediaDialogModel : ViewModelBase
    {
        #region Private Fields

        private Media _media;

        #endregion Private Fields

        #region Public Constructors

        public MediaDialogModel(Media media)
        {
            Selected = media;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name
        {
            get { return _media.Name; }
            set { _media.Name = value; OnPropertyChanged("Name"); }
        }

        public string Path
        {
            get { return _media.Path; }
            set { _media.Path = value; OnPropertyChanged("Path"); }
        }

        /// <summary>
        /// Set a copy of the given media or a Media.Blank() value
        /// </summary>
        public Media Selected
        {
            get { return _media; }
            set { _media = (value == null) ? Media.Blank() : new Media(value); OnPropertyChanged("Selected"); }
        }

        #endregion Public Properties
    }
}