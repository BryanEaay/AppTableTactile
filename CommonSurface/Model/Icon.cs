using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Icon : INotifyPropertyChanged
    {
        #region Private Fields

        private string _color;

        private int _id;

        private string _name;

        private string _source;

        private string _text;

        private bool _visibility;

        private int _x;

        private int _y;

        #endregion Private Fields

        #region Public Constructors

        public Icon(int id, int x, int y, string source)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Source = source;
            this.Visibility = true;
        }

        public Icon(int id, string name, int x, int y, string source, string text, string color, bool visibility)
        {
            this.Id = id;
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Source = source;
            this.Text = text;
            this.Color = color;
            this.Visibility = visibility;
        }

        /// <summary>
        /// Copy icon
        /// </summary>
        /// <param name="other"></param>
        public Icon(Icon other)
        {
            this.Id = other.Id;
            this.Name = other.Name;
            this.X = other.X;
            this.Y = other.Y;
            this.Source = other.Source;
            this.Text = other.Text;
            this.Color = other.Color;
            this.Visibility = other.Visibility;
        }

        #endregion Public Constructors

        #region Private Constructors

        /// <summary>
        /// Serialisation only!
        /// </summary>
        private Icon() { }

        #endregion Private Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Getter & Setter

        [XmlElement]
        public string Color
        {
            get { return _color; }
            set { _color = value; OnPropertyChanged("Color"); }
        }

        [XmlAttribute]
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        [XmlElement]
        public string Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged("Source"); }
        }

        [XmlElement]
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }

        [XmlElement]
        public bool Visibility
        {
            get { return _visibility; }
            set { _visibility = value; OnPropertyChanged("Visibility"); }
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

        #endregion Getter & Setter

        #region Public Methods

        public void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public override string ToString()
        {
            return "[Carte Id=" + Id.ToString() +
                   ", Name=" + Name +
                   ", X=" + X.ToString() +
                   ", Y=" + Y.ToString() +
                   ", Source=" + Source +
                   ", Visibility=" + Visibility.ToString() + "]";
        }

        #endregion Public Methods
    }
}