using AppPalaisRois.ViewModel;
using CommonSurface.Model;
using CommonSurface.Other;
using Gecko;
using Gecko.Events;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AppPalaisRois
{
    /// <summary>
    /// Logique d'interaction pour VisiteWindow.xaml
    /// </summary>
    public partial class VisiteWindow : Window
    {
        #region Private Fields

        private string _actualScene = "";
        private bool _informationOpen = false;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private EventHandler<GeckoDocumentCompletedEventArgs> evLaunchTimer;
        private GeckoWebBrowser myBrowser;
        private bool quit = false;
        private ResourceDictionary resourceDictionary;
        private Storyboard sbHide;
        private VisiteViewModel ViewModel;
        private WindowsFormsHost whost;

        #endregion Private Fields

        /* EFFECTS RESOURCE DICTIONARY */
        /* EVENT HANDLER */

        #region Public Constructors

        public VisiteWindow(object sender, EventArgs e)
        {
            InitializeComponent();

            // Récupération du retour
            returnVisite.Source = ResourceAccessor.loadImage("/CommonSurface;component/Resources/return.png");
            infoVisite.Source = ResourceAccessor.loadImage(ConfigurationManager.AppSettings["cheminIconePanelInformation"]);

            // Initialise l'event event
            evLaunchTimer = new EventHandler<GeckoDocumentCompletedEventArgs>(launchTimer);
            ViewModel = new VisiteViewModel();

            // Fond pour le border
            BitmapDecoder decoder = BitmapDecoder.Create(
                    new Uri("pack://application:,,,/CommonSurface;component/Resources/FondTextBoxe.jpg", UriKind.RelativeOrAbsolute),
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad);
            ImageBrush brush = new ImageBrush(decoder.Frames[0]);
            bdrInfoMASK.Background = brush;

            /* EFFECTS RESOURCE DICTIONARY */
            resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("/CommonSurface;component/XAML/Effects.xaml", UriKind.RelativeOrAbsolute);
            sbHide = resourceDictionary["hideAnimSec"] as Storyboard;

            // On cache le bouton d'information
            Info_button.Visibility = Visibility.Hidden;
            this.DataContext = ViewModel;

            ShowImage(null, null, ViewModel.Visites[0]);
            dock_visite.SelectedIndex = 0;
        }

        #endregion Public Constructors

        #region Private Destructors

        /// <summary>
        /// Destructeur
        /// </summary>
        ~VisiteWindow()
        {
            _actualScene = "";
            ViewModel = null;
            sbHide = null;
            resourceDictionary = null;
            dispatcherTimer = null;
            evLaunchTimer = null;
        }

        #endregion Private Destructors

        #region Private Methods

        /// <summary>
        /// Bouton affichant les informations complémentaires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void BoutonInfo_click(object sender, RoutedEventArgs e)
        {
            //debug(sender, e);
            Storyboard sbMenu, sbWebBrowser;

            if (!this._informationOpen)
            {
                // Affichage de la barre d'information
                sbMenu = Resources["sbShowRightMenu"] as Storyboard;
                sbWebBrowser = Resources["sbReduceWebBrowser"] as Storyboard;
            }
            else
            {
                // On retir la barre d'information
                sbMenu = Resources["sbHideRightMenu"] as Storyboard;
                sbWebBrowser = Resources["sbExpandWebBrowser"] as Storyboard;
            }

            // Lancement de l'animation
            sbMenu.Begin(pnlRightMenu);
            sbWebBrowser.Begin(dock_photo_visite);

            // On inverse l'état de la vue
            _informationOpen = !_informationOpen;
        }

        // Retour et ferme la fenêtre de visite
        private void BoutonQuit_click(object sender, RoutedEventArgs e)
        {
            //Effet de fermeture
            if (!quit)
            {
                // Empêche la fermeture de l'application plusieurs fois
                quit = true;

                // Arrêt du timer
                dispatcherTimer.Stop();
                dispatcherTimer.Tick -= checkDomChanged;
                dispatcherTimer = null;

                // On retire l'hote de fenêtre
                whost.Dispose();
                whost = null;

                Storyboard.SetTarget(sbHide, appVisite);
                sbHide.Completed += (s, t) =>
                {
                    //Fermeture et ouverture des fenetres
                    MainWindow fenetre = new MainWindow();
                    fenetre.Show();
                    DataContext = null;
                    ViewModel = null;
                    this.Close();
                };
                sbHide.Begin();
            }
        }

        /// <summary>
        /// Détecter lorsqu'on change de scène
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void checkDomChanged(object sender, EventArgs e)
        {
            string scene = getActualScene();

            if (scene != this._actualScene)
            {
                // On enregistre la scène actuelle
                this._actualScene = scene;
                ViewModel.FindActualPanorama(scene);

                if (ViewModel.ActualPanorama != null)
                {
                    Info_button.Visibility = Visibility.Visible;
                }
                else
                {
                    Info_button.Visibility = Visibility.Hidden;

                    if (this._informationOpen)
                    {
                        // Affichage de la barre d'information
                        Storyboard sbMenu = Resources["sbHideRightMenu"] as Storyboard;
                        Storyboard sbWebBrowser = Resources["sbExpandWebBrowser"] as Storyboard;

                        sbMenu.Begin(pnlRightMenu);
                        sbWebBrowser.Begin(dock_photo_visite);

                        _informationOpen = false;
                    }
                }
            }

            dispatcherTimer.Start();
        }

        /// <summary>
        /// Détection d'un clic sur la visite virtuelle
        /// </summary>
        private void clickDetection(object sender, RoutedEventArgs e)
        {
            if (this._informationOpen)
            {
                BoutonInfo_click(sender, e);
            }
        }

        /// <summary>
        /// Changement de visite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void dock_visite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Selected = dock_visite.SelectedItem as Visite;
            ShowImage(sender, e, ViewModel.Selected);
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
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
                    // MessageBox.Show("La récupération du panorama a échoué.", "Erreur",
                    // MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            return "";
        }

        private void launchTimer(object sender, EventArgs e)
        {
            myBrowser.DocumentCompleted -= evLaunchTimer;
            evLaunchTimer = null;

            // Timer sur 100ms
            dispatcherTimer.Tick += checkDomChanged;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Relance le média lorsqu'il est finis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Media_Ended(object sender, RoutedEventArgs e)
        {
            MediaElement media = (MediaElement)sender;
            media.Position = new TimeSpan(0, 0, 0, 1);
            media.Play();
        }

        /// <summary>
        /// Affichage de la visite
        /// </summary>
        /// <param name="senderImg"></param>
        /// <param name="f">        </param>
        /// <param name="visite">   </param>
        private void ShowImage(object senderImg, EventArgs f, Visite visite)
        {
            //nettoyer le contenue
            grid.Children.Clear();
            dock_photo_visite.Children.Clear();

            try
            {
                // Déclaration des espaces de textes
                TextBlock textblocktitle = new TextBlock();
                TextBlock textblockAll = new TextBlock();
                TextBlock textblockSource = new TextBlock();
                Grid grid1 = new Grid();
                Grid grid3 = new Grid();

                if (myBrowser != null)
                {
                    myBrowser.Stop();
                    myBrowser.Dispose();
                    myBrowser = null;
                }

                myBrowser = new GeckoWebBrowser();

                // Déclaration de l'elements média
                Image img = new Image();
                BitmapImage imgAdd = new BitmapImage();

                // Inserer le code pour le lancé en cliquent dans le slide
                myBrowser.Navigate(visite.Cover);
                myBrowser.Height = (int)this.dock_photo_visite.Height;
                myBrowser.Width = (int)this.dock_photo_visite.Width;

                // Image de fond du titre
                BitmapDecoder decoder = BitmapDecoder.Create(
                    new Uri("pack://application:,,,/CommonSurface;component/Resources/FondTextBoxe.jpg", UriKind.RelativeOrAbsolute),
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad);
                ImageBrush brush = new ImageBrush(decoder.Frames[0]);
                this.MASK3.Background = brush;

                // Grid titre
                grid1.Margin = new Thickness(10, 30, 10, 30);
                grid1.HorizontalAlignment = HorizontalAlignment.Center;
                grid1.VerticalAlignment = VerticalAlignment.Top;

                // Titre
                textblocktitle.Visibility = Visibility.Visible;
                textblocktitle.FontSize = 40;
                textblocktitle.Foreground = new SolidColorBrush(Colors.White);
                textblocktitle.Text = visite.Title;
                textblocktitle.HorizontalAlignment = HorizontalAlignment.Center;
                textblocktitle.TextAlignment = TextAlignment.Justify;
                textblocktitle.TextWrapping = TextWrapping.WrapWithOverflow;

                //grid Source
                grid3.Margin = new Thickness(10, 55, 10, 10);
                grid3.HorizontalAlignment = HorizontalAlignment.Center;
                grid3.VerticalAlignment = VerticalAlignment.Bottom;

                //titre Source
                textblockSource.Visibility = Visibility.Visible;
                textblockSource.FontSize = 15;
                textblockSource.Foreground = new SolidColorBrush(Colors.White);
                textblockSource.Text = visite.Source;
                textblockSource.HorizontalAlignment = HorizontalAlignment.Center;
                textblockSource.TextAlignment = TextAlignment.Justify;
                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;

                //ajoute de tous les textes blocks
                grid1.Children.Add(textblocktitle);
                grid3.Children.Add(textblockSource);

                // ajoute de tous les grid
                grid.Children.Add(grid1);
                grid.Children.Add(grid3);

                // Lance le Timer lorsque la page est chargée
                myBrowser.DocumentCompleted += evLaunchTimer;

                // Insertion du GeckoWebBrowser dans un FormHost pour pouvoir l'intégrer sur le WPF
                if (whost != null)
                {
                    whost.Child = null;
                    whost.Dispose();
                    whost = null;
                }
                whost = new WindowsFormsHost();
                whost.Child = myBrowser;
                dock_photo_visite.Children.Add(whost);
            }
            catch
            {
                TextBlock textblockSource = new TextBlock
                {
                    //texte erreur de type de fichier
                    Visibility = Visibility.Visible,
                    FontSize = 30,
                    Foreground = new SolidColorBrush(Colors.Red),
                    Text = "Le type de fichier n'est pas lisible",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Justify,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };

                //ajout de l'element créé pour le panorama
                dock_photo_visite.Children.Add(textblockSource);
            }
        }

        #endregion Private Methods
    }
}