using AppPalaisRois.ViewModel;
using CommonSurface.Other;
using CommonSurface.Utils;
using Microsoft.Surface.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace AppPalaisRois
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PuzzleMainWindow : Window
    {
        #region Private Fields

        private PuzzleViewModel _puzzleViewModel;

        /* EFFECTS RESOURCE DICTIONARY */
        private bool canClose = true;
        private double HAUTEUR_ECRAN = SystemParameters.PrimaryScreenHeight;
        private double LARGEUR_ECRAN = SystemParameters.PrimaryScreenWidth;
        private int loadedMedias;
        private List<MediaElement> medias;
        private ResourceDictionary myresourcedictionary;
        private Storyboard sbHide, sbHelp;
        private Temps tps = new Temps();
        private string CheminFrise = ConfigurationManager.AppSettings["CheminFrise"];
        private string CheminBoutonReturn = ConfigurationManager.AppSettings["CheminBoutonReturn"];

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PuzzleMainWindow()
        {
            PuzzleViewModel pvm = new PuzzleViewModel();
            this.DataContext = pvm;
            medias = new List<MediaElement>();
            loadedMedias = 0;
            InitializeComponent();

            // Récupération de la frise
            frisePuzzle.Source = ResourceAccessor.loadImage(CheminFrise);

            // Récupération du retour
            returnPuzzle.Source = ResourceAccessor.loadImage(CheminBoutonReturn);

            // Récupération du sablier
            sablier.Source = ResourceAccessor.loadImage("/CommonSurface;component/Resources/Sablier.png");

            // Ajout des jeux dans la liste des choix Bouton facile Icône
            ChoixPuzzle.Children.Add(new BoutonBarreMenu(new RoutedEventHandler(PuzzleEasy_click), ConfigurationManager.AppSettings["cheminIconeJeu1Puzzle"]).getButton());
            // Nom
            labelEasy.Content = ConfigurationManager.AppSettings["valeurNomJeu1Puzzle"];

            // Bouton moyen Icône
            ChoixPuzzle.Children.Add(new BoutonBarreMenu(new RoutedEventHandler(PuzzleMedium_click), ConfigurationManager.AppSettings["cheminIconeJeu2Puzzle"]).getButton());
            // Nom
            labelMedium.Content = ConfigurationManager.AppSettings["valeurNomJeu2Puzzle"];

            //Bouton difficile
            // Icône
            ChoixPuzzle.Children.Add(new BoutonBarreMenu(new RoutedEventHandler(PuzzleHard_click), ConfigurationManager.AppSettings["cheminIconeJeu3Puzzle"]).getButton());
            // Nom
            labelHard.Content = ConfigurationManager.AppSettings["valeurNomJeu3Puzzle"];

            /* EFFECTS RESOURCE DICTIONARY */
            myresourcedictionary = new ResourceDictionary();
            myresourcedictionary.Source = new Uri("/CommonSurface;component/XAML/Effects.xaml", UriKind.RelativeOrAbsolute);
            sbHide = myresourcedictionary["hideAnimSec"] as Storyboard;
        }

        #endregion Public Constructors

        #region Private Destructors

        ~PuzzleMainWindow()
        {
            sbHide = sbHelp = null;
        }

        #endregion Private Destructors

        #region Public Properties

        public PuzzleViewModel PuzzleViewModel
        {
            get { return _puzzleViewModel; }
            set { _puzzleViewModel = value; }
        }

        #endregion Public Properties

        #region Private Methods

        private void AnimPiece(ScatterViewItem piece)
        {
            Storyboard storyBoard = ((Storyboard)Resources["AnimPiece"]).Clone();
            foreach (AnimationTimeline a in storyBoard.Children)
            {
                DoubleAnimation dA = a as DoubleAnimation;
                if (dA != null)
                {
                    dA.To = 0.0;
                    if ((dA.From = piece.Orientation) > 180) dA.From -= 360;
                }

                PointAnimation pA = a as PointAnimation;
                if (pA != null)
                {
                    pA.To = new Point((scatterPiece.ActualWidth / 2), (scatterPiece.ActualHeight / 2));
                }
            }

            piece.BeginStoryboard(storyBoard);
        }

        // Sortie de l'application Puzzle
        private void BoutonQuit_click(object sender, RoutedEventArgs e)
        {
            if (canClose)
            {
                canClose = false;
                int fin = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                tps.TpsPuzzle = (fin - tps.DebutPuzzle) + tps.TpsPuzzle;
                ((PuzzleViewModel)this.DataContext).quit();

                //Effet de partir
                Storyboard.SetTarget(sbHide, canvas);
                sbHide.Completed += (s, t) =>
                {
                    // Clean
                    _puzzleViewModel = null;
                    tps = null;
                    DataContext = null;
                    medias.Clear();
                    medias = null;
                    myresourcedictionary.Clear();
                    myresourcedictionary = null;
                    loadedMedias = 0;
                    HAUTEUR_ECRAN = LARGEUR_ECRAN = 0;

                    // Garbage Collector
                    GC.Collect();
                    GC.WaitForFullGCComplete();

                    //Fermeture et ouverture des fenetres
                    MainWindow fenetre = new MainWindow();
                    fenetre.Show();
                    this.Close();
                };
                sbHide.Begin();
            }
        }

        private void MediaElementEnded(object sender, RoutedEventArgs e)
        {
            MediaElement m = sender as MediaElement;
            if (m.HasVideo)
            {
                m.Play();
            }
        }

        private void MediaElementLoaded(object sender, RoutedEventArgs e)
        {
            MediaElement m = sender as MediaElement;
            if (m != null)
            {
                m.Play();
                if (m.HasVideo)
                {
                    medias.Add(m);
                    loadedMedias++;

                    StartVideo();
                }
            }
        }

        private void OnManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem item = e.OriginalSource as ScatterViewItem;
            ((Piece)item.DataContext).ScatterCenter = item.ActualCenter;
            if (((Piece)item.DataContext).IsInPosition(scatterPiece.ActualWidth, scatterPiece.ActualHeight))
            {
                AnimPiece(item);
                item.IsTopmostOnActivation = false;
                item.ZIndex = 0;
                ((Piece)item.DataContext).NotPlaced = false;
                ((PuzzleViewModel)this.DataContext).CheckGameEnd();
            }
        }

        #endregion Private Methods

        #region Choix de la difficulté

        /// <summary>
        /// Clic sur le puzzle facile
        /// </summary>
        private void PuzzleEasy_click(object sender, RoutedEventArgs e)
        {
            labelEasy.Visibility = Visibility.Collapsed;
            labelMedium.Visibility = Visibility.Collapsed;
            labelHard.Visibility = Visibility.Collapsed;
            ((PuzzleViewModel)DataContext).SelectedPuzzle = null;
            //on choisit un puzzle facile
            //on regarde d'abord la taille de la collection de puzzle facile
            int taille = ((PuzzleViewModel)DataContext).PuzzlesEasy.Count;
            Random rand = new Random((int)DateTime.Now.Ticks);
            int RandPuzzle = rand.Next(0, taille);

            ((PuzzleViewModel)DataContext).SelectedPuzzle = ((PuzzleViewModel)DataContext).PuzzlesEasy.ElementAt(RandPuzzle);
            puzHelp.Source = new BitmapImage(new Uri(((PuzzleViewModel)DataContext).SelectedPuzzle.ImgSrc));
            sbHelp = myresourcedictionary["animHelp"] as Storyboard;
            Storyboard.SetTarget(sbHelp, puzHelp);
            sbHelp.Begin();

            // Image de fond
            imageFond.Source = puzHelp.Source;
            imageFond.Opacity = 0.2;
            imageFond.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Clic sur le puzzle hard
        /// </summary>
        private void PuzzleHard_click(object sender, RoutedEventArgs e)
        {
            labelEasy.Visibility = Visibility.Collapsed;
            labelMedium.Visibility = Visibility.Collapsed;
            labelHard.Visibility = Visibility.Collapsed;
            ((PuzzleViewModel)DataContext).SelectedPuzzle = null;
            //on choisit un puzzle hard
            //on regarde d'abord la taille de la collection de puzzle hard
            int taille = ((PuzzleViewModel)DataContext).PuzzlesHard.Count;
            Random rand = new Random((int)DateTime.Now.Ticks);
            int RandPuzzle = rand.Next(0, taille);

            ((PuzzleViewModel)DataContext).SelectedPuzzle = ((PuzzleViewModel)DataContext).PuzzlesHard.ElementAt(RandPuzzle);
            puzHelp.Source = new BitmapImage(new Uri(((PuzzleViewModel)DataContext).SelectedPuzzle.ImgSrc));
            sbHelp = myresourcedictionary["animHelp"] as Storyboard;
            Storyboard.SetTarget(sbHelp, puzHelp);
            sbHelp.Begin();
        }

        /// <summary>
        /// Clic sur le puzzle medium
        /// </summary>
        private void PuzzleMedium_click(object sender, RoutedEventArgs e)
        {
            labelEasy.Visibility = Visibility.Collapsed;
            labelMedium.Visibility = Visibility.Collapsed;
            labelHard.Visibility = Visibility.Collapsed;
            ((PuzzleViewModel)DataContext).SelectedPuzzle = null;
            //on choisit un puzzle medium
            //on regarde d'abord la taille de la collection de puzzle medium
            int taille = ((PuzzleViewModel)DataContext).PuzzlesMedium.Count;
            Random rand = new Random((int)DateTime.Now.Ticks);
            int RandPuzzle = rand.Next(0, taille);

            ((PuzzleViewModel)DataContext).SelectedPuzzle = ((PuzzleViewModel)DataContext).PuzzlesMedium.ElementAt(RandPuzzle);
            puzHelp.Source = new BitmapImage(new Uri(((PuzzleViewModel)DataContext).SelectedPuzzle.ImgSrc));
            sbHelp = myresourcedictionary["animHelp"] as Storyboard;
            Storyboard.SetTarget(sbHelp, puzHelp);
            sbHelp.Begin();
        }

        #endregion Choix de la difficulté

        private void StartVideo()
        {
            if (loadedMedias == ((PuzzleViewModel)this.DataContext).SelectedPuzzle.Taille)
            {
                foreach (MediaElement m in medias)
                {
                    TimeSpan time = medias.First().Position;
                    m.Position = time;
                }
            }
        }
    }
}