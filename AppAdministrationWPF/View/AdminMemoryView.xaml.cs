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
    /// <summary>
    /// Logique d'interaction pour AdminMemoryView2.xaml
    /// </summary>
    public partial class AdminMemoryView : UserControl
    {
        #region Private Fields

        private string chemin = ConfigurationManager.AppSettings["cheminMemory"];
        private string cheminLibrairie = ConfigurationManager.AppSettings["cheminLibrairieMemory"];

        private string difficulty;

        #endregion Private Fields

        #region Public Constructors

        public AdminMemoryView()
        {
            InitializeComponent();
            difficulty = "";
            this.DataContext = ServiceLocator.AdminMemoryViewModel;

            #region Récupération des niveaux Memory

            // Niveau 1
            niveau1Memory.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.memory_vert.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            // Niveau 2
            niveau2Memory.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.memory_jaune.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            // Niveau 3
            niveau3Memory.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.memory_rouge.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

            // Arrière plans
            arrierePlansMemory.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.memory_fondecran.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

            #endregion Récupération des niveaux Memory
        }

        #endregion Public Constructors

        #region Public Properties

        public AdminMemoryViewModel viewModel
        {
            get { return (AdminMemoryViewModel)DataContext; }
            set { DataContext = value; }
        }

        #endregion Public Properties

        #region Private Methods

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Picture p = Picture.Blank();
            viewModel.Pictures.Add(p);
            ListPictures.SelectedItem = p;
            gridConfiguration.Visibility = Visibility.Visible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Selected.Name = viewModel.Selected.Source = "";
            gridConfiguration.Visibility = Visibility.Hidden;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListPictures.SelectedItem != null)
            {
                Picture p = ListPictures.SelectedItem as Picture;
                viewModel.Pictures.Remove(p);
                viewModel.Save();
            }
        }

        private void DisplaySelection(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            difficulty = button.Tag as string;
            viewModel.filterPictures(difficulty);
            gridConfiguration.Visibility = Visibility.Hidden;
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
                viewModel.Refresh();
                viewModel.filterPictures(difficulty);
                gridConfiguration.Visibility = Visibility.Hidden;
            };
            import.Show();
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            gridConfiguration.Visibility = Visibility.Visible;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
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

        private void updateSelection(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                viewModel.Selected = e.AddedItems[0] as Picture;
                gridConfiguration.Visibility = Visibility.Hidden;
            }
        }

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Selected != null)
            {
                viewModel.Selected.Name = txtName.Text;
                viewModel.Selected.Source = txtSource.Text;
                gridConfiguration.Visibility = Visibility.Hidden;
                viewModel.Save();
            }
        }

        #endregion Private Methods
    }
}