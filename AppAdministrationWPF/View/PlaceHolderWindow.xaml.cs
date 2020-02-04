using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace AppAdministrationWPF.View
{
    /// <summary>
    /// Logique d'interaction pour PlaceHolderWindow.xaml
    /// </summary>
    public partial class PlaceHolderWindow : Window
    {
        #region Private Fields

        private PlaceHolderWindowModel _viewModel;

        #endregion Private Fields

        #region Public Constructors

        public PlaceHolderWindow(PlaceHolder place)
        {
            _viewModel = new PlaceHolderWindowModel(place);
            this.DataContext = _viewModel;
            InitializeComponent();
        }

        public PlaceHolderWindow(PlaceHolder place, Section section)
        {
            _viewModel = new PlaceHolderWindowModel(place, section);
            this.DataContext = _viewModel;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        public PlaceHolder Selected
        {
            get { return _viewModel.Selected; }
            set { _viewModel.Selected = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Adding a media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btAddMedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDialog dial = new MediaDialog(new MediaDialogModel(null));
            //ouverture de la fenetre d ajout de média
            if (dial.ShowDialog() == true)
            {
                Media m = new Media(dial.Selected);
                m.X = _viewModel.Selected.X;
                m.Y = _viewModel.Selected.Y;
                _viewModel.Selected.Media.Add(m);
            }
            //actualisation des données
            _viewModel.Selected.Name = this.Selected.Name;
            _viewModel.Selected.X = this.Selected.X;
            _viewModel.Selected.Y = this.Selected.Y;
            _viewModel.Selected.Id = this.Selected.Id;
            _viewModel.Selected.IconPath = this.Selected.IconPath;

            _viewModel.Selected.Media = new ObservableCollection<Media>(this.Selected.Media);
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Close the windows without returning true to showDialog()
        /// </summary>
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btDeleteMedia_Click(object sender, RoutedEventArgs e)
        {
            if (lbMedia.SelectedIndex < 0)
            {
                return;
            }

            _viewModel.Selected.Media.Remove(lbMedia.SelectedItem as Media);
        }

        private void btModifyMedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDialog dial = new MediaDialog(new MediaDialogModel((Media)lbMedia.SelectedItem));
            //ouverture de la fenetre d ajout de média
            if (dial.ShowDialog() == true)
            {
                Media m = new Media(dial.Selected);
                m.X = _viewModel.Selected.X;
                m.Y = _viewModel.Selected.Y;
                _viewModel.Selected.Media[lbMedia.SelectedIndex] = m;
            }
            //actualisation des données
            _viewModel.Selected.Name = this.Selected.Name;
            _viewModel.Selected.X = this.Selected.X;
            _viewModel.Selected.Y = this.Selected.Y;
            _viewModel.Selected.Id = this.Selected.Id;
            _viewModel.Selected.IconPath = this.Selected.IconPath;

            _viewModel.Selected.Media = new ObservableCollection<Media>(this.Selected.Media);
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Close the windows and return true to showDialog() method
        /// </summary>
        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Deleting a media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void miDeletePH_Click(object sender, RoutedEventArgs e)
        {
            Media selectedMedia = (Media)lbMedia.SelectedItem;
            //suppression du fichier média
            if (selectedMedia == null)
            {
                MessageBox.Show(this, "Veuillez selectionner un media", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show(this, "Êtes vous sûr de vouloir supprimer ce media?", "Suppression d'un media", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _viewModel.Selected.Media.Remove(selectedMedia);
            }
            //actualisation des données
            _viewModel.Selected.Name = this.Selected.Name;
            _viewModel.Selected.X = this.Selected.X;
            _viewModel.Selected.Y = this.Selected.Y;
            _viewModel.Selected.Id = this.Selected.Id;
            _viewModel.Selected.IconPath = this.Selected.IconPath;

            _viewModel.Selected.Media = new ObservableCollection<Media>(this.Selected.Media);
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Updating a media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void miEditPH_Click(object sender, RoutedEventArgs e)
        {
            Media selectedMedia = (Media)lbMedia.SelectedItem;
            MediaDialog dial = new MediaDialog(new MediaDialogModel(selectedMedia));
            //suppression de l'ancien fichier média
            if (selectedMedia == null)
            {
                MessageBox.Show(this, "Veuillez selectionner un media", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                _viewModel.Selected.Media.Remove(selectedMedia);
            }
            //ouverture de la fenetre de modification
            if (dial.ShowDialog() == true)
            {
                Media m = new Media(dial.Selected);
                m.X = _viewModel.Selected.X;
                m.Y = _viewModel.Selected.Y;
                _viewModel.Selected.Media.Add(m);
            }
            //actualisation des données
            _viewModel.Selected.Name = this.Selected.Name;
            _viewModel.Selected.X = this.Selected.X;
            _viewModel.Selected.Y = this.Selected.Y;
            _viewModel.Selected.Id = this.Selected.Id;
            _viewModel.Selected.IconPath = this.Selected.IconPath;

            _viewModel.Selected.Media = new ObservableCollection<Media>(this.Selected.Media);
            this.DataContext = _viewModel;
        }

        private void txtIconPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Images (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 3;
            fileDialog.Multiselect = false;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                Selected.IconPath = fileDialog.FileName;
            }
            //actualisation des données
            _viewModel.Selected.Name = this.Selected.Name;
            _viewModel.Selected.X = this.Selected.X;
            _viewModel.Selected.Y = this.Selected.Y;
            _viewModel.Selected.Id = this.Selected.Id;
            _viewModel.Selected.IconPath = this.Selected.IconPath;

            _viewModel.Selected.Media = new ObservableCollection<Media>(this.Selected.Media);
            this.DataContext = _viewModel;
        }

        #endregion Private Methods
    }
}