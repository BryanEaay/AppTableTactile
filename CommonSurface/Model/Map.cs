using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Map : INotifyPropertyChanged, IDisposable
    {
        #region Private Fields

        private string _background;

        private int _id;

        private string _name;

        private ObservableCollection<PlaceHolder> _placeholders;

        #endregion Private Fields

        #region Public Constructors

        // DO NOT USE
        public Map()
        {
        }

        public Map(int id, string name, string background)
        {
            this._id = id;
            this._name = name;
            this._background = background;
            this._placeholders = new ObservableCollection<PlaceHolder>();
        }

        public Map(Map other)
        {
            this._id = other.ID;
            this._name = other.Name;
            this._background = other.Background;
            this._placeholders = new ObservableCollection<PlaceHolder>(other.PlaceHolders);
        }

        #endregion Public Constructors

        #region Private Destructors

        ~Map()
        {
            this._id = 0;
            this._name = null;
            this._background = null;
            this._placeholders = null;
        }

        #endregion Private Destructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Methods

        public static Map Blank()
        {
            return new Map(-1, "", "");
        }

        public static Map Blank(int id)
        {
            return new Map(id, "", "");
        }

        public void Copy(Map copy)
        {
            this._id = copy.ID;
            this._name = copy.Name;
            this._background = copy.Background;
            this._placeholders = new ObservableCollection<PlaceHolder>(copy.PlaceHolders);
        }

        public void Dispose()
        {
            this._placeholders.CollectionChanged -= _placeholder_CollectionChanged;
            foreach (PlaceHolder p in this._placeholders)
            {
                p.Dispose();
            }
            this._placeholders = null;
        }

        public override string ToString()
        {
            return "Visite[ID= " + ID + ", " +
                "Name=" + Name + "]";
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion Protected Methods

        #region Private Methods

        private void _placeholder_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("PlaceHolders");
        }

        #endregion Private Methods

        #region GETTEUR/SETTEUR

        [XmlElement]
        public string Background
        {
            get { return _background; }
            set { _background = value; OnPropertyChanged("Background"); }
        }

        [XmlAttribute]
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        [XmlArray("PlaceHolders")]
        [XmlArrayItem("PlaceHolder")]
        public ObservableCollection<PlaceHolder> PlaceHolders
        {
            get { return _placeholders; }

            set
            {
                _placeholders = value;
                _placeholders.CollectionChanged += _placeholder_CollectionChanged;
                OnPropertyChanged("Panoramas");
            }
        }

        #endregion GETTEUR/SETTEUR
    }
}