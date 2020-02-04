using CommonSurface.Model;
using CommonSurface.ViewModel;

namespace AppAdministrationWPF.ViewModel
{
    internal class PlaceHolderWindowModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Selected placeholder
        /// </summary>
        private PlaceHolder _placeholder;

        private Section section;

        #endregion Private Fields

        #region Public Constructors

        public PlaceHolderWindowModel(PlaceHolder place)
        {
            Selected = place;
        }

        public PlaceHolderWindowModel(PlaceHolder place, Section section)
        {
            Selected = place;
            this.section = section;
        }

        #endregion Public Constructors

        #region Public Properties

        public string path
        {
            get { return _placeholder.IconPath; }
            set { _placeholder.IconPath = value; OnPropertyChanged("pathIcon"); }
        }

        /// <summary>
        /// Set a copy
        /// </summary>
        public PlaceHolder Selected
        {
            get { return _placeholder; }
            set
            {
                if (value != null)
                {
                    value.Section = section;
                    _placeholder = new PlaceHolder(value);
                }
                else
                {
                    _placeholder = PlaceHolder.Blank(section);
                }
                OnPropertyChanged("Selected");
            }
        }

        #endregion Public Properties
    }
}