using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows;
using System.Xml;

namespace AppAdministrationWPF.View
{
    public partial class ExpoAdd : Window
    {
        #region Private Fields

        private MediaDialogModel _viewModel;
        private string chemin = ConfigurationManager.AppSettings["cheminExpoVirtuelles"];
        private AdminMediathequeView lastPage;

        private int maxId;

        //qui permet de savoir si on modifi ou on ajoute, 1 modifier 0 ajouter
        private int modifAdd;

        #endregion Private Fields

        #region Public Constructors

        public ExpoAdd(int modifOrAdd, int idMax, MediaDialogModel viewModel, AdminMediathequeView page)
        {
            InitializeComponent();
            ViewModel = viewModel;

            //tester si c'est une modification ou un ajout pour recupérer ou non les anciennes données
            if (modifOrAdd != 0)
            {
                //récupération du document xml
                XmlDocument doc = new XmlDocument();
                doc.Load(chemin);
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + modifOrAdd + "']");
                foreach (XmlNode xn in nodeList)
                {
                    ViewModel.Path = xn["cover"].InnerText;
                    txtName.Text = xn["titre"].InnerText;
                    Description.Text = xn["text"].InnerText;
                }
            }
            modifAdd = modifOrAdd;
            lastPage = page;
            maxId = idMax;
            DataContext = ViewModel;
        }

        #endregion Public Constructors

        #region Public Properties

        public Media Selected
        {
            get { return _viewModel.Selected; }
            set { _viewModel.Selected = value; }
        }

        public MediaDialogModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        #endregion Public Properties

        #region Private Methods

        //bouton de fermeture
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            //récupération du document xml
            XmlDocument doc = new XmlDocument();
            doc.Load(chemin);
            if (modifAdd == 0)
            {
                //ajouter une expo
                XmlNode node = doc.DocumentElement;

                //ajout de tous les noeuds
                XmlElement newNode = doc.CreateElement("expo");

                XmlNode IdExpoNode = doc.CreateElement("IdExpo");

                IdExpoNode.InnerText = "" + (maxId + 1);

                XmlNode coverNode = doc.CreateElement("cover");

                coverNode.InnerText = txtPath.Text;

                XmlNode titreNode = doc.CreateElement("titre");

                titreNode.InnerText = txtName.Text;

                XmlNode textNode = doc.CreateElement("text");

                textNode.InnerText = Description.Text;

                XmlNode numberDiapo = doc.CreateElement("numberDiapo");

                numberDiapo.InnerText = "0";

                XmlNode contenuNode = doc.CreateElement("contenu");

                //emboitement de tous les noeuds
                newNode.AppendChild(IdExpoNode);

                newNode.AppendChild(coverNode);

                newNode.AppendChild(titreNode);

                newNode.AppendChild(textNode);

                newNode.AppendChild(numberDiapo);

                newNode.AppendChild(contenuNode);

                node.AppendChild(newNode);

                modifAdd = maxId + 1;
            }
            else
            {
                //modifier une expo
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + modifAdd + "']");
                foreach (XmlNode xn in nodeList)
                {
                    //modification des données
                    xn["cover"].InnerText = txtPath.Text;
                    xn["titre"].InnerText = txtName.Text;
                    xn["text"].InnerText = Description.Text;
                }
            }
            //et lance la sauvegarde et lecture de la liste
            doc.Save(chemin);

            //recharger la page avec les modification
            lastPage.rest();
            lastPage.launchExpo2(modifAdd);

            Close();
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre de recherche de fichier sur des fichiers video image ou panorama
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Videos(*.mov, *.wmv, *.mp4)|*.mov;*.wmv;*.mp4|Photos (*.jpg, *.png)|*.jpg;*.png|Photos & Videos|*.mov;*.wmv;*.jpg;*.png; *.mp4; *.*";
            fileDialog.FilterIndex = 4;
            fileDialog.Multiselect = false;
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                //ajouter les données a la fenetre
                ViewModel.Path = fileDialog.FileName;
                String name = fileDialog.FileName;
            }
        }

        #endregion Private Methods
    }
}