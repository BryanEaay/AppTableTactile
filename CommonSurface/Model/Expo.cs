using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    public class Expo : INotifyPropertyChanged
    {
        #region Private Fields

        private ObservableCollection<PlaceHolder> _holder;

        #endregion Private Fields

        #region Public Constructors

        public Expo()
        {
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        [XmlArray(ElementName = "MediaCollection")]
        public ObservableCollection<PlaceHolder> holder
        {
            get { return _holder; }
            set
            {
                _holder = value;
                _holder.CollectionChanged += new NotifyCollectionChangedEventHandler(_holder_CollectionChanged);
                OnPropertyChanged("holder");
            }
        }

        #endregion Public Properties

        #region Protected Methods

        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void _holder_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("holder");
        }

        #endregion Private Methods
    }
}