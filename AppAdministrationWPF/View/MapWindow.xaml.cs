using CommonSurface.Model;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows;

namespace AppAdministrationWPF.View
{
    /// <summary>
    /// Logique d'interaction pour MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        #region Private Fields

        private bool _cancel;
        private Map new_map;
        private Map old_map;

        #endregion Private Fields

        #region Public Constructors

        public MapWindow(Map map)
        {
            InitializeComponent();

            old_map = map;
            new_map = new Map(map);

            _cancel = true;

            txtMap.Text = map.Background;
            txtName.Text = map.Name;

            this.DataContext = new_map;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool isCancel()
        {
            return _cancel;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Annulation des modifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancel = true;
            this.Close();
        }

        /// <summary>
        /// Validations des modifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            new_map.Name = txtMap.Text;
            new_map.Name = txtName.Text;

            if (new_map.Name.Length <= 0 || new_map.Background.Length <= 0)
            {
                MessageBox.Show("Le nom ou l'image n'a pas été définis.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            old_map.Copy(new_map);
            _cancel = false;
            this.Close();
        }

        private void buttonSearchFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Photos (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 0;
            fileDialog.RestoreDirectory = true;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                new_map.Background = fileDialog.FileName;
                txtMap.Text = fileDialog.FileName;
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
                new_map.Thumbnail = fileDialog.FileName;
            }
        }

        #endregion Private Methods
    }
}