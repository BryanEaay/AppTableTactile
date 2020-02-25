using System;
using System.Configuration;
using System.IO;

namespace CommonSurface.Other
{
    public static class MediaManager
    {
        #region Public Fields

        public static String cheminMediaDefaut = "";
        public static String cheminMediasMemory = ConfigurationManager.AppSettings["cheminLibrairieMemory"];
        public static String cheminMediasPuzzle = ConfigurationManager.AppSettings["cheminLibrairiePuzzle"];
        public static String cheminMediasRegion = ConfigurationManager.AppSettings["cheminLibrairieRegion"];

        #endregion Public Fields

        #region Public Methods

        public static String AddMediaToMemory(int IdJeu, String cheminMediaOri, String fileName)
        {
            string destFolder = System.IO.Path.Combine(cheminMediasMemory, "jeu" + IdJeu.ToString());
            string destFile = System.IO.Path.Combine(destFolder, fileName + System.IO.Path.GetExtension(cheminMediaOri));
            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
            }
            if (!cheminMediaOri.Equals(destFile))
            {
                if (System.IO.File.Exists(destFile))
                {
                    RemoveMedia(destFile);
                }
                System.IO.File.Copy(cheminMediaOri, destFile, true);
            }
            return destFile;
        }

        public static String AddMediaToPuzzle(int IdJeu, String cheminMediaOri, String fileName)
        {
            String destFolder = System.IO.Path.Combine(cheminMediasPuzzle, "jeu" + IdJeu.ToString());
            string destFile = System.IO.Path.Combine(destFolder, fileName + System.IO.Path.GetExtension(cheminMediaOri));
            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
            }
            if (!cheminMediaOri.Equals(destFile))
            {
                if (System.IO.File.Exists(destFile))
                {
                    RemoveMedia(destFile);
                }
                System.IO.File.Copy(cheminMediaOri, destFile, true);
            }
            return destFile;
        }

        /// <summary>
        /// Add a media to the media folder of the region app
        /// </summary>
        /// <param name="originPath">the path of the distant media</param>
        /// <returns></returns>
        public static String AddMediaToRegion(String originPath)
        {
            Console.WriteLine("Add media " + originPath);
            if (originPath.StartsWith("http://"))
            {
                Console.WriteLine("WEB PATH!");
                return originPath;
            }
            string destFolder = cheminMediasRegion;
            string filename = Path.GetFileName(originPath);
            int start = 0;
            int stop = filename.LastIndexOf(".");
            int length = stop - start;

            filename = filename.Substring(start, length);
            string destFile = Path.Combine(destFolder, filename + Path.GetExtension(originPath));

            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
            }

            if (!originPath.Equals(destFile))
            {
                if (File.Exists(destFile))
                {
                    RemoveMedia(destFile);
                }
                File.Copy(originPath, destFile, true);
            }
            Console.WriteLine("moved to " + destFile);
            return destFile;
        }

        public static int RemoveMedia(string Path)
        {
            if (!Path.Equals(cheminMediaDefaut))
            {
                System.IO.File.Delete(Path);
            }
            return 0;
        }

        #endregion Public Methods
    }
}