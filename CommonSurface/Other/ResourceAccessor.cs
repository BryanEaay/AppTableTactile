using System;
using System.Windows.Media.Imaging;

namespace CommonSurface.Other
{
    public static class ResourceAccessor
    {
        #region Public Methods

        /// <summary>
        /// Chargement d'une image présente dans les ressources
        /// </summary>
        /// <param name="path">Uri de l'image</param>
        /// <returns>BitmapImage de la ressource</returns>
        public static BitmapImage loadImage(string path, UriKind kind = UriKind.RelativeOrAbsolute)
        {
            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = new Uri(path, kind);
                image.EndInit();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Public Methods
    }
}