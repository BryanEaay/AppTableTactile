using System.Diagnostics;

namespace AppPalaisRois
{
    public class Chronometre : Stopwatch
    {
        #region Public Constructors

        public Chronometre()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public string GetFormatTime()
        {
            return FixZero(this.Elapsed.Minutes.ToString(), 2) + ":" + FixZero(this.Elapsed.Seconds.ToString(), 2);
        }

        public void Go()
        {
            this.Reset();
            this.Start();
        }

        #endregion Public Methods

        #region Private Methods

        private string FixZero(string str, int digits)
        {
            return "0000".Substring(1, digits - str.Length) + str;
        }

        #endregion Private Methods
    }
}