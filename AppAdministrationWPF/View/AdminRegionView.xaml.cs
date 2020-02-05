﻿using AppAdministrationWPF.Utils;
using AppAdministrationWPF.ViewModel;
using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.Other;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace AppAdministrationWPF.View
{
    /// <summary>
    /// Logique d'interaction pour AdminRegionView.xaml
    /// </summary>
    public partial class AdminRegionView : UserControl
    {
        #region Private Fields

        private AdminRegionViewModel _viewModel;
        private string chemin = ConfigurationManager.AppSettings["cheminRegion"];
        private string cheminLibrairie = ConfigurationManager.AppSettings["cheminLibrairieRegion"];
        private string defautCarte = ConfigurationManager.AppSettings["cheminDefautCarte"];
        private ResourceDictionary myresourcedictionary;
        private Button selectedOne;

        #endregion Private Fields

        #region Public Constructors

        public AdminRegionView()
        {
            InitializeComponent();
            _viewModel = ServiceLocator.AdminRegionViewModel;
            this.DataContext = _viewModel;

            if (ViewModel.Maps.Count > 0)
            {
                listboxMaps.SelectedIndex = 0;
            }

            myresourcedictionary = new ResourceDictionary();
            myresourcedictionary.Source = new Uri("/CommonSurface;component/XAML/Effects.xaml", UriKind.RelativeOrAbsolute);

            listboxMaps.Items.SortDescriptions.Clear();
            listboxMaps.Items.SortDescriptions.Add(
                new SortDescription("ID", ListSortDirection.Ascending)
                );
        }

        #endregion Public Constructors

        #region Public Properties

        public AdminRegionViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public void DeletePlace(PlaceHolder place)
        {
            _viewModel.SelectedMap.PlaceHolders.Remove(place);
        }

        public void EditPlace(PlaceHolder place)
        {
            if (place == null)
            {
                MessageBox.Show("Veuillez selectionner un lieu", "Erreur");
                return;
            }
            PlaceHolderWindow cw = new PlaceHolderWindow(place);
            if (cw.ShowDialog() == true)
            {
                _viewModel.SelectedPlaceholder.Name = cw.Selected.Name;
                _viewModel.SelectedPlaceholder.X = cw.Selected.X;
                _viewModel.SelectedPlaceholder.Y = cw.Selected.Y;
                _viewModel.SelectedPlaceholder.Id = cw.Selected.Id;
                _viewModel.SelectedPlaceholder.IconPath = cw.Selected.IconPath;

                _viewModel.SelectedPlaceholder.Media = new System.Collections.ObjectModel.ObservableCollection<Media>(cw.Selected.Media);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AddPlace()
        {
            PlaceHolderWindow cw = new PlaceHolderWindow(null);
            if (cw.ShowDialog() == true)
            {
                _viewModel.SelectedMap.PlaceHolders.Add(cw.Selected);
                _viewModel.SelectedPlaceholder = cw.Selected;
            }
        }

        /// <summary>
        /// Ajout d'une nouvelle carte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btAddMap_Click(object sender, RoutedEventArgs e)
        {
            // Nouvelle carte
            ListBox listboxMaps1 = listboxMaps;
            int id = (listboxMaps1.Items[listboxMaps.Items.Count - 1] as Map).ID + 1;
            Map map = Map.Blank(id);
            MapWindow window = new MapWindow(map);

            // On ajoute que si tout est bon
            window.Closing += (s, t) =>
            {
                if (!window.isCancel())
                {
                    _viewModel.Maps.Add(map);
                    listboxMaps.SelectedIndex = _viewModel.Maps.Count - 1;
                }
            };

            window.Show();
        }

        private void btAddPlace_Click(object sender, RoutedEventArgs e)
        {
            AddPlace();
        }

        /// <summary>
        /// Suppresion d'une carte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btDeleteMap_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedMap != null)
            {
                int index = listboxMaps.SelectedIndex;
                _viewModel.Maps.Remove(_viewModel.SelectedMap);

                if (ViewModel.Maps.Count < 1)
                {
                    listboxMaps.SelectedIndex = -1;
                }
                else if (index > ViewModel.Maps.Count - 1)
                {
                    listboxMaps.SelectedIndex = ViewModel.Maps.Count - 1;
                }
            }
        }

        private void btEditMap_Click(object sender, RoutedEventArgs e)
        {
            Map selected = _viewModel.SelectedMap;
            MapWindow window = new MapWindow(selected);
            window.Closing += (s, t) =>
            {
                if (!window.isCancel())
                {
                    listboxMaps.Items.Refresh();
                    DAORegion.Save();
                    Carte.Source = ResourceAccessor.loadImage(selected.Background);
                }
            };
            window.Show();
        }

        //Action du bouton Exporter
        private void btExporter_Click(object sender, RoutedEventArgs e)
        {
            Export fenetre = new Export(chemin, cheminLibrairie);
            fenetre.Show();
        }

        //Action du bouton Importer
        private void btImporter_Click(object sender, RoutedEventArgs e)
        {
            Import fenetre = new Import(chemin, cheminLibrairie, false, false);
            fenetre.Closing += update_OnClose;
            fenetre.Show();
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            int index = listboxMaps.SelectedIndex;
            if (index < listboxMaps.Items.Count - 1)
            {
                Map map1 = listboxMaps.Items[index] as Map;
                Map map2 = listboxMaps.Items[index + 1] as Map;

                int tmp = map2.ID;
                map2.ID = map1.ID;
                map1.ID = tmp;
            }
            listboxMaps.Items.SortDescriptions.Clear();
            listboxMaps.Items.SortDescriptions.Add(
                new SortDescription("ID", ListSortDirection.Ascending)
                );
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            int index = listboxMaps.SelectedIndex;
            if (index > 0)
            {
                Map map1 = listboxMaps.Items[index] as Map;
                Map map2 = listboxMaps.Items[index - 1] as Map;

                int tmp = map2.ID;
                map2.ID = map1.ID;
                map1.ID = tmp;
            }
            listboxMaps.Items.SortDescriptions.Clear();
            listboxMaps.Items.SortDescriptions.Add(
                new SortDescription("ID", ListSortDirection.Ascending)
                );
        }

        /// <summary>
        /// Sélection de la carte par la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void changeMap(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _viewModel.SelectedMap = listboxMaps.SelectedItem as Map;
                _viewModel.SelectedPlaceholder = null;

                Carte.Source = ResourceAccessor.loadImage(_viewModel.SelectedMap.Background);
            }
        }

        private void Map_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            // Récupération de la position de la souris par rapport à la carte
            double x = e.GetPosition(canvasCarte).X;
            double y = e.GetPosition(canvasCarte).Y;

            // En dehors de la carte
            if (x < 0 || x > canvasCarte.ActualWidth || y < 0 || y > canvasCarte.ActualHeight)
            {
                return;
            }

            setPlaceHolderPosition(x, y);
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Map_MouseLeftButtonUp(null, e);
            }
        }

        private void miDeletePH_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedPlaceholder == null)
            {
                MessageBox.Show("Veuillez selectionner un lieu", "Erreur");
                return;
            }
            if (MessageBox.Show("Êtes vous sûr de vouloir supprimer ce lieu?", "Suppression d'un lieu", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DeletePlace(_viewModel.SelectedPlaceholder);
                _viewModel.SelectedPlaceholder = null;
            }
        }

        private void miEditPH_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedPlaceholder == null)
            {
                MessageBox.Show("Veuillez selectionner un lieu", "Erreur");
                return;
            }
            EditPlace(_viewModel.SelectedPlaceholder);
        }

        /// <summary>
        /// Move the selected item to the given position on the map
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void selectOne(object sender, RoutedEventArgs e)
        {
            if (selectedOne != null)
            {
                Storyboard sbDefloating = myresourcedictionary["animDefloat"] as Storyboard;
                Storyboard.SetTarget(sbDefloating, selectedOne);
                selectedOne.BeginStoryboard(sbDefloating);
            }

            selectedOne = (sender as FrameworkElement) as Button;
            _viewModel.SelectedPlaceholder = selectedOne.DataContext as PlaceHolder;

            Storyboard sbFloating = myresourcedictionary["animFloat"] as Storyboard;
            Storyboard.SetTarget(sbFloating, selectedOne);
            selectedOne.BeginStoryboard(sbFloating);
        }

        private void setPlaceHolderPosition(double x, double y)
        {
            if (_viewModel.SelectedPlaceholder != null)
            {
                _viewModel.SelectedPlaceholder.X = (int)(x - selectedOne.ActualWidth / 2);
                _viewModel.SelectedPlaceholder.Y = (int)(y - selectedOne.ActualHeight / 2);
            }
        }

        private void update_OnClose(object sender, CancelEventArgs e)
        {
            DAORegion.Refresh();
            MessageBox.Show("Changer d'onglet et revenez sur celui-ci pour terminer l'importation.");

            if (_viewModel.Maps.Count > 0)
            {
                mapBackground.ItemsSource = _viewModel.Maps[0].PlaceHolders;
                Carte.Source = ResourceAccessor.loadImage(_viewModel.Maps[0].Background);
                listboxMaps.SelectedIndex = 0;
                listboxMaps.Items.Refresh();
                listboxMaps.Items.SortDescriptions.Clear();
                listboxMaps.Items.SortDescriptions.Add(
                    new SortDescription("ID", ListSortDirection.Ascending)
                    );
            }
            else
            {
                mapBackground.ItemsSource = null;
                Carte.Source = null;
                listboxMaps.SelectedIndex = -1;
            }
        }

        #endregion Private Methods

        #region media actions

        /// <summary>
        /// Add a media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btAddMedia_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedPlaceholder == null)
            {
                MessageBox.Show("Veuillez selectionner un lieu", "Erreur");
                return;
            }
            MediaDialog dial = new MediaDialog(new MediaDialogModel(null));
            if (dial.ShowDialog() == true)
            {
                _viewModel.SelectedPlaceholder.Media.Add(dial.Selected);
            }
        }

        #endregion media actions
    }
}