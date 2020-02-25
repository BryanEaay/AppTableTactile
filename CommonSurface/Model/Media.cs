using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Media : INotifyPropertyChanged, IDisposable
    {
        #region Private Fields

        private string _name;

        private string _path;

        private Section _section;

        private MediaType _type;

        private double _x;

        private double _y;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Serialization constructor DO NOT USE!
        /// </summary>
        public Media()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copy"></param>
        public Media(Media copy)
        {
            this.X = copy.X;
            this.Y = copy.Y;
            this.Name = copy.Name;
            this.Type = copy.Type;
            this.Section = copy.Section;
            this.Path = copy.Path;
        }

        public Media(string Name, string Path)
        {
            this.X = 0;
            this.Y = 0;
            this.Name = Name;
            this.Type = MediaType.DEFAUT;
            this.Section = Section.DEFAUT;
            this.Path = Path;
        }

        public Media(string Name, string Path, MediaType Type)
        {
            this.X = 0;
            this.Y = 0;
            this.Name = Name;
            this.Type = Type;
            this.Section = Section.DEFAUT;
            this.Path = Path;
        }

        public Media(double X, double Y, string Name, MediaType Type, Section Section)
        {
            this.X = X;
            this.Y = Y;
            this.Name = Name;
            this.Type = Type;
            this.Section = Section;
            this.Path = "";
        }

        public Media(double X, double Y, string Name, string Path, MediaType Type, Section Section)
        {
            this.X = X;
            this.Y = Y;
            this.Name = Name;
            this.Type = Type;
            this.Section = Section;
            this.Path = Path;
        }

        #endregion Public Constructors

        #region Private Destructors

        ~Media()
        {
            this._x = 0;
            this._y = 0;
            this._name = null;
            this._path = null;
        }

        #endregion Private Destructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region getters and setters

        [XmlElement]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        [XmlElement]
        public string Path
        {
            get { return _path; }
            set { _path = value; OnPropertyChanged("Path"); }
        }

        [XmlElement]
        public Section Section
        {
            get { return _section; }
            set { _section = value; OnPropertyChanged("Section"); }
        }

        [XmlElement]
        public MediaType Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        [XmlElement]
        public double X
        {
            get { return _x; }
            set { _x = value; OnPropertyChanged("X"); }
        }

        [XmlElement]
        public double Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged("Y"); }
        }

        #endregion getters and setters

        #region Public Methods

        /// <summary>
        /// Create a blank media
        /// </summary>
        /// <returns></returns>
        public static Media Blank()
        {
            return new Media(0, 0, "", "", MediaType.DEFAUT, Section.DEFAUT);
        }

        public void Dispose()
        {
            this._x = 0;
            this._y = 0;
            this._name = null;
            this._path = null;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Media))
                return false;
            Media other = (Media)obj;

            if (other._x != _x)
                return false;
            else if (other._y != _y)
                return false;
            else if (other._path != _path)
                return false;
            else if (other._name != _name)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "Media[X=" + this.X + ", " +
                "Y=" + this.Y + ", " +
                "Nom=" + this.Name + ", " +
                "Type=" + this.Type + ", " +
                "Section=" + this.Section + ", " +
                "Path" + this.Path +
                "]";
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