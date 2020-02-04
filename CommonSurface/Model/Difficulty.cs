using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Difficulty : INotifyPropertyChanged
    {
        #region Private Fields

        private ObservableCollection<Picture> _pictures;
        private int _size;
        private string _type;

        #endregion Private Fields

        #region Public Constructors

        public Difficulty(int size, string type)
        {
            _size = size;
            _type = type;
        }

        #endregion Public Constructors

        #region Private Constructors

        private Difficulty()
        {
        }

        #endregion Private Constructors

        #region Private Destructors

        ~Difficulty()
        {
            _size = 0;
            _type = null;
            _pictures.Clear();
            _pictures = null;
        }

        #endregion Private Destructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Getter & Setter

        [XmlArray("Pictures")]
        [XmlArrayItem("Picture")]
        public ObservableCollection<Picture> Pictures
        {
            get { return _pictures; }
            set { _pictures = value; OnPropertyChanged("Pictures"); }
        }

        [XmlAttribute]
        public int Size
        {
            get { return _size; }
            set { _size = value; OnPropertyChanged("Size"); }
        }

        [XmlAttribute]
        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        #endregion Getter & Setter

        #region Public Methods

        public static Difficulty Blank()
        {
            return new Difficulty(-1, "");
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion Protected Methods
    }
}