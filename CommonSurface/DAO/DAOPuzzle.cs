using CommonSurface.Model;
using CommonSurface.Utils;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace CommonSurface.DAO
{
    public class DAOPuzzle
    {
        #region Private Fields

        private static DAOPuzzle _instance;
        private static string defaultPath = ConfigurationManager.AppSettings["cheminPuzzle"];
        private ObservableHashSet<Difficulty> _difficulties;

        #endregion Private Fields

        #region Private Constructors

        private DAOPuzzle()
        {
            _difficulties = new ObservableHashSet<Difficulty>();
        }

        private DAOPuzzle(string filename)
        {
            Load(defaultPath);
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Récupération de la liste des puzzles à la difficulté demandée
        /// </summary>
        /// <param name="type">Difficulté des puzzles</param>
        /// <returns>Liste des puzzles de la difficulté</returns>
        public ObservableCollection<Picture> FindPictures(string type)
        {
            foreach (var difficulty in DAOPuzzle.Instance.Difficulties)
            {
                if (difficulty.Type == type)
                {
                    return difficulty.Pictures;
                }
            }

            return null;
        }

        public void Refresh()
        {
            Load(defaultPath);
            Save(defaultPath);
        }

        /// <summary>
        /// Sauvegarde manuelle
        /// </summary>
        public void Update()
        {
            Save(defaultPath);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load data from a file <br /> Create default if the file doesn't exists
        /// </summary>
        /// <param name="filename"></param>
        private void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileLoadException();
            }

            FileStream file = File.OpenRead(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(DAOPuzzle));
            DAOPuzzle other = (DAOPuzzle)serializer.Deserialize(file);
            file.Close();

            // copying items
            _difficulties = other.Difficulties;
        }

        /// <summary>
        /// Sauvegarde des modifications dans le fichier
        /// </summary>
        /// <param name="filename"></param>
        private void Save(string filename)
        {
            try
            {
                FileStream file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                file.SetLength(0);
                XmlSerializer serializer = new XmlSerializer(typeof(DAOPuzzle));
                serializer.Serialize(file, this);
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to save: " + e.Message);
            }
        }

        #endregion Private Methods

        #region Getter / Setter

        /// <summary>
        /// Singleton
        /// </summary>
        public static DAOPuzzle Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DAOPuzzle(defaultPath);
                }
                return _instance;
            }
        }

        public ObservableHashSet<Difficulty> Difficulties
        {
            get { return _difficulties; }
            set { _difficulties = value; }
        }

        #endregion Getter / Setter
    }
}