using CommonSurface.Model;
using CommonSurface.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace CommonSurface.DAO
{
    public class DAOMemory : IDisposable
    {
        #region Private Fields

        private static DAOMemory _instance;

        private static string defaultPath = ConfigurationManager.AppSettings["cheminMemory"];
        private ObservableHashSet<Picture> _backgrounds;
        private ObservableHashSet<Difficulty> _difficulties;

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Construct default DAO Objet
        /// </summary>
        private DAOMemory()
        {
            _backgrounds = new ObservableHashSet<Picture>();
            _difficulties = new ObservableHashSet<Difficulty>();
        }

        /// <summary>
        /// Construct the DAO with data contains in default path
        /// </summary>
        private DAOMemory(string filename)
        {
            Load(filename);
        }

        #endregion Private Constructors

        #region Private Destructors

        ~DAOMemory()
        {
        }

        #endregion Private Destructors

        #region Public Methods

        public static void Refresh()
        {
            DAOMemory.Instance.Load(defaultPath);
            DAOMemory.Instance.Save(defaultPath);
        }

        /// <summary>
        /// Force save data
        /// </summary>
        public static void Save()
        {
            DAOMemory.Instance.Save(defaultPath);
        }

        /// <summary>
        /// Get the list of available backgrounds
        /// </summary>
        /// <returns></returns>
        [Obsolete("Do not use anymore, use Backgrounds Property instead", true)]
        public List<string> FindAllBackgrounds()
        {
            return new List<string>();
        }

        /// <summary>
        /// Find cards for the given game id <br />
        /// note: list not observated
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public List<Picture> FindCardsForGame(int gameId)
        {
            List<Picture> cs = new List<Picture>();
            return cs;
        }

        public ObservableCollection<Picture> FindPictures(string type)
        {
            foreach (var difficulty in DAOMemory.Instance.Difficulties)
            {
                if (difficulty.Type == type)
                {
                    return difficulty.Pictures;
                }
            }

            return null;
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
                throw new FileNotFoundException();
            }

            FileStream file = File.OpenRead(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(DAOMemory));
            DAOMemory other = (DAOMemory)serializer.Deserialize(file);
            file.Close();

            // copying items
            UpdateWith(other);
        }

        /// <summary>
        /// Save data to the given filename
        /// </summary>
        /// <param name="filename"></param>
        private void Save(string filename)
        {
            try
            {
                FileStream file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                file.SetLength(0);
                XmlSerializer serializer = new XmlSerializer(typeof(DAOMemory));
                serializer.Serialize(file, this);
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to save: " + e.Message);
                //case of concurrent call, the newt call will write it
            }
        }

        /// <summary>
        /// Update data with an other DAOMemory <br /> Usually called from Load methods
        /// </summary>
        /// <param name="other"></param>
        private void UpdateWith(DAOMemory other)
        {
            Difficulties = other.Difficulties;
            Backgrounds = other.Backgrounds;
        }

        #endregion Private Methods

        #region Events

        public void Dispose()
        {
            _backgrounds.Clear();
            _backgrounds = null;
            _difficulties.Clear();
            _difficulties = null;
            _instance = null;
        }

        private void cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Save(defaultPath);
        }

        #endregion Events

        #region PROPERTIES

        /// <summary>
        /// </summary>
        public ObservableHashSet<Picture> Backgrounds
        {
            get { return _backgrounds; }
            set { _backgrounds = value; }
        }

        /// <summary>
        /// </summary>
        public ObservableHashSet<Difficulty> Difficulties
        {
            get { return _difficulties; }
            set { _difficulties = value; }
        }

        #endregion PROPERTIES

        #region STATIC

        /// <summary>
        /// Get the single instance
        /// </summary>
        public static DAOMemory Instance
        {
            get
            {
                if (_instance == null) { _instance = new DAOMemory(defaultPath); }
                return _instance;
            }
        }

        #endregion STATIC
    }
}