using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows;
using System.Xml;

namespace AppAdministrationWPF.View
{
    public partial class DiapoAdd : Window
    {
        #region Private Fields

        private MediaDialogModel _viewModel;
        private string chemin = ConfigurationManager.AppSettings["cheminExpoVirtuelles"];
        private int ExpoID;

        private AdminMediathequeView lastPage;

        private int MaxId;

        //qui permet de savoir si on modifi ou on ajoute, 1 modifier 0 ajouter
        private int modifAdd;

        #endregion Private Fields

        #region Public Constructors

        public DiapoAdd(int modifOrAdd, int Expoid, MediaDialogModel viewModel, AdminMediathequeView page)
        {
            InitializeComponent();
            _viewModel = viewModel;
            //si c'est une modification et pas un ajout recupération des données
            if (modifOrAdd != 0)
            {
                //récupération du document xml
                XmlDocument doc = new XmlDocument();
                doc.Load(chemin);
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + Expoid + "']/contenu/document[ID='" + modifOrAdd + "']");
                foreach (XmlNode xn in nodeList)
                {
                    //récuperation de toute les anciennes données
                    txtName.Text = xn["titre"].InnerText;
                    txtDescription.Text = xn["text"].InnerText;
                    ViewModel.Path = xn["element"].InnerText;
                    txtImg1.Text = xn["image1"].InnerText;
                    txtImg2.Text = xn["image2"].InnerText;
                    //teste pour selectionné le bon type de base
                    if (xn["type"].InnerText != "video")
                    {
                        if (xn["type"].InnerText == "photo")
                        {
                            Video.IsSelected = false;
                            Photo.IsSelected = true;
                        }
                        else
                        {
                            Video.IsSelected = false;
                            Panorama.IsSelected = true;
                        }
                    }
                }
            }
            modifAdd = modifOrAdd;
            ExpoID = Expoid;
            lastPage = page;
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

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btImg1_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre de recherche de fichier
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Photos (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 4;
            fileDialog.Multiselect = false;
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                txtImg1.Text = fileDialog.FileName;
            }
        }

        private void btImg2_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre de recherche de fichier
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Photos (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 4;
            fileDialog.Multiselect = false;
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                txtImg2.Text = fileDialog.FileName;
            }
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            //récupération du document xml
            XmlDocument doc = new XmlDocument();
            doc.Load(chemin);
            //si c'est un ajout
            if (modifAdd == 0)
            {
                //recupération de la liste de la bonne expo ou ajouter la diapo
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + ExpoID + "']");
                foreach (XmlNode xn in nodeList)
                {
                    MaxId = Int32.Parse(xn["numberDiapo"].InnerText);
                    //ajouté 1 au nombre d'expo
                    xn["numberDiapo"].InnerText = "" + (MaxId + 1);
                }

                //récuperation du noeud du contenu
                nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + ExpoID + "']/contenu");
                foreach (XmlNode xn in nodeList)
                {
                    //ajouter une nouvelle diapo
                    XmlElement newNode = doc.CreateElement("document");

                    //création de tous les noeuds
                    XmlNode IdExpoNode = doc.CreateElement("ID");

                    IdExpoNode.InnerText = "" + (MaxId + 1);

                    XmlNode coverNode = doc.CreateElement("element");

                    coverNode.InnerText = txtPath.Text;

                    XmlNode titreNode = doc.CreateElement("titre");

                    titreNode.InnerText = txtName.Text;

                    XmlNode textNode = doc.CreateElement("text");

                    textNode.InnerText = txtDescription.Text;

                    XmlNode Img1Node = doc.CreateElement("image1");

                    Img1Node.InnerText = txtImg1.Text;

                    XmlNode Img2Node = doc.CreateElement("image2");

                    Img2Node.InnerText = txtImg2.Text;

                    XmlNode typeNode = doc.CreateElement("type");

                    //teste de ce qui est selectionné comme type
                    if (Video.IsSelected)
                    {
                        typeNode.InnerText = "video";
                    }
                    else if (Photo.IsSelected)
                    {
                        typeNode.InnerText = "photo";
                    }
                    else if (Panorama.IsSelected)
                    {
                        typeNode.InnerText = "panorama";
                    }

                    //emboitement des noeuds
                    newNode.AppendChild(typeNode);

                    newNode.AppendChild(IdExpoNode);

                    newNode.AppendChild(titreNode);

                    newNode.AppendChild(textNode);

                    newNode.AppendChild(Img1Node);

                    newNode.AppendChild(Img2Node);

                    newNode.AppendChild(coverNode);

                    xn.AppendChild(newNode);
                }
                modifAdd = MaxId + 1;
            }//si c'est une modification
            else
            {
                //modifier une expo
                //recupération du noeud de la diapo a modifier
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + ExpoID + "']/contenu/document[ID='" + modifAdd + "']");
                foreach (XmlNode xn in nodeList)
                {
                    //modification de tous les element de la diapo
                    xn["element"].InnerText = txtPath.Text;
                    xn["titre"].InnerText = txtName.Text;
                    xn["text"].InnerText = txtDescription.Text;
                    xn["image1"].InnerText = txtImg1.Text;
                    xn["image2"].InnerText = txtImg2.Text;
                    if (Video.IsSelected)
                    {
                        xn["type"].InnerText = "video";
                    }
                    else if (Photo.IsSelected)
                    {
                        xn["type"].InnerText = "photo";
                    }
                    else if (Panorama.IsSelected)
                    {
                        xn["type"].InnerText = "panorama";
                    }
                }
            }
            //et lance la sauvegarde et lecture de la liste
            doc.Save(chemin);

            //recharger la page avec les modification
            lastPage.rest();
            lastPage.launchExpo2(ExpoID);
            Close();
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre de recherche de fichier
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Videos(*.mov, *.wmv, *.mp4)|*.mov;*.wmv;*.mp4|Photos (*.jpg, *.png)|*.jpg;*.png|Photos & Videos|*.mov;*.wmv;*.jpg;*.png; *.mp4; *.*";
            fileDialog.FilterIndex = 4;
            fileDialog.Multiselect = false;
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                ViewModel.Path = fileDialog.FileName;
                String name = fileDialog.FileName;
                try
                {
                    int start = name.LastIndexOf(@"\") + 1;
                    int stop = name.LastIndexOf(".");
                    int length = stop - start;

                    name = name.Substring(start, length);
                }
                catch (Exception) { }
            }
        }

        #endregion Private Methods
    }
}