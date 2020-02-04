using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using Gecko;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AppAdministrationWPF.View
{
    public partial class AdminVisiteView : UserControl
    {
        #region Private Fields

        private bool _open;
        private AdminVisiteViewModel _viewModel;
        private string chemin = ConfigurationManager.AppSettings["cheminVisitesVirtuelles"];
        private string cheminLibrairie = ConfigurationManager.AppSettings["cheminLibrairieVisitesVirtuelles"];
        private GeckoWebBrowser myBrowser;

        #endregion Private Fields

        #region Public Constructors

        public AdminVisiteView()
        {
            InitializeComponent();

            this._open = false;
            ViewModel = new AdminVisiteViewModel();

            #region Récupération des flèches

            // Haut
            flecheHaut.Source = Imaging.CreateBitmapSourceFromHBitmap(
                               CommonSurface.Properties.Resources.FlecheHaut.GetHbitmap(),
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());
            // Bas
            flecheBas.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.FlecheBas.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

            #endregion Récupération des flèches

            this.DataContext = ViewModel;

            // On ordonne par ID
            listboxVisites.Items.SortDescriptions.Clear();
            listboxVisites.Items.SortDescriptions.Add(
                new SortDescription("ID", ListSortDirection.Ascending)
                );
        }

        #endregion Public Constructors

        #region Public Properties

        public AdminVisiteViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Chargement des différentes previews de la visite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        public void launchExpo(object sender, SelectionChangedEventArgs e)
        {
            // Previews retirées
            dock_main_photo.Children.Clear();

            if (ViewModel.Selected != null)
            {
                myBrowser = new GeckoWebBrowser();
                myBrowser.Navigate(ViewModel.Selected.Cover);
                myBrowser.Height = (int)this.dock_main_photo.Height;
                myBrowser.Width = (int)this.dock_main_photo.Width;

                // Ajout du WebBrowser dans le grid
                WindowsFormsHost whost = new WindowsFormsHost();
                whost.Child = myBrowser;
                dock_main_photo.Children.Add(whost);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void buttonAddPanorama_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Selected != null && !this._open)
            {
                // On récupère la scène actuelle
                string scene = getActualScene();
                if (scene.Length <= 0)
                {
                    MessageBox.Show("Scène impossible à récupéré...", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // On vérifie que le panorama n'existe pas déjà
                foreach (Panorama p in ViewModel.Selected.Panoramas)
                {
                    if (scene == p.Scene)
                    {
                        MessageBox.Show("Panorama déjà existant", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                Panorama panorama = Panorama.Blank(scene);
                PanoramaEditDialog dialog = new PanoramaEditDialog(panorama);
                dialog.Closing += (s, t) =>
                {
                    if (!dialog.IsCancel())
                    {
                        ViewModel.Selected.Panoramas.Add(panorama);
                    }
                    this._open = false;
                };
                this._open = true;
                dialog.Show();
            }
        }

        /// <summary>
        /// Ajoute une nouvelle visite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonAddVisite_Click(object sender, RoutedEventArgs e)
        {
            if (!this._open)
            {
                // Ouverture de la fenetre VisiteAdd
                int id = (listboxVisites.Items[listboxVisites.Items.Count - 1] as Visite).ID + 1;
                Visite visite = Visite.Blank(id);

                VisiteEditDialog fenetre = new VisiteEditDialog(visite);
                fenetre.Closing += (s, t) =>
                {
                    if (!fenetre.IsCancel())
                    {
                        ViewModel.Visites.Add(visite);
                    }
                    this._open = false;
                };

                this._open = true;
                fenetre.Show();
            }
        }

        /// <summary>
        /// Supprime le panorama sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonDeletePanorama_Click(object sender, RoutedEventArgs e)
        {
            Panorama panorama = listboxPanorama.SelectedItem as Panorama;
            if (ViewModel.Selected != null && panorama != null)
            {
                ViewModel.Selected.Panoramas.Remove(panorama);
            }
        }

        /// <summary>
        /// Supprime la visite sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonDeleteVisite_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Selected != null)
            {
                ViewModel.Visites.Remove(ViewModel.Selected);
                ViewModel.Selected = null;
            }
        }

        /// <summary>
        /// Bouge la visite un cran inférieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonDown(object sender, RoutedEventArgs e)
        {
            int index = listboxVisites.SelectedIndex;
            if (index < listboxVisites.Items.Count - 1)
            {
                // On sélectionne les 2 éléments à intervertir
                Visite v1 = listboxVisites.Items[index] as Visite;
                Visite v2 = listboxVisites.Items[index + 1] as Visite;

                int tmp = v1.ID;
                v1.ID = v2.ID;
                v2.ID = tmp;
            }

            listboxVisites.Items.SortDescriptions.Clear();
            listboxVisites.Items.SortDescriptions.Add(
                new SortDescription("ID", ListSortDirection.Ascending)
                );
        }

        /// <summary>
        /// Exportation des visites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonExporter_Click(object sender, RoutedEventArgs e)
        {
            Export fenetre = new Export(chemin, cheminLibrairie);
            fenetre.Show();
        }

        /// <summary>
        /// Importation des visites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonImporter_Click(object sender, RoutedEventArgs e)
        {
            Import fenetre = new Import(chemin, cheminLibrairie);
            fenetre.Show();
        }

        /// <summary>
        /// Ouverture la fenêtre de customisation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonModifyPanorama_Click(object sender, RoutedEventArgs e)
        {
            if (!this._open)
            {
                int selected = listboxPanorama.SelectedIndex;
                if (ViewModel.Selected != null && selected >= 0)
                {
                    PanoramaEditDialog dialog = new PanoramaEditDialog(ViewModel.Selected.Panoramas[selected]);
                    dialog.Closing += (s, t) =>
                    {
                        ViewModel.Save();
                        this._open = false;
                    };
                    this._open = true;
                    dialog.Show();
                }
            }
        }

        /// <summary>
        /// Modifie la visite sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonModifyVisite_Click(object sender, RoutedEventArgs e)
        {
            if (!this._open)
            {
                VisiteEditDialog fenetre = new VisiteEditDialog(ViewModel.Selected);
                fenetre.Closing += (s, t) =>
                {
                    this._open = false;
                };

                this._open = true;
                fenetre.Show();
            }
        }

        /// <summary>
        /// Bouge la visite un cran supérieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void ButtonUp(object sender, RoutedEventArgs e)
        {
            int index = listboxVisites.SelectedIndex;
            if (index > 0)
            {
                // On sélectionne les 2 éléments à intervertir
                Visite v1 = listboxVisites.Items[index] as Visite;
                Visite v2 = listboxVisites.Items[index - 1] as Visite;

                int tmp = v1.ID;
                v1.ID = v2.ID;
                v2.ID = tmp;
            }

            listboxVisites.Items.SortDescriptions.Clear();
            listboxVisites.Items.SortDescriptions.Add(
                new SortDescription("ID", ListSortDirection.Ascending)
                );
        }

        /// <summary>
        /// Changement de la scène affichée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void changeScene(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Panorama selected = e.AddedItems[0] as Panorama;

                using (AutoJSContext context = new AutoJSContext(myBrowser.Window))
                {
                    try
                    {
                        string script = "document.getElementById(\"krpanoSWFObject\").call(\"loadscene(" + selected.Scene + ")\");";
                        context.EvaluateScript(script);
                    }
                    catch (GeckoException)
                    {
                    }
                };
            }
        }

        /// <summary>
        /// On affiche les panoramas de la visite sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void displayPanoramaList(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.ViewModel.Selected = e.AddedItems[0] as Visite;
            }
            else
            {
                this.ViewModel.Selected = null;
            }

            launchExpo(sender, e);
        }

        /// <summary>
        /// Récupération de la scène actuelle sur la visite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private string getActualScene()
        {
            using (AutoJSContext context = new AutoJSContext(myBrowser.Window))
            {
                string result;
                try
                {
                    context.EvaluateScript("document.getElementById(\"krpanoSWFObject\").get(\"xml.scene\");", out result);
                    return result;
                }
                catch (GeckoException)
                {
                    return "";
                }
            };

            return "";
        }

        /// <summary>
        /// Relance le média lorsque celui-ci se termine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void m_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = sender as MediaElement;
            media.Position = TimeSpan.FromSeconds(0);
            media.Play();
        }

        #endregion Private Methods
    }
}