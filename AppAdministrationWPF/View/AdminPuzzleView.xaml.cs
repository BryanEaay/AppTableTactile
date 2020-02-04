using AppAdministrationWPF.Utils;
using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AppAdministrationWPF.View
{
    public partial class AdminPuzzleView : UserControl
    {
        #region Private Fields

        private AdminPuzzleViewModel _viewModel;
        private string chemin = ConfigurationManager.AppSettings["cheminPuzzle"];
        private string cheminLibrairie = ConfigurationManager.AppSettings["cheminLibrairiePuzzle"];

        #endregion Private Fields

        #region Public Constructors

        public AdminPuzzleView()
        {
            // Requis pour initialiser des variables
            InitializeComponent();
            _viewModel = ServiceLocator.AdminPuzzleViewModel;
            this.DataContext = _viewModel;

            #region Récupération des niveaux Puzzle

            // Niveau 1
            niveau1Puzzle.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.PUZZLE_FACILE.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            // Niveau 2
            niveau2Puzzle.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.PUZZLE_MOYEN.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            // Niveau 3
            niveau3Puzzle.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.PUZZLE_DIFFICILE.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

            #endregion Récupération des niveaux Puzzle

            #region Récupération des icônes

            // Ajouter
            iconeAjouter.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.gear.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            // Modifier
            iconeModifier.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.gear.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            // Supprimer
            iconeSupprimer.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.gear.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

            #endregion Récupération des icônes

            DeleteButton.Visibility = Visibility.Collapsed;
        }

        #endregion Public Constructors

        #region Public Properties

        public AdminPuzzleViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Ajout d'un nouveau puzzle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            gridAdministration.Visibility = Visibility.Visible;
            UpdateVisibility(true);

            Picture p = Picture.Blank();
            ViewModel.Pictures.Add(p);
            ListPuzzle.SelectedItem = ViewModel.Selected = p;
        }

        /// <summary>
        /// Sélection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Photos (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 0;
            fileDialog.Multiselect = false;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                txtSource.Text = fileDialog.FileName;
                previewMedia.Source = new Uri(fileDialog.FileName);
            }
        }

        /// <summary>
        /// Annulation des modifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            gridAdministration.Visibility = Visibility.Hidden;
            UpdateVisibility(false);
        }

        /// <summary>
        /// Suppression du puzzle sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Selected != null)
            {
                gridAdministration.Visibility = Visibility.Hidden;
                UpdateVisibility(false);
                ViewModel.Pictures.Remove(ViewModel.Selected);
                ViewModel.Selected = null;
                ViewModel.Save();
            }
        }

        /// <summary>
        /// Sélection de la difficulté
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void DifficultySelection(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ViewModel.FilterPictures(button.Tag as string);
            gridAdministration.Visibility = Visibility.Hidden;
            UpdateVisibility(false);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Export export = new Export(chemin, cheminLibrairie);
            export.Show();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            Import import = new Import(chemin, cheminLibrairie, false, false);
            import.Closing += (s, t) =>
            {
                _viewModel.Refresh();
            };
            import.Show();
        }

        /// <summary>
        /// Modification d'un puzzle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Selected != null)
            {
                gridAdministration.Visibility = Visibility.Visible;
                UpdateVisibility(true);
            }
        }

        /// <summary>
        /// Mise à jour de la sélection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void UpdateSeletion(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ViewModel.Selected = e.AddedItems[0] as Picture;
                txtSource.Text = ViewModel.Selected.Source;
                txtName.Text = ViewModel.Selected.Name;
            }
            else
            {
                gridAdministration.Visibility = Visibility.Hidden;
                txtSource.Text = txtName.Text = "";
            }
        }

        /// <summary>
        /// Mise à jour des la visibilité des boutons de configuration
        /// </summary>
        /// <param name="previewVisible">Preview visible ?</param>
        /// <param name="deleteVisible"> Bouton de suppression visible ?</param>
        private void UpdateVisibility(bool editMode)
        {
            AddButton.Visibility = ModifyButton.Visibility = editMode ? Visibility.Collapsed : Visibility.Visible;
            previewMedia.Visibility = DeleteButton.Visibility = editMode ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Applications des modifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gridAdministration.Visibility = Visibility.Hidden;
                UpdateVisibility(false);
                ViewModel.Selected.Source = txtSource.Text;
                ViewModel.Selected.Name = txtName.Text;
                ListPuzzle.Items.Refresh();
                ViewModel.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error AdminPuzzleView : " + ex.Message);
            }
        }

        #endregion Private Methods
    }
}