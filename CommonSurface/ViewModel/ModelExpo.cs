using System.Collections.Generic;

namespace CommonSurface.ViewModel
{
    public class ModelExpo
    {
        #region Public Fields

        public string cover;
        public int id;
        public List<DiapoModel> ListeDiapo = new List<DiapoModel>();
        public string text;
        public string titre;

        #endregion Public Fields

        #region Public Constructors

        public ModelExpo()
        {
        }

        #endregion Public Constructors

        #region Private Destructors

        ~ModelExpo()
        {
            id = 0;
            cover = titre = text = null;
            ListeDiapo.Clear();
            ListeDiapo = null;
        }

        #endregion Private Destructors
    }
}