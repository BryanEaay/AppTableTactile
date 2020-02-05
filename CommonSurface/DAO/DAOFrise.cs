using CommonSurface.Model;
using CommonSurface.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace CommonSurface.DAO
{
    /// <summary>
    /// DAO Frise handle every placeHolders for the app Frise
    /// </summary>
    public class DAOFrise : IDisposable
    {
        #region Private Fields

        private static DAOFrise _instance;
        private static NotifyCollectionChangedEventHandler collectionChanged;
        private static string defaultPath = ConfigurationManager.AppSettings["cheminFrise"];
        private ObservableHashSet<Map> _maps;

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Construct default DAO Objet
        /// </summary>
        private DAOFrise()
        {
            _maps = new ObservableHashSet<Map>();
            collectionChanged = new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            _maps.CollectionChanged += collectionChanged;
        }

        /// <summary>
        /// Construct the DAO with data contains in default path
        /// </summary>
        private DAOFrise(string filename)
        {
            Load(filename);
        }

        #endregion Private Constructors

        #region Private Destructors

        ~DAOFrise()
        {
            if (collectionChanged != null)
            {
                _maps.CollectionChanged -= collectionChanged;
                collectionChanged = null;
            }
            _instance = null;
        }

        #endregion Private Destructors

        #region Public Methods

        /// <summary>
        /// Find all media in every placeholders
        /// </summary>
        /// <returns></returns>
        public static List<PlaceHolder> findAllPlaceholders()
        {
            List<PlaceHolder> placeholders = new List<PlaceHolder>();
            foreach (Map map in DAOFrise.Instance.findAllPlaceMaps())
            {
                placeholders.AddRange(map.PlaceHolders);
            }
            return placeholders;
        }

        public static void Refresh()
        {
            DAOFrise.Instance.Load(defaultPath);
            DAOFrise.Instance.Save(defaultPath);
        }

        /// <summary>
        /// Force save data
        /// </summary>
        public static void Save()
        {
            DAOFrise.Instance.Save(defaultPath);
        }

        public void Dispose()
        {
            _maps.CollectionChanged -= collectionChanged;
            collectionChanged = null;

            foreach (Map m in _maps)
            {
                m.Dispose();
            }
            _maps.Clear();
            _maps = null;
            _instance = null;
        }

        public List<Map> findAllPlaceMaps()
        {
            return new List<Map>(Instance.Maps);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Create a default object and save it the given path
        /// </summary>
        /// <param name="fileName"></param>
        private void CreateDefault(string fileName)
        {
            DAOFrise.Instance.Save(fileName);
        }

        private void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                CreateDefault(filename);
            }

            FileStream file = File.OpenRead(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(DAOFrise));
            DAOFrise other = (DAOFrise)serializer.Deserialize(file);
            file.Close();

            // copying items
            _maps = other._maps;
        }

        /// <summary>
        /// Handle change on the place holder collection
        /// </summary>
        /// <param name="sender"></param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Save(defaultPath);
        }

        private void Save(string filename)
        {
            try
            {
                FileStream file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                file.SetLength(0);
                XmlSerializer serializer = new XmlSerializer(typeof(DAOFrise));
                serializer.Serialize(file, this);
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to save: " + e.Message);
            }
        }

        #endregion Private Methods

        #region getters and setters

        /// <summary>
        /// Get the single instance
        /// </summary>
        public static DAOFrise Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DAOFrise(defaultPath);
                }
                return _instance;
            }
        }

        /// <summary>
        /// Get the place holders collection
        /// </summary>
        public ObservableCollection<Map> Maps
        {
            get { return _maps; }
        }

        #endregion getters and setters
    }
}