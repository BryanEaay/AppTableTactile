using System.ComponentModel;

namespace CommonSurface.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Public Delegates

        public delegate void OnCloseViewModelEventHandler();

        #endregion Public Delegates

        #region Public Events

        public event OnCloseViewModelEventHandler OnCloseViewModelEvent;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Protected Methods

        protected void Close()
        {
            OnCloseViewModelEvent?.Invoke();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion Protected Methods
    }
}