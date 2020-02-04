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
    public class DAOVisite : IDisposable
    {
        #region Private Fields

        /// <summary>
        /// single instance
        /// </summary>
        private static DAOVisite _instance;

        private static NotifyCollectionChangedEventHandler collectionChanged;
        private static string defaultPath = ConfigurationManager.AppSettings["cheminVisitesVirtuelles"];

        /// <summary>
        /// list of our placeholders need
        /// </summary>
        private ObservableHashSet<Visite> _visites;

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Construct default DAO Objet
        /// </summary>
        private DAOVisite()
        {
            _visites = new ObservableHashSet<Visite>();
            collectionChanged = new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            _visites.CollectionChanged += collectionChanged;
        }

        /// <summary>
        /// Construct the DAO with data contains in default path
        /// </summary>
        private DAOVisite(string filename)
        {
            Load(filename);
        }

        #endregion Private Constructors

        #region Private Destructors

        ~DAOVisite()
        {
            if (collectionChanged != null)
            {
                _visites.CollectionChanged -= collectionChanged;
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
        public static List<Panorama> findAllPanoramas()
        {
            List<Panorama> panoramas = new List<Panorama>();
            foreach (Visite visite in DAOVisite.findAllVisites())
            {
                panoramas.AddRange(visite.Panoramas);
            }
            return panoramas;
        }

        public static List<Visite> findAllVisites()
        {
            return new List<Visite>(Instance.Visites);
        }

        public static void Refresh()
        {
            DAOVisite.Instance.Load(defaultPath);
            DAOVisite.Instance.Save(defaultPath);
        }

        /// <summary>
        /// Force save data
        /// </summary>
        public static void Save()
        {
            DAOVisite.Instance.Save(defaultPath);
        }

        public void Dispose()
        {
            _visites.CollectionChanged -= collectionChanged;
            collectionChanged = null;

            foreach (Visite v in _visites)
            {
                v.Dispose();
            }

            _visites.Clear();
            _visites = null;
            _instance = null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Create a default object and save it the given path
        /// </summary>
        /// <param name="fileName"></param>
        private void CreateDefault(string fileName)
        {
            DAOVisite obj = new DAOVisite();
            obj.Save(fileName);
        }

        private void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                CreateDefault(filename);
            }

            DAOVisite other;
            try
            {
                FileStream file = File.OpenRead(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(DAOVisite));
                other = (DAOVisite)serializer.Deserialize(file);
                file.Close();
            }
            catch (Exception)
            {
                throw new Exception("Erreur de serialization");
            }

            // copying items
            _visites = other._visites;
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
                XmlSerializer serializer = new XmlSerializer(typeof(DAOVisite));
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
        public static DAOVisite Instance
        {
            get
            {
                if (_instance == null) { _instance = new DAOVisite(defaultPath); }
                return _instance;
            }
        }

        /// <summary>
        /// Get the place holders collection
        /// </summary>
        public ObservableCollection<Visite> Visites
        {
            get { return _visites; }
        }

        #endregion getters and setters
    }
}