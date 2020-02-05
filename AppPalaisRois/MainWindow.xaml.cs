using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.Other;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AppPalaisRois
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields

        private int isopen = 0;
        private ResourceDictionary myresourcedictionary;
        private Storyboard sbHideAnim, sbShowAnim, sbHideAnimSec, sbShowAnimSec;

        #endregion Private Fields

        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();

            #region Gestion des icones

            List<Icon> icons = new List<Icon>(DAOMenu.Instance.Icons);
            icons.Add(DAOMenu.Instance.Credits.Icon);
            foreach (Icon icon in icons)
            {
                // Récupération des éléments graphiques de l'icone
                StackPanel panelIcon = this.FindName("position" + icon.Name) as StackPanel;
                Image imageIcon = this.FindName("image" + icon.Name) as Image;
                Label labelIcon = this.FindName("label" + icon.Name) as Label;

                // On change la visibilité de l'icone
                panelIcon.Visibility = icon.Visibility ? Visibility.Visible : Visibility.Collapsed;

                // On ajuste les paramètres
                if (icon.Visibility)
                {
                    // Positionne le panel à la position de la configuration
                    Canvas.SetLeft(panelIcon, Convert.ToDouble(icon.X));
                    Canvas.SetTop(panelIcon, Convert.ToDouble(icon.Y));

                    // Charge l'icone de la configuration
                    imageIcon.Source = ResourceAccessor.loadImage(icon.Source);

                    // Label avec le texte définit dans la configuration
                    if (icon.Text != null)
                    {
                        labelIcon.Content = icon.Text;
                    }

                    if (icon.Color != null)
                    {
                        labelIcon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(icon.Color));
                    }
                }
            }

            #endregion Gestion des icones

            // Chargement des ressources
            imageBackground.Source = ResourceAccessor.loadImage(DAOMenu.Instance.Background);
            mediaCredits.Source = new Uri(DAOMenu.Instance.Credits.Source);

            #region Dimension du crédit

            // Calcul de la taille du crédit
            var img = ResourceAccessor.loadImage(DAOMenu.Instance.Credits.Source);
            if (img.Width > img.Height)
            {
                sviCredits.MinWidth = ScatterView.MinWidth = 400 * (img.Width / img.Height);
                sviCredits.MinHeight = ScatterView.MinHeight = 400;
            }
            else
            {
                ScatterView.MinWidth = 400;
                ScatterView.MinHeight = 400 * (img.Height / img.Width);
            }

            #endregion Dimension du crédit

            // Récupération du dictionnaire des ressources
            myresourcedictionary = new ResourceDictionary();
            myresourcedictionary.Source = new Uri("/CommonSurface;component/XAML/Effects.xaml", UriKind.RelativeOrAbsolute);

            // Récupération des animations
            sbHideAnim = myresourcedictionary["hideAnim"] as Storyboard;
            sbHideAnimSec = myresourcedictionary["hideAnimSec"] as Storyboard;
            sbShowAnim = myresourcedictionary["showAnim"] as Storyboard;
            sbShowAnimSec = myresourcedictionary["showAnimSec"] as Storyboard;

            // Garbage collector
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Changement de la visibilité du crédit / copyright
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            int opacity = (ScatterView.Visibility == Visibility.Visible) ? 1 : 0;
            var animation = new DoubleAnimation()
            {
                From = opacity,
                To = 1 - opacity,
                Duration = new Duration(TimeSpan.FromMilliseconds(450)),
            };

            if (opacity > 0)
            {
                animation.Completed += (s, t) =>
                {
                    ScatterView.Visibility = (sviCredits.Opacity > 0) ? Visibility.Visible : Visibility.Collapsed;
                };
            }
            else
            {
                ScatterView.Visibility = Visibility.Visible;
            }

            sviCredits.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// Lancement de l'application de l'expo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchExpo(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    ExpoWindow fenetre = new ExpoWindow();
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Lancement de l'application de memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchMemory(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    MemoryMainWindow fenetre = new MemoryMainWindow();
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Lancement de l'application de puzzle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchPuzzle(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    PuzzleMainWindow fenetre = new PuzzleMainWindow();
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Lancement de l'application de region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchRegion(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    AppRegionMainView fenetre = new AppRegionMainView();
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Lancement de l'application de Frise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchFrise(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    AppFriseMainView fenetre = new AppFriseMainView();
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Lancement de l'application de region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchBanqueImages(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    AppBanqueImagesMainView fenetre = new AppBanqueImagesMainView();
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Lancement de l'application de visite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void LaunchVisite(object sender, EventArgs e)
        {
            // Garbage Collector
            GC.Collect();
            GC.WaitForFullGCComplete();

            if (isopen == 0)
            {
                isopen = 1;
                //lancement de l'animation avant de fermer
                Storyboard.SetTarget(sbHideAnimSec, canvas);
                sbHideAnimSec.Completed += (s, t) =>
                {
                    VisiteWindow fenetre = new VisiteWindow(sender, e);
                    fenetre.Show();
                    this.Close();
                };
                sbHideAnimSec.Begin();
            }
        }

        /// <summary>
        /// Fermeture de l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion Private Methods
    }
}