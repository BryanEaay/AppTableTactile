using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Forms;

namespace AppAdministrationWPF.View
{
    public partial class Export : Window
    {
        #region Private Fields

        private string chemin;
        private string librairie;

        #endregion Private Fields

        #region Public Constructors

        public Export(string chemin, string librairie)
        {
            InitializeComponent();

            this.chemin = chemin;
            this.librairie = librairie;
        }

        #endregion Public Constructors

        #region Private Methods

        //bouton de fermeture
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "Fichiers Zip (*.zip)|*.zip|Tous les fichiers (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            fileDialog.DefaultExt = "zip";
            fileDialog.AddExtension = true;

            fileDialog.ShowDialog();
            txtEnregistrer.Text = fileDialog.FileName;
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            // Affichage de la barre de chargement
            InformationPanel.Visibility = Visibility.Visible;

            // Désactivation des contrôles
            txtEnregistrer.IsReadOnly = true;

            btEnregistrer.IsEnabled = false;

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
            arguments.Add(txtEnregistrer.Text);
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

            // Compteur pour calculer le pourcentage de progression
            double cpt = 0;

            // Récupération de tous les fichiers dans le chemin choisi
            string[] files = Directory.GetFiles(arguments[0], "*.*", SearchOption.AllDirectories);

            // Maximum pour le calcul du pourcentage
            double maximum = files.Length + 1;

            // Creation du fichier Zip
            using (Stream zipStream = new FileStream(Path.GetFullPath(arguments[1]), FileMode.Create, FileAccess.Write))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                using (Stream fileStream = new FileStream(arguments[2], FileMode.Open, FileAccess.Read))
                {
                    string[] split = arguments[2].Split('/');
                    string relativePath = split[split.Length - 1];
                    ZipArchiveEntry archiveEntry = archive.CreateEntry(relativePath);

                    using (Stream fileStreamInZip = archiveEntry.Open())
                    {
                        fileStream.CopyTo(fileStreamInZip);
                        cpt++;
                        // Calcul du pourcentage
                        int pourcentage = (int)(cpt / maximum * 100.0);
                        // Envoi de la progression
                        (sender as BackgroundWorker).ReportProgress(pourcentage, "Création du zip\r\n");
                    }
                }

                using (StreamWriter writer = new StreamWriter("ancien_chemin.txt"))
                {
                    writer.WriteLine(librairie);
                }

                using (Stream fileStream = new FileStream("ancien_chemin.txt", FileMode.Open, FileAccess.Read))
                {
                    ZipArchiveEntry archiveEntry = archive.CreateEntry("ancien_chemin.txt");

                    using (Stream fileStreamInZip = archiveEntry.Open())
                    {
                        fileStream.CopyTo(fileStreamInZip);
                        cpt++;
                        // Calcul du pourcentage
                        int pourcentage = (int)(cpt / maximum * 100.0);
                        // Envoi de la progression
                        (sender as BackgroundWorker).ReportProgress(pourcentage, "Création du fichier 'ancien_chemin.txt'\r\n");
                    }
                }

                // Envoi de la progression
                (sender as BackgroundWorker).ReportProgress((int)(cpt / maximum * 100.0), "Ajouts des fichiers:\r\n");

                int file_counter = 0;
                // Pour chaque fichier
                foreach (var filePath in files)
                {
                    // On ne garde que le chemin relatif
                    var relativePath = filePath.Replace(arguments[0] + "\\", string.Empty);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // Création d'une nouvelle entrée dans le fichier Zip grâce au chemin relatif
                        ZipArchiveEntry archiveEntry = archive.CreateEntry(relativePath);
                        // Lecture du fichier et copie du fichier dans le fichier Zip
                        using (Stream fileStreamInZip = archiveEntry.Open())
                        {
                            // Copie
                            fileStream.CopyTo(fileStreamInZip);
                            // Compteur augmenter après le traitement du fichier
                            cpt++;
                            // Calcul du pourcentage
                            int pourcentage = (int)(cpt / maximum * 100.0);
                            // Envoi de la progression
                            file_counter++;
                            (sender as BackgroundWorker).ReportProgress(pourcentage, "[" + file_counter + "/" + files.Length + "] " + filePath + "\r\n");
                        }
                    }
                }
            }
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