using CommonSurface.Model;
using CommonSurface.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading;
using System.Xml;

namespace AppPalaisRois.ViewModel
{
    public class PuzzleViewModel : ViewModelBase
    {
        #region Private Fields

        // Chemin du xml du puzzle
        private static string defaultPath = ConfigurationManager.AppSettings["cheminPuzzle"];

        // Visibilité du choix
        private string _choiceVisibility;

        // Boolean en train de jouer
        private bool _isPlaying;

        // Collection des puzzles
        private ObservableCollection<Puzzle> _PuzzlesEasy;

        private ObservableCollection<Puzzle> _PuzzlesHard;

        private ObservableCollection<Puzzle> _PuzzlesMedium;

        // Selection
        private int _SelectedIndex;

        private Puzzle _SelectedPuzzle;

        // Etoiles
        private string _Star1Control;

        private string _Star2Control;

        private string _Star3Control;

        private string _timerGame;

        // Visibilité du puzzle 12 pieces
        private string _visibilityC12;

        // Visibilité du puzzle 6 pieces
        private string _visibilityC6;

        // Visibilité de la zone de jeu
        private string _VisibilityScatterPiece;

        private Chronometre chrono;

        private string starFull = ConfigurationManager.AppSettings["cheminEtoileObtenue"];

        // Liens des images étoiles
        private string starNone = ConfigurationManager.AppSettings["cheminEtoileNonObtenue"];

        // Déclaration du thread timer
        private Thread timerThread;

        #endregion Private Fields

        #region Public Constructors

        public PuzzleViewModel()
        {
            // Création collection des puzzles
            this.PuzzlesEasy = new ObservableCollection<Puzzle>();
            this.PuzzlesMedium = new ObservableCollection<Puzzle>();
            this.PuzzlesHard = new ObservableCollection<Puzzle>();
            chrono = new Chronometre();

            XmlDocument doc = new XmlDocument();
            doc.Load(defaultPath);
            XmlNodeList difficulties = doc.SelectNodes("//Difficulty");
            foreach (XmlNode difficulty in difficulties)
            {
                XmlNode pictures = difficulty.FirstChild;

                foreach (XmlNode puzzle in pictures.ChildNodes)
                {
                    Media m = new Media(puzzle.Attributes["Name"].Value, puzzle.Attributes["Source"].Value, MediaType.IMAGE);
                    switch (difficulty.Attributes["Type"].Value)
                    {
                        case "Easy":
                            PuzzlesEasy.Add(new Puzzle(m, Convert.ToInt32(difficulty.Attributes["Size"].Value), Puzzle.EASY));
                            break;

                        case "Medium":
                            PuzzlesMedium.Add(new Puzzle(m, Convert.ToInt32(difficulty.Attributes["Size"].Value), Puzzle.MEDIUM));
                            break;

                        case "Hard":
                            PuzzlesHard.Add(new Puzzle(m, Convert.ToInt32(difficulty.Attributes["Size"].Value), Puzzle.HARD));
                            break;

                        default:
                            break;
                    }
                }
            }

            // Instanciation du thread Timer, on spécifie dans le délégué ThreadStart le nom de la
            // méthode qui sera exécutée lorsque l'on appele la méthode Start() de notre thread.
            timerThread = new Thread(new ThreadStart(TimerLoop));
            // Lancement du thread
            timerThread.Start();

            // Ouverture liste puzzles
            Choice();
        }

        #endregion Public Constructors

        #region Private Destructors

        ~PuzzleViewModel()
        {
            // Etoiles
            starNone = starFull = null;
            _Star1Control = _Star2Control = _Star3Control = null;

            // Timer
            chrono.Stop();
            chrono = null;
            timerThread.Abort();
            timerThread = null;

            // Visibilités
            _visibilityC6 = _visibilityC12 = _VisibilityScatterPiece = _choiceVisibility = null;

            // Selection
            _SelectedIndex = 0;
            _SelectedPuzzle = null;
            _isPlaying = false;

            // Puzzles
            _PuzzlesEasy.Clear();
            _PuzzlesMedium.Clear();
            _PuzzlesHard.Clear();
            _PuzzlesEasy = _PuzzlesEasy = _PuzzlesHard = null;
        }

        #endregion Private Destructors

        #region Public Properties

        public string ChoiceVisibility
        {
            get { return _choiceVisibility; }
            set
            {
                _choiceVisibility = value;
                OnPropertyChanged("ChoiceVisibility");
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }

        public ObservableCollection<Puzzle> PuzzlesEasy
        {
            get { return _PuzzlesEasy; }
            set
            {
                _PuzzlesEasy = value;
                OnPropertyChanged("PuzzlesEasy");
            }
        }

        public ObservableCollection<Puzzle> PuzzlesHard
        {
            get { return _PuzzlesHard; }
            set
            {
                _PuzzlesHard = value;
                OnPropertyChanged("PuzzlesHard");
            }
        }

        public ObservableCollection<Puzzle> PuzzlesMedium
        {
            get { return _PuzzlesMedium; }
            set
            {
                _PuzzlesMedium = value;
                OnPropertyChanged("PuzzlesMedium");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public Puzzle SelectedPuzzle
        {
            get
            {
                return _SelectedPuzzle;
            }
            set
            {
                // On enleve les étoiles
                Star1Control = Star2Control = Star3Control = null;

                if (_SelectedPuzzle != null)
                    foreach (Piece p in _SelectedPuzzle.Pieces) p.NotPlaced = true;

                if (value != null)
                {
                    _SelectedPuzzle = value;
                    VisibilityScatterPiece = "Visible";
                    ChoiceVisibility = "Hidden";

                    switch (value.Taille)
                    {
                        case 6:
                            visibilityC6 = "Visible";
                            visibilityC12 = "Hidden";
                            break;

                        case 12:
                            visibilityC6 = "Hidden";
                            visibilityC12 = "Visible";
                            break;
                    }
                    IsPlaying = true;
                    OnPropertyChanged("SelectedPuzzle");

                    // On démarre le chrono
                    chrono.Go();
                }
            }
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

        public string visibilityC12
        {
            get
            {
                return _visibilityC12;
            }
            set
            {
                _visibilityC12 = value;
                OnPropertyChanged("visibilityC12");
            }
        }

        public string visibilityC6
        {
            get
            {
                return _visibilityC6;
            }
            set
            {
                _visibilityC6 = value;
                OnPropertyChanged("visibilityC6");
            }
        }

        public string VisibilityScatterPiece
        {
            get { return _VisibilityScatterPiece; }
            set
            {
                _VisibilityScatterPiece = value;
                if (timerThread.IsAlive == true)
                {
                    chrono.Reset();
                }
                OnPropertyChanged("VisibilityScatterPiece");
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Fonction de fin jeu
        /// </summary>
        public void CheckGameEnd()
        {
            int nb = 0;
            foreach (Piece p in _SelectedPuzzle.Pieces)
                if (!p.NotPlaced) nb++;
            if (_SelectedPuzzle.Pieces.Count == nb)
            {
                // On arrete le chronometre
                chrono.Stop();
                // On affiche les étoiles de score
                Star1Control = starFull;
                Star2Control = (chrono.Elapsed.TotalSeconds < ((SelectedPuzzle.Pieces.Count == 6) ? 30 : 65)) ? starFull : starNone;
                Star3Control = (chrono.Elapsed.TotalSeconds < ((SelectedPuzzle.Pieces.Count == 6) ? 18 : 50)) ? starFull : starNone;
            }
        }

        // Ouverture de la liste des puzzles
        public void Choice()
        {
            SelectedIndex = -1;
            SelectedPuzzle = null;
            ChoiceVisibility = "Visible";
            VisibilityScatterPiece = "Hidden";
            visibilityC6 = "Hidden";
            visibilityC12 = "Hidden";
            IsPlaying = false;
        }

        // On arrête le chrono
        public void quit()
        {
            timerThread.Abort();
            chrono.Stop();
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
    }
}