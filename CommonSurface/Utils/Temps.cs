namespace CommonSurface.Utils
{
    public class Temps
    {
        #region declaration des compteurs

        private static int tpsGoogleEarth;
        private static int tpsMap;
        private static int tpsMediatheque;
        private static int tpsMemory;
        private static int tpsPuzzle;
        private static int tpsVisio;

        #endregion declaration des compteurs

        #region compteurs debut

        private static int debutMemory, debutPuzzle, debutMediatheque, debutVisio, debutGoogleEarth, debutMap;

        #endregion compteurs debut

        #region GettersAndSetters

        public int DebutGoogleEarth
        {
            get
            {
                return debutGoogleEarth;
            }
            set
            {
                debutGoogleEarth = value;
            }
        }

        public int DebutMap
        {
            get
            {
                return debutMap;
            }
            set
            {
                debutMap = value;
            }
        }

        public int DebutMediatheque
        {
            get
            {
                return debutMediatheque;
            }
            set
            {
                debutMediatheque = value;
            }
        }

        public int DebutMemory
        {
            get
            {
                return debutMemory;
            }
            set
            {
                debutMemory = value;
            }
        }

        public int DebutPuzzle
        {
            get
            {
                return debutPuzzle;
            }
            set
            {
                debutPuzzle = value;
            }
        }

        public int DebutVisio
        {
            get
            {
                return debutVisio;
            }
            set
            {
                debutVisio = value;
            }
        }

        public int TpsGoogleEarth
        {
            get
            {
                return tpsGoogleEarth;
            }
            set
            {
                tpsGoogleEarth = value;
            }
        }

        public int TpsMap
        {
            get
            {
                return tpsMap;
            }
            set
            {
                tpsMap = value;
            }
        }

        public int TpsMediatheque
        {
            get
            {
                return tpsMediatheque;
            }
            set
            {
                tpsMediatheque = value;
            }
        }

        public int TpsMemory
        {
            get
            {
                return tpsMemory;
            }
            set
            {
                tpsMemory = value;
            }
        }

        public int TpsPuzzle
        {
            get
            {
                return tpsPuzzle;
            }
            set
            {
                tpsPuzzle = value;
            }
        }

        public int TpsVisio
        {
            get
            {
                return tpsVisio;
            }
            set
            {
                tpsVisio = value;
            }
        }

        #endregion GettersAndSetters

        #region tempsPasse

        public static int tempsMediatheque()
        {
            return tpsMediatheque;
        }

        #endregion tempsPasse
    }
}