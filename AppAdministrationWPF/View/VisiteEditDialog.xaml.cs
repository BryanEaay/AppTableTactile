using CommonSurface.Model;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows;

namespace AppAdministrationWPF.View
{
    public partial class VisiteEditDialog : Window
    {
        #region Private Fields

        private bool _cancel;
        private string chemin = ConfigurationManager.AppSettings["cheminVisitesVirtuelles"];

        private Visite new_visite;
        private Visite visite;

        #endregion Private Fields

        #region Public Constructors

        public VisiteEditDialog(Visite visite)
        {
            InitializeComponent();

            this._cancel = true;
            this.visite = visite;
            this.new_visite = new Visite(visite);

            // On initialise le contenu des composants
            DataContext = new_visite;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool IsCancel()
        {
            return this._cancel;
        }

        #endregion Public Methods

        #region Private Methods

        //bouton de fermeture
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this._cancel = true;
            Close();
        }

        private void btCheminVisite_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre de recherche de fichier sur des fichiers video image ou panorama
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Visite Virtuelle (*.html)|*.html|Tous les fichiers|*.*";
            fileDialog.FilterIndex = 0;
            fileDialog.Multiselect = false;
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                //ajouter les données a la fenetre
                new_visite.Cover = fileDialog.FileName;
            }
        }

        private void btMiniature_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre de recherche de fichier sur des fichiers video image ou panorama
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Videos(*.mov, *.wmv, *.mp4)|*.mov;*.wmv;*.mp4|Photos (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 2;
            fileDialog.Multiselect = false;
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                new_visite.Thumbnail = fileDialog.FileName;
            }
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            if (new_visite.Cover.Length > 0)
            {
                visite.Copy(new_visite);
                this._cancel = false;
                Close();
            }
            else
            {
                this._cancel = true;
                MessageBox.Show("Aucune visite n'a été assignée!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion Private Methods
    }
}