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
    /// DAO region handle every placeHolders for the app Region
    /// </summary>
    public class DAORegion : IDisposable
    {
        #region Private Fields

        private static DAORegion _instance;
        private static NotifyCollectionChangedEventHandler collectionChanged;
        private static string defaultPath = ConfigurationManager.AppSettings["cheminRegion"];
        private ObservableHashSet<Map> _maps;

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Construct default DAO Objet
        /// </summary>
        private DAORegion()
        {
            _maps = new ObservableHashSet<Map>();
            collectionChanged = new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            _maps.CollectionChanged += collectionChanged;
        }

        /// <summary>
        /// Construct the DAO with data contains in default path
        /// </summary>
        private DAORegion(string filename)
        {
            Load(filename);
        }

        #endregion Private Constructors

        #region Private Destructors

        ~DAORegion()
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
            foreach (Map map in DAORegion.Instance.findAllPlaceMaps())
            {
                placeholders.AddRange(map.PlaceHolders);
            }
            return placeholders;
        }

        public static void Refresh()
        {
            DAORegion.Instance.Load(defaultPath);
            DAORegion.Instance.Save(defaultPath);
        }

        /// <summary>
        /// Force save data
        /// </summary>
        public static void Save()
        {
            DAORegion.Instance.Save(defaultPath);
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
            DAORegion.Instance.Save(fileName);
        }

        private void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                CreateDefault(filename);
            }

            FileStream file = File.OpenRead(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(DAORegion));
            DAORegion other = (DAORegion)serializer.Deserialize(file);
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
                XmlSerializer serializer = new XmlSerializer(typeof(DAORegion));
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
        public static DAORegion Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DAORegion(defaultPath);
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