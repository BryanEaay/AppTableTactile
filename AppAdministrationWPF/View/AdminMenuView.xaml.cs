using AppAdministrationWPF.Utils;
using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using CommonSurface.Other;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Xml;

namespace AppAdministrationWPF.View
{
    /// <summary>
    /// Logique d'interaction pour AdminMenuView.xaml
    /// </summary>
    public partial class AdminMenuView : UserControl
    {
        #region Calcul des ratios de redimensionnement

        private const double CANVAS_HEIGHT = 1080.0;
        private const double CANVAS_WIDTH = 1920.0;
        private const int GRID_STEP = 20;
        private const double RATIO_X = 1665.0 / CANVAS_WIDTH;
        private const double RATIO_Y = 920.0 / CANVAS_HEIGHT;

        #endregion Calcul des ratios de redimensionnement

        #region Private Fields

        private bool _gridVisible = false;
        private bool _open = false;
        private StackPanel _selectedItem;
        private AdminMenuViewModel _viewModel;
        private string chemin = ConfigurationManager.AppSettings["cheminMenu"];
        private string cheminLibrairie = ConfigurationManager.AppSettings["cheminLibrairieMenu"];

        private DropShadowEffect shadowEffect = new DropShadowEffect()
        {
            Color = new Color { A = 120, R = 0, G = 0, B = 0 },
            Direction = 225,
            ShadowDepth = 4,
            BlurRadius = 4,
            Opacity = 1
        };

        #endregion Private Fields

        #region Public Constructors

        public AdminMenuView()
        {
            InitializeComponent();

            // Ratio du canvas
            canvasScaleTransform.ScaleX = RATIO_X;
            canvasScaleTransform.ScaleY = RATIO_Y;

            // Insertion des items
            _viewModel = ServiceLocator.AdminMenuViewModel;
            this.DataContext = _viewModel;
            this._selectedItem = null;
        }

        #endregion Public Constructors

        #region Public Properties

        public AdminMenuViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        #endregion Public Properties

        #region Fonction du quadrillage

        /// <summary>
        /// Affiche ou cache le cadrillage du canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonShowHideGrid_Click(object sender, RoutedEventArgs e)
        {
            if (this._gridVisible)
            {
                // On s'assure que toutes les lignes ont bien été retirés
                int lineCounter = 1;
                while (lineCounter > 0)
                {
                    for (int i = 0; i < canvasMenu.Children.Count; i++)
                    {
                        lineCounter = 0;
                        UIElement elem = canvasMenu.Children[i];

                        // On retire que les lignes
                        if (elem is Line)
                        {
                            lineCounter++;
                            canvasMenu.Children.Remove(elem);
                        }
                    }
                }
            }
            else
            {
                // On dessine le cadriallage
                drawGridLines();
            }

            // On inverse l'état de visibilité du cadriallage
            this._gridVisible = !this._gridVisible;
        }

        /// <summary>
        /// Dessine le cadriallage sur le canvas
        /// </summary>
        private void drawGridLines()
        {
            // Lignes verticales
            for (int i = 1; i < CANVAS_WIDTH / GRID_STEP; i++)
            {
                drawLine(i * GRID_STEP, 0, i * GRID_STEP, CANVAS_HEIGHT, Brushes.DarkGray);
            }

            // Ligne centrale verticale
            drawLine(CANVAS_WIDTH / 2, 0, CANVAS_WIDTH / 2, CANVAS_HEIGHT, Brushes.DarkRed, 3, 3);

            // Lignes horizontales
            for (double i = 1; i < CANVAS_HEIGHT / GRID_STEP; i++)
            {
                drawLine(0, i * GRID_STEP, CANVAS_WIDTH, i * GRID_STEP, Brushes.DarkGray);
            }

            // Ligne centrale horizontale
            drawLine(0, CANVAS_HEIGHT / 2, CANVAS_WIDTH, CANVAS_HEIGHT / 2, Brushes.DarkRed, 3, 3);
        }

        /// <summary>
        /// Dessine une ligne sur le canvas
        /// </summary>
        /// <param name="x1">       Coordonnées x1</param>
        /// <param name="y1">       Coordonnées y1</param>
        /// <param name="x2">       Coordonnées x2</param>
        /// <param name="y2">       Coordonnées y2</param>
        /// <param name="color">    Couleur de la ligne</param>
        /// <param name="thickness">Epaisseur de la ligne</param>
        /// ///
        /// <param name="thickness">Index de profondeur de la ligne</param>
        private void drawLine(double x1, double y1, double x2, double y2, SolidColorBrush color, int thickness = 1, int zindex = 1)
        {
            Line line = new Line();

            // Couleur et épaisseur de la ligne
            line.StrokeThickness = thickness;
            line.Stroke = color;
            Canvas.SetZIndex(line, zindex);

            // Points de la ligne
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;

            canvasMenu.Children.Add(line);
        }

        #endregion Fonction du quadrillage

        #region Private Methods

        /// <summary>
        /// Bouton pour sélectionner l'image ou vidéo afficher aux crédits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonCredits_Click(object sender, RoutedEventArgs e)
        {
            // Filedialog pour sélectionner une image de fond
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Photos (*.jpg, *.png)|*.jpg;*.png|Vidéos (*.mp4, *.mov)|*.mp4;*.mov";
            fileDialog.FilterIndex = 0;
            fileDialog.Multiselect = false;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                // Changement de l'image affichée
                this._viewModel.Credits.Source = fileDialog.FileName;

                // On sauvegarde la nouvelle image de fond dans le XML
                XmlDocument doc = new XmlDocument();
                doc.Load(chemin);
                XmlNode node = doc.DocumentElement.SelectSingleNode("/DAOMenu/Credits/Source");
                node.InnerText = this._viewModel.Credits.Source;
                doc.Save(chemin);
            }
        }

        /// <summary>
        /// Configuration du bouton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            // On s'assure qu'un objet a bien étét sélectionné
            if (this._selectedItem != null)
            {
                // On vérifie que la fenêtre n'est pas déjà ouverte
                if (!this._open)
                {
                    // Fenêtre pour ajouter une icone retirée
                    MenuIconEditDialog buttondialog = new MenuIconEditDialog(_viewModel.Selected);
                    this._open = true;
                    buttondialog.Show();
                    buttondialog.Closing += (s, t) =>
                    {
                        this._open = false;
                    };
                }
            }
            else
            {
                MessageBox.Show("Erreur!\nAucun objet de sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Exportation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            Export export = new Export(chemin, cheminLibrairie);
            export.Show();
        }

        /// <summary>
        /// Importation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            Import import = new Import(chemin, cheminLibrairie, false, false);
            import.Closing += (s, t) =>
            {
                _viewModel.Refresh();
            };
            import.Show();
        }

        /// <summary>
        /// Change la visibilité du bouton dans le programme principal. Représenter ici par une
        /// modification de l'opacité.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedItem != null)
            {
                _viewModel.Selected.Visibility = !_viewModel.Selected.Visibility;
                buttonVisibility.Content = (_viewModel.Selected.Visibility) ? "Cacher" : "Afficher";
            }
        }

        /// <summary>
        /// Déplacement du bouton sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void moveIcon(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                if (this._selectedItem != null)
                {
                    double xx, yy;

                    // Position de la souris sur le canvas
                    double eX = e.GetPosition(canvasMenu).X;
                    double eY = e.GetPosition(canvasMenu).Y;

                    #region Gestion de la position de la souris

                    if (eX < 0 || eX > CANVAS_WIDTH || eY < 0 || eY > CANVAS_HEIGHT)
                    {
                        // La souris est en dehors du canvas
                        return;
                    }

                    #endregion Gestion de la position de la souris

                    #region Gestion des bordures pour X

                    if (eX - this._selectedItem.Width / 2 < 0)
                    {
                        xx = 0;
                    }
                    else if (eX + this._selectedItem.Width / 2 > CANVAS_WIDTH)
                    {
                        xx = CANVAS_WIDTH - this._selectedItem.Width;
                    }
                    else
                    {
                        if (this._gridVisible)
                        {
                            int pos = (int)(eX / GRID_STEP);
                            xx = pos * GRID_STEP - this._selectedItem.Width / 2;
                        }
                        else
                        {
                            xx = eX - this._selectedItem.Width / 2;
                        }
                    }

                    #endregion Gestion des bordures pour X

                    #region Gestion des bordures pour Y

                    if (eY - this._selectedItem.Height / 2 < 0)
                    {
                        yy = 0;
                    }
                    else if (eY + this._selectedItem.Height / 2 > CANVAS_HEIGHT)
                    {
                        yy = CANVAS_HEIGHT - this._selectedItem.Height;
                    }
                    else
                    {
                        if (this._gridVisible)
                        {
                            int pos = (int)(eY / GRID_STEP);
                            yy = pos * GRID_STEP - this._selectedItem.Height / 2;
                        }
                        else
                        {
                            yy = eY - this._selectedItem.Height / 2;
                        }
                    }

                    #endregion Gestion des bordures pour Y

                    // Destination de l'icone
                    _viewModel.Selected.X = (int)xx;
                    _viewModel.Selected.Y = (int)yy;
                }
            }
        }

        /// <summary>
        /// Sélection du bouton à modifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void selectIcon(object sender, EventArgs e)
        {
            // Gestion d'un cas d'erreur
            if (sender == null)
            {
                return;
            }

            // On remet l'opacité de l'ancien objet choisis
            if (this._selectedItem != null)
            {
                this._selectedItem.Effect = null;
            }

            // Si l'objet est différent de l'ancien, on le sélectionne Sinon, on déselectionne
            // l'objet courant
            if (this._selectedItem != sender as StackPanel)
            {
                _selectedItem = sender as StackPanel;
                _selectedItem.Effect = shadowEffect;
                _viewModel.Selected = (_selectedItem.Tag as string == _viewModel.Credits.Icon.Name) ? _viewModel.Credits.Icon : (Icon)_selectedItem.DataContext;
                panelIconEdit.Visibility = Visibility.Visible;
				if (this._viewModel.Selected.Name == "Credits")
                {
                    buttonCredit.Visibility = Visibility.Visible;
                }											   
                buttonVisibility.Content = (_viewModel.Selected.Visibility) ? "Cacher" : "Afficher";
            }
            else
            {
                this._selectedItem = null;
                panelIconEdit.Visibility = Visibility.Hidden;
				buttonCredit.Visibility = Visibility.Collapsed;											   
            }
        }

        #endregion Private Methods
    }
}