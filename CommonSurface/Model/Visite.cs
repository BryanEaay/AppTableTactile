using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Visite : INotifyPropertyChanged, IDisposable
    {
        #region Private Fields

        private string _cover;

        private int _id;

        private string _name;

        private ObservableCollection<Panorama> _panoramas;

        private string _source;

        private string _thumbnail;

        private string _title;

        private NotifyCollectionChangedEventHandler collectionChanged;

        #endregion Private Fields

        #region Public Constructors

        public Visite(int id, string name, string cover, string thumbnail, string title, string source)
        {
            this._id = id;
            this._name = name;
            this._cover = cover;
            this._thumbnail = thumbnail;
            this._title = title;
            this._source = source;
            this._panoramas = new ObservableCollection<Panorama>();
        }

        public Visite(Visite other)
        {
            this._id = other.ID;
            this._name = other.Name;
            this._cover = other.Cover;
            this._thumbnail = other.Thumbnail;
            this._title = other.Title;
            this._source = other.Source;
            this._panoramas = new ObservableCollection<Panorama>(other.Panoramas);
        }

        #endregion Public Constructors

        #region Private Constructors

        // ONLY SERIALIZATION
        private Visite()
        {
        }

        #endregion Private Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Methods

        public static Visite Blank()
        {
            return new Visite(-1, "", "", "", "", "");
        }

        public static Visite Blank(int id)
        {
            return new Visite(id, "", "", "", "", "");
        }

        public void Copy(Visite copy)
        {
            this.ID = copy.ID;
            this.Name = copy.Name;
            this.Cover = copy.Cover;
            this.Thumbnail = copy.Thumbnail;
            this.Title = copy.Title;
            this.Source = copy.Source;
            this.Panoramas = new ObservableCollection<Panorama>(copy.Panoramas);
        }

        public void Dispose()
        {
            foreach (Panorama p in _panoramas)
            {
                p.Dispose();
            }
            this._id = 0;
            this._name = null;
            this._cover = null;
            this._thumbnail = null;
            this._title = null;
            this._source = null;

            if (collectionChanged != null)
            {
                _panoramas.CollectionChanged -= collectionChanged;
            }

            this._panoramas.Clear();
            this._panoramas = null;
        }

        public override string ToString()
        {
            return "Visite[ID= " + ID + ", " +
                "Name=" + Name + ", " +
                "Title=" + Title + "]";
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion Protected Methods

        #region Private Methods

        private void _panorama_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Panoramas");
        }

        #endregion Private Methods

        #region GETTEUR/SETTEUR

        [XmlElement]
        public string Cover
        {
            get { return _cover; }
            set { _cover = value; OnPropertyChanged("Cover"); }
        }

        [XmlElement]
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

        [XmlArray("Panoramas")]
        [XmlArrayItem("Panorama")]
        public ObservableCollection<Panorama> Panoramas
        {
            get { return _panoramas; }

            set
            {
                _panoramas = value;
                collectionChanged = new NotifyCollectionChangedEventHandler(_panorama_CollectionChanged);
                _panoramas.CollectionChanged += collectionChanged;
                OnPropertyChanged("Panoramas");
            }
        }

        [XmlElement]
        public string Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged("Source"); }
        }

        [XmlElement]
        public string Thumbnail
        {
            get { return _thumbnail; }
            set { _thumbnail = value; OnPropertyChanged("Thumbnail"); }
        }

        [XmlElement]
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        #endregion GETTEUR/SETTEUR
    }
}