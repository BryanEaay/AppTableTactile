using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Credits : INotifyPropertyChanged
    {
        #region Private Fields

        private Icon _icon;

        private string _source;

        #endregion Private Fields

        #region Public Constructors

        public Credits(Icon icon, string source)
        {
            this._icon = icon;
            this._source = source;
        }

        #endregion Public Constructors

        #region Private Constructors

        private Credits()
        {
        }

        #endregion Private Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Getter & Setter

        [XmlElement("Icon")]
        public Icon Icon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged("Icon"); }
        }

        [XmlElement("Source")]
        public string Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged("Source"); }
        }

        #endregion Getter & Setter

        #region Public Methods

        public void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public override string ToString()
        {
            return "[Credits Source=" + _source + "]";
        }

        #endregion Public Methods
    }
}