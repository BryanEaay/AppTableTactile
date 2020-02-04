using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AppAdministrationWPF.View
{
    public partial class Import : Window
    {
        #region Private Fields

        private bool append;
        private bool changeID;
        private string chemin;
        private string librairie;

        #endregion Private Fields

        #region Public Constructors

        public Import(string chemin, string librairie, bool append = true, bool changeID = true)
        {
            InitializeComponent();

            this.chemin = chemin;
            this.librairie = librairie;
            this.append = append;
            this.changeID = changeID;
        }

        #endregion Public Constructors

        #region Private Methods

        //bouton de fermeture
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btImporter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Fichiers Zip (*.zip)|*.zip|Tous les fichiers (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            fileDialog.DefaultExt = "zip";
            fileDialog.AddExtension = true;

            fileDialog.ShowDialog();
            txtImporter.Text = fileDialog.FileName;
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            // Affichage de la barre de chargement
            InformationPanel.Visibility = Visibility.Visible;

            // Désactivation des contrôles
            txtImporter.IsReadOnly = true;

            btImporter.IsEnabled = false;

            btCancel.IsEnabled = false;
            btOK.IsEnabled = false;

            // Mise à 0 de la barre
            ProgressBar.Value = 0;

            // Création d'un thread en background
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_Export;
            worker.RunWorkerCompleted += worker_Completed;
            worker.ProgressChanged += worker_Update;

            // Arguments pour le travail du thread
            List<string> arguments = new List<string>();
            arguments.Add(librairie);
            arguments.Add(txtImporter.Text);
            arguments.Add(chemin);

            // Lancement du thread
            worker.RunWorkerAsync(arguments);
        }

        /// <summary>
        /// Descend tout en bas du textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void txtInformation_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            txtInformation.ScrollToEnd();
        }

        // Fin du thread
        private void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            // On ferme la fenêtre dès la fin du thread
            this.Close();
        }

        private void worker_Export(object sender, DoWorkEventArgs e)
        {
            // récupération des arguments
            List<string> arguments = (List<string>)e.Argument;

            double cpt = 0;

            using (Stream zipStream = new FileStream(Path.GetFullPath(arguments[1]), FileMode.Open, FileAccess.Read))
            {
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    double maximum = archive.Entries.Count;
                    (sender as BackgroundWorker).ReportProgress(0, "Importation des fichiers :\r\n");
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string[] split = entry.FullName.Split('\\');
                        string nouveau_chemin = arguments[0];
                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            nouveau_chemin = Path.Combine(nouveau_chemin, split[i]);
                            if (!Directory.Exists(nouveau_chemin))
                            {
                                Directory.CreateDirectory(nouveau_chemin);
                            }
                        }
                        try
                        {
                            using (Stream fileStream = new FileStream(Path.Combine(arguments[0], entry.FullName), FileMode.OpenOrCreate))
                            {
                                using (Stream stream = entry.Open())
                                {
                                    stream.CopyTo(fileStream);
                                    // Compteur augmenter après le traitement du fichier
                                    cpt++;
                                    // Calcul du pourcentage
                                    int pourcentage = (int)(cpt / maximum * 100.0);
                                    // Envoi de la progression
                                    (sender as BackgroundWorker).ReportProgress(pourcentage, "[" + cpt + "/" + (int)maximum + "] " + Path.Combine(arguments[0], entry.FullName) + "\r\n");
                                }
                            }
                        }
                        catch (IOException)
                        {
                            // Compteur augmenter après le traitement du fichier
                            cpt++;
                            // Calcul du pourcentage
                            int pourcentage = (int)(cpt / maximum * 100.0);
                            // Envoi de la progression
                            (sender as BackgroundWorker).ReportProgress(pourcentage, "[" + cpt + "/" + (int)maximum + "] Erreur de fichier : " + entry.FullName + "\r\n");
                        }
                    }
                }
            }

            string[] files = Directory.GetFiles(arguments[0]);

            XElement original = XElement.Load(arguments[2]);
            XElement import = null;

            string ancien_chemin = "";
            string fichier_ancien_chemin = "";
            string fichier_xml = "";

            foreach (string file in files)
            {
                if (file.EndsWith("xml"))
                {
                    fichier_xml = file;
                    import = XElement.Load(file);
                }
                else if (file.EndsWith("txt"))
                {
                    fichier_ancien_chemin = file;
                }
            }

            if (!append)
            {
                original.RemoveAll();
            }

            StreamReader reader = new StreamReader(fichier_ancien_chemin);
            ancien_chemin = reader.ReadLine();
            ancien_chemin = ancien_chemin.Replace(@"/", @"\");
            reader.Close();

            foreach (XNode node in import.Nodes())
            {
                original.Add(node);
            }

            if (changeID)
            {
                int id = 1;

                foreach (XNode nodeOriginal in original.Nodes())
                {
                    ((nodeOriginal as XElement).FirstNode as XElement).Value = id.ToString();
                    id++;
                }
            }

            original.Save(arguments[2]);

            string text = File.ReadAllText(arguments[2]);
            text = text.Replace(ancien_chemin, librairie.Replace(@"/", @"\"));
            File.WriteAllText(arguments[2], text);

            File.Delete(fichier_xml);
            File.Delete(fichier_ancien_chemin);

            (sender as BackgroundWorker).ReportProgress(100, "Importation terminée\r\n");
        }

        // Mise à jour de la barre de chargement
        private void worker_Update(object sender, ProgressChangedEventArgs e)
        {
            // On récupère le pourcentage envoyé et on l'affecte à la barre
            ProgressBar.Value = e.ProgressPercentage;
            txtInformation.Text += e.UserState as string;
        }

        #endregion Private Methods
    }
}