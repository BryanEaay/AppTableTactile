using CommonSurface.DAO;
using CommonSurface.Model;
using CommonSurface.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AppPalaisRois.ViewModel
{
    internal class MemoryViewModel : ViewModelBase
    {
        #region Public Fields

        // Déclaration du thread timer
        public Thread timerThread;

        #endregion Public Fields

        #region Private Fields

        private ObservableCollection<PlayableCard> _cards;

        private string _ChoiceVisibility;

        private int _gameSize;

        // Booleen En train de jouer
        private bool _isplaying;

        private string _ItemsControlVisibility;

        private int _selectedGame;

        private string _Star1Control;

        private string _Star2Control;

        private string _Star3Control;

        private string _timerGame;

        // Collection des cartes à retourner
        private List<string> Backgrounds;

        private bool checking = false;

        private Chronometre chrono;

        private DispatcherTimer dispatcherTimer;

        private ObservableCollection<PlayableCard> FlipedCards;

        private bool gameEnd;

        // Liens des images étoiles
        private string starFull = ConfigurationManager.AppSettings["cheminEtoileObtenue"];

        private string starNone = ConfigurationManager.AppSettings["cheminEtoileNonObtenue"];

        #endregion Private Fields

        #region Public Constructors

        public MemoryViewModel()
        {
            // Chronomètre
            chrono = new Chronometre();

            // Liste des fonds memory
            Backgrounds = new List<string>();
            foreach (Picture m in DAOMemory.Instance.Backgrounds)
            {
                Backgrounds.Add(m.Source);
            }

            // Timer utilisé pour retourner les cartes lors d'un tour
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            // Instanciation du thread Timer, on spécifie dans le délégué ThreadStart le nom de la
            // méthode qui sera exécutée lorsque l'on appele la méthode Start() de notre thread.
            timerThread = new Thread(new ThreadStart(TimerLoop));
            timerThread.Start();

            // On affiche les choix
            Choice();
        }

        #endregion Public Constructors

        #region Private Destructors

        ~MemoryViewModel()
        {
            chrono.Stop();
            chrono = null;
            dispatcherTimer.Stop();
            dispatcherTimer = null;
            timerThread.Abort();
            timerThread = null;
            _cards = null;
            _gameSize = 0;
            _ChoiceVisibility = _ItemsControlVisibility = null;
            _selectedGame = 0;
            _Star1Control = _Star2Control = _Star3Control = null;
            _timerGame = null;
            if (Backgrounds != null) Backgrounds.Clear();
            Backgrounds = null;
            if (FlipedCards != null) FlipedCards.Clear();
            FlipedCards = null;
            starNone = starFull = null;
        }

        #endregion Private Destructors

        #region Getter & Setter

        public ObservableCollection<PlayableCard> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;
                OnPropertyChanged("Cards");
            }
        }

        public bool Checking
        {
            get
            {
                return checking;
            }
            set
            {
                checking = value;
            }
        }

        public string ChoiceVisibility
        {
            get
            {
                return _ChoiceVisibility;
            }
            set
            {
                _ChoiceVisibility = value;
                OnPropertyChanged("ChoiceVisibility");
            }
        }

        public bool GameEnd
        {
            get
            {
                return gameEnd;
            }
            set
            {
                gameEnd = value;
                OnPropertyChanged("GameEnd");
            }
        }

        public int GameSize
        {
            get
            {
                return _gameSize;
            }
            set
            {
                _gameSize = value;
                OnPropertyChanged("GameSize");
            }
        }

        public bool IsPlaying
        {
            get { return _isplaying; }
            set { _isplaying = value; }
        }

        public string ItemsControlVisibility
        {
            get
            {
                return _ItemsControlVisibility;
            }
            set
            {
                _ItemsControlVisibility = value;
                if (timerThread.IsAlive == true)
                {
                    chrono.Reset();
                }
                OnPropertyChanged("ItemsControlVisibility");
            }
        }

        public int SelectedGame
        {
            get { return _selectedGame; }
            set { _selectedGame = value; }
        }

        public string Star1Control
        {
            get { return _Star1Control; }
            set
            {
                _Star1Control = value;
                OnPropertyChanged("Star1Control");
            }
        }

        public string Star2Control
        {
            get { return _Star2Control; }
            set
            {
                _Star2Control = value;
                OnPropertyChanged("Star2Control");
            }
        }

        public string Star3Control
        {
            get { return _Star3Control; }
            set
            {
                _Star3Control = value;
                OnPropertyChanged("Star3Control");
            }
        }

        public string TimerGame
        {
            get
            {
                return _timerGame;
            }
            set
            {
                _timerGame = value;
                OnPropertyChanged("TimerGame");
            }
        }

        #endregion Getter & Setter

        #region Public Methods

        public void Check()
        {
            Checking = true;

            if (FlipedCards[0].Id == FlipedCards[1].Id)
            {
                FlipedCards[0].Solved = FlipedCards[1].Solved = true;
                FlipedCards.Clear();
                Checking = false;

                // Si le jeu est fini
                if (GameEnd = CheckGameEnd())
                {
                    // On arrete le chronometre
                    chrono.Stop();
                    // On fade les pieces
                    foreach (PlayableCard c in Cards)
                    {
                        c.MyRadius = new CornerRadius(0);
                        c.Fade = false;
                        c.GameEnd = true;
                    }

                    // On affiche les étoiles de score
                    Star1Control = starFull;
                    Star2Control = (chrono.Elapsed.TotalSeconds < ((Cards.Count > 16) ? 150 : 50)) ? starFull : starNone;
                    Star3Control = (chrono.Elapsed.TotalSeconds < ((Cards.Count > 16) ? 120 : 30)) ? starFull : starNone;
                }
            }
            else
            {
                dispatcherTimer.Start();
            }
        }

        /// <summary>
        /// Ouvrir la liste des choix
        /// </summary>
        public void Choice()
        {
            ItemsControlVisibility = "Hidden";
            ChoiceVisibility = "Visible";
        }

        public Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

        // Retourne image coupé
        public ObservableCollection<BitmapImage> CropImage(int cut, string chemin)
        {
            ObservableCollection<BitmapImage> temp = new ObservableCollection<BitmapImage>();
            ObservableCollection<Bitmap> bmp = new ObservableCollection<Bitmap>();
            Image model;
            try
            {
                model = Image.FromFile(chemin);
            }
            catch
            {
                model = Image.FromFile(ConfigurationManager.AppSettings["cheminDefautArriereMemory"]);
            }
            Bitmap dImg = new Bitmap(model);
            int cote = (dImg.Width < dImg.Height) ? dImg.Width : dImg.Height;
            for (int i = 0; i < cut; i++)
            {
                for (int j = 0; j < cut; j++)
                {
                    bmp.Add(CropBitmap(dImg, j * (cote / cut), i * (cote / cut), cote / cut, cote / cut));
                }
            }
            foreach (Bitmap b in bmp)
            {
                MemoryStream ms = new MemoryStream();
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                BitmapImage bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(ms.ToArray());
                bImg.EndInit();
                temp.Add(bImg);
            }
            return temp;
        }

        public void FlipCard(PlayableCard c)
        {
            if (!Checking)
            {
                if (!c.Front)
                {
                    c.Front = true;
                    FlipedCards.Add(c);
                }
                if (FlipedCards.Count > 1)
                {
                    Check();
                }
            }
        }

        /// <summary>
        /// Création du nouveau jeu
        /// </summary>
        /// <param name="id">ID du jeu selectionné</param>
        public void LoadGame(string type)
        {
            // Collection des cartes à retourner
            Cards = new ObservableCollection<PlayableCard>();
            // Collection des cartes retournées
            FlipedCards = new ObservableCollection<PlayableCard>();

            FlipedCards.Clear();

            GameEnd = false;
            // On enleve les étoiles
            Star1Control = Star2Control = Star3Control = null;
            // On affiche la zone de jeu
            ItemsControlVisibility = "Visible";

            // On cache la zone de choix
            ChoiceVisibility = "Hidden";

            // On ajoute 2 fois l'image, le jeu fonctionnant par paires
            int id = 0;
            foreach (Picture c in DAOMemory.Instance.FindPictures(type))
            {
                Cards.Add(new PlayableCard(id, c));
                Cards.Add(new PlayableCard(id, c));
                id++;
            }

            // Découpe de l'image de fond
            Random backgroundRdm = new Random();
            int rdmIndex = backgroundRdm.Next(Backgrounds.Count);
            ObservableCollection<BitmapImage> fondImg =
                CropImage((int)Math.Sqrt(Cards.Count),
                (Backgrounds.Count > 0) ? Backgrounds[rdmIndex] : null);

            // On enregistre la taille du plateau de jeu
            GameSize = (int)Math.Sqrt(Cards.Count) * 120;

            // On mélange les cartes
            Random rdm = new Random();
            for (int i = Cards.Count - 1; i > 0; i--)
            {
                int n = rdm.Next(i + 1);
                PlayableCard temp = Cards[i];
                Cards[i] = Cards[n];
                Cards[i].ImageBmp = fondImg[i];
                Cards[n] = temp;
                Cards[n].ImageBmp = fondImg[n];
            }

            // On démarre le chronomètre
            chrono.Go();
        }

        public void quit()
        {
            timerThread.Abort();
            chrono.Stop();
            DAOMemory.Instance.Dispose();
        }

        // Cette méthode est appelé lors du lancement du thread C'est ici que le temps affiché dans
        // la balise texte va etre actualiser
        public void TimerLoop()
        {
            // Tant que le thread n'est pas tué, on travaille
            while (Thread.CurrentThread.IsAlive)
            {
                // Attente de 500 ms
                Thread.Sleep(500);
                // Affichage du temps
                TimerGame = chrono.GetFormatTime();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool CheckGameEnd()
        {
            bool returnValue = true;
            foreach (PlayableCard c in Cards)
            {
                if (c.Solved == false)
                {
                    returnValue = false;
                    break;
                }
            }
            return returnValue;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            // Si on a retourné + d'une carte
            if (FlipedCards.Count > 1)
            {
                FlipedCards[0].Front = FlipedCards[1].Front = false;
            }
            FlipedCards.Clear();
            Checking = false;
        }

        #endregion Private Methods
    }
}