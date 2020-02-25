using CommonSurface.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppPalaisRois.ViewModel
{
    public class Puzzle : INotifyPropertyChanged
    {
        #region Public Fields

        public static string EASY = "#71EA00";

        public static string HARD = "#FF1A00";

        public static string MEDIUM = "#F48600";

        #endregion Public Fields

        #region Private Fields

        private string _levelColor;

        private Media media;

        // Collection de pieces du puzzle
        private ObservableCollection<Piece> pieces;

        private int taille;

        #endregion Private Fields

        #region Public Constructors

        public Puzzle(Media m, int taille) : this(m, taille, EASY)
        {
        }

        public Puzzle(Media m, int taille, string color)
        {
            media = m;
            Taille = taille;
            LevelColor = color;
            Pieces = new ObservableCollection<Piece>();

            int centerX = Rows * 150 / 2;
            int centerY = Columns * 150 / 2;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Piece p;
                    if (r == 0)
                    {
                        if (c == 0)
                        {
                            p = new Piece(Piece.Type.TOPLEFT, this.ImgSrc, this.taille);
                        }
                        else
                        {
                            if (c == Columns - 1)
                            {
                                p = new Piece(Piece.Type.TOPRIGHT, this.ImgSrc, this.taille);
                            }
                            else
                            {
                                p = new Piece(Piece.Type.TOP, this.ImgSrc, this.taille);
                            }
                        }
                    }
                    else
                    {
                        if (r == Rows - 1)
                        {
                            if (c == 0)
                            {
                                p = new Piece(Piece.Type.BOTLEFT, this.ImgSrc, this.taille);
                            }
                            else
                            {
                                if (c == Columns - 1)
                                {
                                    p = new Piece(Piece.Type.BOTRIGHT, this.ImgSrc, this.taille);
                                }
                                else
                                {
                                    p = new Piece(Piece.Type.BOTTOM, this.ImgSrc, this.taille);
                                }
                            }
                        }
                        else
                        {
                            if (c == 0)
                            {
                                p = new Piece(Piece.Type.LEFT, this.ImgSrc, this.taille);
                            }
                            else
                            {
                                if (c == Columns - 1)
                                {
                                    p = new Piece(Piece.Type.RIGHT, this.ImgSrc, this.taille);
                                }
                                else
                                {
                                    p = new Piece(Piece.Type.CENTER, this.ImgSrc, this.taille);
                                }
                            }
                        }
                    }

                    p.XClip = (150 * c) - p.leftOffset;
                    p.YClip = (150 * r) - p.topOffset;
                    Pieces.Add(p);
                }
            }
        }

        #endregion Public Constructors

        #region Private Destructors

        ~Puzzle()
        {
            PropertyChanged = null;
            _levelColor = null;
            media = null;
            taille = 0;
            EASY = MEDIUM = HARD = null;
            pieces = null;
        }

        #endregion Private Destructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Getter & Setter

        public int Columns
        {
            get
            {
                switch (taille)
                {
                    case 6: return 3;
                    case 12: return 4;
                    default: return 0;
                }
            }
        }

        public string ImgSrc
        {
            get { return media.Path; }
            set
            {
                media.Path = value;
                OnPropertyChanged("ImgSrc");
            }
        }

        public string LevelColor
        {
            get
            {
                return _levelColor;
            }
            set
            {
                _levelColor = value;
                OnPropertyChanged("LevelColor");
            }
        }

        public ObservableCollection<Piece> Pieces
        {
            get
            {
                return pieces;
            }
            set
            {
                pieces = value;
                OnPropertyChanged("Pieces");
            }
        }

        public int Rows
        {
            get
            {
                switch (taille)
                {
                    case 6: return 2;
                    case 12: return 3;
                    default: return 0;
                }
            }
        }

        public int Taille
        {
            get { return taille; }
            set { taille = value; }
        }

        #endregion Getter & Setter

        #region Public Methods

        public void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion Public Methods
    }
}