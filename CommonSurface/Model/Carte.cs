using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace CommonSurface.Model
{
    [Serializable]
    public class Carte : INotifyPropertyChanged
    {
        #region Protected Fields

        protected string _chemin;

        #endregion Protected Fields

        #region Private Fields

        private int _gameId;

        private int _id;

        private string _nom;

        #endregion Private Fields

        #region Public Constructors

        public Carte(int id, string chemin, string nom, int gameId)
        {
            this.Id = id;
            this.Chemin = chemin;
            this.Nom = nom;
            this.GameId = gameId;
        }

        public Carte(int id, string chemin, string nom)
        {
            this.Id = id;
            this.Chemin = chemin;
            this.Nom = nom;
            this.GameId = -1;
        }

        /// <summary>
        /// Copy ctor
        /// </summary>
        /// <param name="other"></param>
        public Carte(Carte other)
        {
            Id = other.Id;
            GameId = other.GameId;
            Chemin = other.Chemin;
            Nom = other.Nom;
        }

        #endregion Public Constructors

        #region Private Constructors

        /// <summary>
        /// Serialisation only!
        /// </summary>
        private Carte()
        {
        }

        #endregion Private Constructors

        #region Private Destructors

        ~Carte()
        {
            _id = _gameId = 0;
            _chemin = _nom = null;
        }

        #endregion Private Destructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        [XmlAttribute]
        public string Chemin
        {
            get { return _chemin; }
            set { _chemin = value; OnPropertyChanged("Chemin"); }
        }

        [XmlAttribute]
        public int GameId
        {
            get { return _gameId; }
            set { _gameId = value; OnPropertyChanged("GameId"); }
        }

        [XmlAttribute]
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        [XmlAttribute]
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged("Nom"); }
        }

        public static BitmapImage Source { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public override string ToString()
        {
            return "[Carte Id=" + Id + ", Nom=" + Nom + ", GameId=" + GameId + ", Chemin=" + Chemin + "]";
        }

        #endregion Public Methods
    }
}