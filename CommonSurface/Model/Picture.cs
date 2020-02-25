using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Picture : INotifyPropertyChanged
    {
        #region Private Fields

        private string _name;
        private string _source;

        #endregion Private Fields

        #region Public Constructors

        public Picture(string name, string source)
        {
            _name = name;
            _source = source;
        }

        public Picture(Picture other)
        {
            _name = other.Name;
            _source = other.Source;
        }

        #endregion Public Constructors

        #region Private Constructors

        private Picture()
        {
        }

        #endregion Private Constructors

        #region Private Destructors

        ~Picture()
        {
            _name = _source = null;
        }

        #endregion Private Destructors

        #region Getter & Setter

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        [XmlAttribute]
        public string Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged("Source"); }
        }

        #endregion Getter & Setter

        #region Public Methods

        public static Picture Blank()
        {
            return new Picture("", "");
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