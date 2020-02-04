using CommonSurface.Model;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AppPalaisRois.ViewModel
{
    internal class PlayableCard : Carte
    {
        #region Private Fields

        private bool _front;
        private bool _GameEnd;

        private BitmapImage _imagebmp;
        private CornerRadius _MyRadius;

        private bool _solved;
        private bool fade;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructeur
        /// </summary>
        public PlayableCard(Carte c)
            : base(c.Id, c.Chemin, c.Nom)
        {
            Solved = false;
            MyRadius = new CornerRadius(6);
            Front = false;
            GameEnd = false;
            Fade = false;
        }

        public PlayableCard(int id, Picture p)
            : base(id, p.Source, p.Name)
        {
            Solved = false;
            MyRadius = new CornerRadius(6);
            Front = false;
            GameEnd = false;
            Fade = false;
        }

        #endregion Public Constructors

        #region Private Destructors

        ~PlayableCard()
        {
            _imagebmp = null;
        }

        #endregion Private Destructors

        #region Getter & Setter

        public BitmapImage BmpSource
        {
            get
            {
                if (Solved)
                    return ImageBmp;
                if (Front)
                    return new BitmapImage(
                        new Uri(base.Chemin, UriKind.RelativeOrAbsolute));
                else
                    return new BitmapImage(
                        new Uri(ConfigurationManager.AppSettings["cheminArriereMemory"], UriKind.RelativeOrAbsolute));
            }
        }

        new public string Chemin
        {
            get
            {
                if (Front)
                    return base.Chemin;
                else
                    return ConfigurationManager.AppSettings["cheminArriereMemory"];
            }
            set
            {
                base.Chemin = value;
                OnPropertyChanged("Chemin");
                OnPropertyChanged("BmpSource");
            }
        }

        public bool Fade
        {
            get
            {
                return fade;
            }
            set
            {
                fade = value;
                OnPropertyChanged("Fade");
            }
        }

        public double Fading
        {
            get
            {
                return (Fade) ? 0.0 : 1.0;
            }
        }

        public bool Front
        {
            get
            {
                if (Solved) return true;
                else return _front;
            }
            set
            {
                _front = value;
                OnPropertyChanged("Chemin");
                OnPropertyChanged("Front");
                OnPropertyChanged("BmpSource");
            }
        }

        public bool GameEnd
        {
            get { return _GameEnd; }
            set
            {
                _GameEnd = value;
                OnPropertyChanged("GameEnd");
            }
        }

        public BitmapImage ImageBmp
        {
            get
            {
                return _imagebmp;
            }
            set
            {
                _imagebmp = value;
                OnPropertyChanged("Chemin");
                OnPropertyChanged("ImageBmp");
                OnPropertyChanged("BmpSource");
            }
        }

        public CornerRadius MyRadius
        {
            get
            {
                return _MyRadius;
            }
            set
            {
                _MyRadius = value;
                OnPropertyChanged("MyRadius");
            }
        }

        public bool Solved
        {
            get { return _solved; }
            set
            {
                _solved = value;
                Fade = value;
                OnPropertyChanged("BmpSource");
                OnPropertyChanged("Solved");
            }
        }

        #endregion Getter & Setter
    }
}