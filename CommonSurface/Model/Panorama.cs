using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Panorama : INotifyPropertyChanged, IDisposable
    {
        #region Private Fields

        private string _copyright;

        private string _description;

        private string _media;

        private string _scene;

        private string _title;

        #endregion Private Fields

        #region Public Constructors

        // DO NOT USE
        public Panorama() { }

        public Panorama(string title, string scene, string description, string media, string copyright)
        {
            this._title = title;
            this._scene = scene;
            this._description = description;
            this._media = media;
            this._copyright = copyright;
        }

        public Panorama(Panorama other)
        {
            this._title = other.Title;
            this._scene = other.Scene;
            this._description = other.Description;
            this._media = other.Media;
            this._copyright = other.Copyright;
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Methods

        public static Panorama Blank()
        {
            return new Panorama("", "", "", "", "");
        }

        public static Panorama Blank(string scene)
        {
            return new Panorama("", scene, "", "", "");
        }

        public void Copy(Panorama copy)
        {
            this.Title = copy.Title;
            this.Scene = copy.Scene;
            this.Description = copy.Description;
            this.Media = copy.Media;
            this.Copyright = copy.Copyright;
        }

        public void Dispose()
        {
            this._title = null;
            this._scene = null;
            this._description = null;
            this._media = null;
            this._copyright = null;
        }

        public override string ToString()
        {
            return $"Panorama [Name: {this.Name}]";
        }

        #endregion Public Methods

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

        #region GETTEUR/SETTEUR

        [XmlElement]
        public string Copyright
        {
            get { return this._copyright; }
            set { this._copyright = value; OnPropertyChanged("Copyright"); }
        }

        [XmlElement]
        public string Description
        {
            get { return this._description; }
            set { this._description = value; OnPropertyChanged("Description"); }
        }

        [XmlElement]
        public string Media
        {
            get { return this._media; }
            set { this._media = value; OnPropertyChanged("Media"); }
        }

        public string Name
        {
            get
            {
                if (this._title != null)
                {
                    if (this._title.Length > 0)
                    {
                        return this._title;
                    }
                }

                return this._scene;
            }
        }

        [XmlAttribute]
        public string Scene
        {
            get { return this._scene; }
            set { this._scene = value; OnPropertyChanged("Scene"); }
        }

        [XmlElement]
        public string Title
        {
            get { return this._title; }
            set { this._title = value; OnPropertyChanged("Title"); }
        }

        #endregion GETTEUR/SETTEUR
    }
}