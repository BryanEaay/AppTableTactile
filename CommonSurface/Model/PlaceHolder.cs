using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    /// <summary>
    /// I don't care of french words. <br /> A place holder is displayed on regional map and contains
    /// multiple media. <br /> it has a name and it represent a place. (or at least, should be...)
    /// </summary>
    [Serializable]
    public class PlaceHolder : INotifyPropertyChanged, IDisposable
    {
        #region Public Constructors

        /// <summary>
        /// Serialization constructor DO NOT USE!
        /// </summary>
        public PlaceHolder() { }

        public PlaceHolder(int id, string name, int x, int y)
        {
            _id = id;
            _name = name;
            _x = x;
            _y = y;
            _iconPath = null;
            Media = new ObservableCollection<Media>();
        }

        public PlaceHolder(int id, string name, int x, int y, Section owner)
        {
            _id = id;
            _name = name;
            _x = x;
            _y = y;
            _iconPath = null;
            Media = new ObservableCollection<Media>();
            _section = owner;
        }

        public PlaceHolder(int id, string name, int x, int y, string path)
        {
            _id = id;
            _name = name;
            _x = x;
            _y = y;
            _iconPath = path;
            Media = new ObservableCollection<Media>();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="copy"></param>
        public PlaceHolder(PlaceHolder copy)
        {
            Id = copy.Id;
            Name = copy.Name;
            X = copy.X;
            Y = copy.Y;
            IconPath = copy.IconPath;
            Media = new ObservableCollection<Media>(copy.Media);
            _section = copy.Section;
        }

        #endregion Public Constructors

        #region Private Destructors

        ~PlaceHolder()
        {
        }

        #endregion Private Destructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region properties

        private string _iconPath;
        private int _id;
        private bool _isSelected;
        private ObservableCollection<Media> _media;
        private string _name;
        private Section _section = Section.DEFAUT;

        private int _x;
        private int _y;

        #endregion properties

        #region Public Methods

        /// <summary>
        /// get a blank placeholder
        /// </summary>
        /// <returns></returns>
        public static PlaceHolder Blank()
        {
            return new PlaceHolder(-1, "", 10, 10);
        }

        public static PlaceHolder Blank(Section section)
        {
            return new PlaceHolder(-1, "", 10, 10, section);
        }

        public void Dispose()
        {
            _id = 0;
            _name = null;
            _x = 0;
            _y = 0;
            _iconPath = null;
            foreach (Media m in _media)
            {
                m.Dispose();
            }
            _media = null;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PlaceHolder))
                return false;

            PlaceHolder other = (PlaceHolder)obj;

            if (other._id != _id)
                return false;
            if (other._name != _name)
                return false;

            if (other._x != _x)
                return false;
            if (other._y != _y)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "PlaceHolder[id= " + Id + ", " +
                "Name=" + Name + ", " +
                "X=" + X + ", " +
                "Y=" + Y + "]";
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// used to fire change event
        /// </summary>
        /// <param name="info"></param>
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion Protected Methods

        #region Private Methods

        private void _media_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Media");
        }

        #endregion Private Methods

        #region getters and setters

        [XmlElement]
        public string IconPath
        {
            get { return _iconPath == null ? "" : _iconPath; }
            set { _iconPath = value; OnPropertyChanged("IconPath"); }
        }

        [XmlAttribute]
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        [XmlElement]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        [XmlArray("MediaCollection")]
        [XmlArrayItem("Media")]
        public ObservableCollection<Media> Media
        {
            get { return _media; }
            set
            {
                _media = value;
                _media.CollectionChanged += new NotifyCollectionChangedEventHandler(_media_CollectionChanged);
                OnPropertyChanged("Media");
            }
        }

        [XmlAttribute]
        public String Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        [XmlAttribute]
        public Section Section
        {
            get { return _section; }
            set { _section = value; OnPropertyChanged("Section"); }
        }

        [XmlElement]
        public int X
        {
            get { return _x; }
            set { _x = value; OnPropertyChanged("X"); }
        }

        [XmlElement]
        public int Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged("Y"); }
        }

        #endregion getters and setters
    }
}