using AppAdministrationWPF.ViewModel;
using CommonSurface.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml;

namespace AppAdministrationWPF.View
{
    public partial class AdminMediathequeView : UserControl
    {
        #region Private Fields

        private string chemin = ConfigurationManager.AppSettings["cheminExpoVirtuelles"];
        private string cheminLibrairie = ConfigurationManager.AppSettings["cheminLibrairieExpoVirtuelles"];
        private DiapoModel diapo = new DiapoModel();
        private int diapoID = 0;
        private ModelExpo ExpoElement = new ModelExpo();
        private int expoID = 0;
        private List<ModelExpo> listeExpo = new List<ModelExpo>();

        #endregion Private Fields

        #region Public Constructors

        public AdminMediathequeView()
        {
            InitializeComponent();

            #region Récupération des flèches

            // Haut
            flecheHaut.Source = Imaging.CreateBitmapSourceFromHBitmap(
                               CommonSurface.Properties.Resources.FlecheHaut.GetHbitmap(),
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());
            // Bas
            flecheBas.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.FlecheBas.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

            #endregion Récupération des flèches

            Load_List();
        }

        #endregion Public Constructors

        #region Public Methods

        //fonction de lecture et de création d'une diapo
        public void FonctionDiapo(string value, string nameElement)
        {
            //le switch envoie la valeur dans la bonne variable selon le nom de l'element
            switch (nameElement)
            {
                case "type":
                    diapo.type = value;
                    break;

                case "ID":
                    diapo.id = Int32.Parse(value);
                    break;

                case "element":
                    diapo.element = value;
                    break;

                case "titre":
                    diapo.titre = value;
                    break;

                case "text":
                    diapo.text = value;
                    break;

                case "source":
                    diapo.source = value;
                    break;
            }
        }

        //fonction de lecture et de création d'une expo
        public void FonctionExpo(string value, string nameElement)
        {
            //le switch envoie la valeur dans la bonne variable selon le nom de l'element
            switch (nameElement)
            {
                case "IdExpo":
                    ExpoElement.id = Int32.Parse(value);
                    break;

                case "cover":
                    ExpoElement.cover = value;
                    break;

                case "titre":
                    ExpoElement.titre = value;
                    break;

                case "text":
                    ExpoElement.text = value;
                    break;
            }
        }

        //lancement du media de la diapo selectionné
        public void launchDiapo(object sender, EventArgs e, int id)
        {
            //déclaration du nom et recupération de l'id
            string stack_name = ((StackPanel)sender).Name;
            stack_name = stack_name.Replace("stack", "");
            int id_stack = Convert.ToInt32(stack_name);
            diapoID = id_stack;

            //récupération de l'ancienne expo
            expoID = id;

            MediaElement media = new MediaElement();

            //source du média
            media.Source = new Uri(listeExpo[id - 1].ListeDiapo[id_stack - 1].element);

            //ajouter le média au grid
            dock_main_photo.Children.Clear();
            dock_main_photo.Children.Add(media);
        }

        //lancement du media de l'expo selectionné
        public void launchExpo(object Sender, EventArgs e)
        {
            //déclaration du nom et recupération de l'id
            string stack_name = ((StackPanel)Sender).Name;
            stack_name = stack_name.Replace("stack", "");
            int id_stack = Convert.ToInt32(stack_name);
            MediaElement media = new MediaElement();
            diapoID = 0;
            expoID = id_stack;

            listDiapo.Items.Clear();
            dock_main_photo.Children.Clear();

            //affichage de la liste des diapos dans la deuxieme liste
            foreach (DiapoModel j in listeExpo[id_stack - 1].ListeDiapo)
            {
                //element a ajouté a la liste qui contient tous les elements
                StackPanel stack = new StackPanel();

                //texte boxe pour la description
                TextBlock textblocktitle = new TextBlock();

                textblocktitle.Text = j.titre;
                textblocktitle.Width = 350;
                stack.Name = "stack" + j.id;
                stack.Children.Add(textblocktitle);

                //détection de la selection
                stack.MouseUp += (sender, f) => launchDiapo(sender, f, id_stack);
                stack.TouchDown += (sender, f) => launchDiapo(sender, f, id_stack);

                listDiapo.Items.Add(stack);
            }

            //affichage du média selectionné
            try
            {
                media.Source = new Uri(listeExpo[id_stack - 1].cover);
            }
            catch
            {
                media.Source = null;
            }

            //ajouter le média au grid
            dock_main_photo.Children.Add(media);
        }

        //lancement du media de l'expo quand on modifie supprime ou ajoute sans selection dans la liste (ancienne selection ou selection moins 1 quand c'est supprimé)
        public void launchExpo2(int id)
        {
            int id_stack = id;
            MediaElement media = new MediaElement();
            diapoID = 0;

            listDiapo.Items.Clear();
            dock_main_photo.Children.Clear();

            if (id_stack >= listeExpo.Count || id_stack <= 0)
            {
                id_stack = 1;
            }

            if (listeExpo.Count <= 0)
            {
                return;
            }

            //affichage de la liste des diapos dans la deuxieme liste
            foreach (DiapoModel j in listeExpo[id_stack - 1].ListeDiapo)
            {
                //element a ajouté a la liste qui contient tous les elements
                StackPanel stack = new StackPanel();

                //texte boxe pour la description
                TextBlock textblocktitle = new TextBlock();

                textblocktitle.Text = j.titre;
                textblocktitle.Width = 350;
                stack.Name = "stack" + j.id;
                stack.Children.Add(textblocktitle);

                //détection de la selection
                stack.MouseUp += (sender, f) => launchDiapo(sender, f, id_stack);
                stack.TouchDown += (sender, f) => launchDiapo(sender, f, id_stack);

                listDiapo.Items.Add(stack);
            }
            //affichage du média selectionné
            media.Source = new Uri(listeExpo[id_stack - 1].cover);
            //ajouter le média au grid
            dock_main_photo.Children.Add(media);
        }

        //apres une modification ou un ajout
        public void rest()
        {
            //recharger la page avec les modification
            listExpo.Items.Clear();
            listDiapo.Items.Clear();
            listeExpo = new List<ModelExpo>();
            Load_List();
        }

        #endregion Public Methods

        #region Private Methods

        //Action du bouton Ajouter une Expo
        private void btAddExpo_Click(object sender, RoutedEventArgs e)
        {
            //ouverture de la fenetre expoadd
            ExpoAdd fenetre = new ExpoAdd(0, listeExpo.Count, new MediaDialogModel(null), this);
            fenetre.Show();
        }

        //Action du bouton Ajouter une diapo
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            //si c'est une expo selectionné
            if (expoID != 0)
            {
                //ouverture de la fenetre diapoadd
                DiapoAdd fenetre = new DiapoAdd(0, expoID, new MediaDialogModel(null), this);
                fenetre.Show();
            }
        }

        //Action du bouton modifier
        private void ButtonDown(object sender, RoutedEventArgs e)
        {
            //récupération du document xml
            XmlDocument doc = new XmlDocument();
            doc.Load(chemin);

            int tailleListe = 0;
            //action pour supprimer une expo
            XmlNodeList nodetaille = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + expoID + "']");
            foreach (XmlNode xn in nodetaille)
            {
                tailleListe = Convert.ToInt32(xn["numberDiapo"].InnerText);
            }

            if (diapoID == 0)
            {
                if (expoID != 0 && expoID != listeExpo.Count)
                {
                    //modifie le document xml
                    XmlNodeList nodeList = doc.SelectNodes("/listExpo/expo");

                    foreach (XmlNode xn in nodeList)
                    {
                        if (expoID + 1 == Convert.ToInt32(xn["IdExpo"].InnerText))
                        {
                            foreach (XmlNode xn2 in nodeList)
                            {
                                if (expoID == Convert.ToInt32(xn2["IdExpo"].InnerText))
                                {
                                    //changement de l'id pour changer la position
                                    xn2["IdExpo"].InnerText = "" + (expoID + 1);
                                    break;
                                }
                            }
                            //changement de l'id pour changer la position
                            xn["IdExpo"].InnerText = "" + (expoID);
                            break;
                        }
                    }

                    //et lance la sauvegarde et lecture de la liste
                    doc.Save(chemin);

                    //recharger la page avec les modification
                    listExpo.Items.Clear();
                    listDiapo.Items.Clear();
                    listeExpo = new List<ModelExpo>();
                    Load_List();
                    launchExpo2(expoID);
                }
            }
            else
            {
                if (diapoID != tailleListe)
                {
                    //modifie le document xml
                    XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + expoID + "']/contenu/document");
                    foreach (XmlNode xn in nodeList)
                    {
                        if (diapoID + 1 == Convert.ToInt32(xn["ID"].InnerText))
                        {
                            foreach (XmlNode xn2 in nodeList)
                            {
                                if (diapoID == Convert.ToInt32(xn2["ID"].InnerText))
                                {
                                    //changement de l'id pour changer la position
                                    xn2["ID"].InnerText = "" + (diapoID + 1);
                                    break;
                                }
                            }
                            //changement de l'id pour changer la position
                            xn["ID"].InnerText = "" + (diapoID);
                            break;
                        }
                    }
                    //et lance la sauvegarde et lecture de la liste
                    doc.Save(chemin);

                    //recharger la page avec les modification
                    listExpo.Items.Clear();
                    listDiapo.Items.Clear();
                    listeExpo = new List<ModelExpo>();
                    Load_List();
                    launchExpo2(expoID);
                }
            }
        }

        //Action du bouton Modifier
        private void ButtonExporter_Click(object sender, RoutedEventArgs e)
        {
            Export fenetre = new Export(chemin, cheminLibrairie);
            fenetre.Show();
        }

        //Action du bouton Modifier
        private void ButtonImporter_Click(object sender, RoutedEventArgs e)
        {
            Import fenetre = new Import(chemin, cheminLibrairie);
            fenetre.Closing += update_OnClose;
            fenetre.Show();
        }

        //Action du bouton modifier
        private void ButtonModifier_Click(object sender, RoutedEventArgs e)
        {
            //si une expo est selectionné
            if (diapoID == 0 && expoID != 0)
            {
                ExpoAdd fenetre = new ExpoAdd(expoID, listeExpo.Count, new MediaDialogModel(null), this);
                fenetre.Show();
            }//si une diapo est selectionné
            else if (expoID != 0)
            {
                DiapoAdd fenetre = new DiapoAdd(diapoID, expoID, new MediaDialogModel(null), this);
                fenetre.Show();
            }
        }

        //Action du bouton Supprimer
        private void ButtonSupprimer_Click(object sender, RoutedEventArgs e)
        {
            //récupération du document xml
            XmlDocument doc = new XmlDocument();
            doc.Load(chemin);

            //tester si ce n'est pas une dipo de selectionné et qu'il y a une expo selectionné et que la taille de la liste est supérieur a 1
            if (diapoID == 0 && expoID != 0 && listeExpo.Count() > 1)
            {
                int tailleListe = listeExpo.Count();
                //action pour supprimer une expo
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + expoID + "']");
                foreach (XmlNode xn in nodeList)
                {
                    xn.ParentNode.RemoveChild(xn);
                }
                //action pour reffaire les idées suppérieur
                for (int i = 1; i <= tailleListe - expoID; i++)
                {
                    XmlNodeList node = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + (expoID + i) + "']");
                    foreach (XmlNode xn in node)
                    {
                        xn["IdExpo"].InnerText = "" + ((expoID + i) - 1);
                    }
                }
                //expo moins 1 pour afficher une expo qui existe
                expoID--;
            }//si une diapo est selectionné
            else if (expoID != 0)
            {
                int tailleListe = 0;
                //pour modifier la taille de la liste de diapo dans l'expo
                XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + expoID + "']");
                foreach (XmlNode xn in nodeList)
                {
                    tailleListe = Convert.ToInt32(xn["numberDiapo"].InnerText);
                    xn["numberDiapo"].InnerText = "" + (Convert.ToInt32(xn["numberDiapo"].InnerText) - 1);
                }
                //action pour supprimer la diapo
                nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + expoID + "']/contenu/document[ID='" + diapoID + "']");
                foreach (XmlNode xn in nodeList)
                {
                    xn.ParentNode.RemoveChild(xn);
                }
                //modification de toute les idées des diapo superieur a moins 1
                for (int i = 1; i <= tailleListe - diapoID; i++)
                {
                    XmlNodeList node = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + (expoID) + "']/contenu/document[ID='" + (diapoID + i) + "']");
                    foreach (XmlNode xn in node)
                    {
                        xn["ID"].InnerText = "" + ((diapoID + i) - 1);
                    }
                }
            }

            //et lance la sauvegarde et lecture de la liste
            doc.Save(chemin);

            //recharger la page avec les modification
            listExpo.Items.Clear();
            listDiapo.Items.Clear();
            listeExpo = new List<ModelExpo>();
            Load_List();
            launchExpo2(expoID);
        }

        //Action du bouton modifier
        private void ButtonUp(object sender, RoutedEventArgs e)
        {
            //récupération du document xml
            XmlDocument doc = new XmlDocument();
            doc.Load(chemin);

            if (diapoID == 0)
            {
                if (expoID != 0 && expoID != 1)
                {
                    //modifie le document xml
                    //récupération de la liste d'expo
                    XmlNodeList nodeList = doc.SelectNodes("/listExpo/expo");

                    //recherche de l'expo a descendre
                    foreach (XmlNode xn in nodeList)
                    {
                        if (expoID == Convert.ToInt32(xn["IdExpo"].InnerText))
                        {
                            //recherche de l'expo a monter
                            foreach (XmlNode xn2 in nodeList)
                            {
                                if (expoID - 1 == Convert.ToInt32(xn2["IdExpo"].InnerText))
                                {
                                    //changement de l'id pour changer la position
                                    xn2["IdExpo"].InnerText = "" + (expoID);
                                    //stoper la boucle foreach
                                    break;
                                }
                            }
                            //changement de l'id pour changer la position
                            xn["IdExpo"].InnerText = "" + (expoID - 1);
                            break;
                        }
                    }

                    //et lance la sauvegarde et lecture de la liste
                    doc.Save(chemin);

                    //recharger la page avec les modification
                    listExpo.Items.Clear();
                    listDiapo.Items.Clear();
                    listeExpo = new List<ModelExpo>();
                    Load_List();
                    launchExpo2(expoID);
                }
            }
            else
            {
                if (diapoID != 1)
                {
                    //modifie le document xml
                    //récupération de la liste d'expo
                    XmlNodeList nodeList = doc.SelectNodes("descendant::listExpo/expo[IdExpo='" + expoID + "']/contenu/document");

                    foreach (XmlNode xn in nodeList)
                    {
                        if (diapoID == Convert.ToInt32(xn["ID"].InnerText))
                        {
                            foreach (XmlNode xn2 in nodeList)
                            {
                                if (diapoID - 1 == Convert.ToInt32(xn2["ID"].InnerText))
                                {
                                    //changement de l'id pour changer la position
                                    xn2["ID"].InnerText = "" + (diapoID);
                                    break;
                                }
                            }
                            //changement de l'id pour changer la position
                            xn["ID"].InnerText = "" + (diapoID - 1);
                            break;
                        }
                    }
                    //et lance la sauvegarde et lecture de la liste
                    doc.Save(chemin);

                    //recharger la page avec les modification
                    listExpo.Items.Clear();
                    listDiapo.Items.Clear();
                    listeExpo = new List<ModelExpo>();
                    Load_List();
                    launchExpo2(expoID);
                }
            }
        }

        //Récupérer la liste
        private void Load_List()
        {
            //pour savoir le nom des elements traité
            string nameElement = "";

            bool isDocument = false;

            //chemin vers le fichier xml. peut être mis en relatif
            using (XmlTextReader xmlString = new XmlTextReader(chemin))
            {
                //boucle de lecture du document xml des expos
                while (xmlString.Read())
                {
                    switch (xmlString.NodeType)
                    {
                        case XmlNodeType.Element: // le node est un element
                            nameElement = xmlString.Name; // recuperation de l'element
                                                          //si l'element est une expo initialisé l'expo
                            if (nameElement == "expo") ExpoElement = new ModelExpo();
                            //si l'element est une diapo changer la fariabla isDocument a true pour l'indiquer
                            if (nameElement == "document")
                            {
                                isDocument = true;
                                diapo = new DiapoModel();
                            }
                            break;

                        case XmlNodeType.Text: //le node est la valeur de l'element
                                               //si on est dans une diapo
                            if (isDocument)
                            {
                                //envoie de la fonction de creation d'une diapo
                                FonctionDiapo(xmlString.Value, nameElement);
                            }
                            else //sinon on est dans une expo donc
                            {
                                //envoie de la création d'une expo
                                FonctionExpo(xmlString.Value, nameElement);
                            }
                            break;

                        case XmlNodeType.EndElement: //le node est la fin de l'element
                                                     //si c'est la fin d'une expo
                            if (xmlString.Name == "expo")
                            {
                                //ajouter l'expo a la liste des expos
                                listeExpo.Add(ExpoElement);
                            }
                            //si c'est la fin d'un document
                            if (xmlString.Name == "document")
                            {
                                //ajouté la diapo a la liste de l'expo actuelle
                                ExpoElement.ListeDiapo.Add(diapo);
                                //changemlent de la variable pour indiquer que la diapo est fini
                                isDocument = false;
                            }

                            break;
                    }
                }
                xmlString.Close();
            }

            //remise dans l'ordre des expos
            List<ModelExpo> listeOrdre = new List<ModelExpo>();
            int numExpo = 1;
            foreach (ModelExpo i in listeExpo)
            {
                //recherche de l'id ordonné
                foreach (ModelExpo h in listeExpo)
                {
                    //si il est correcte ajout a la liste ordonné
                    if (numExpo == h.id)
                    {
                        //element a ajouté a la liste qui contient tous les elements
                        StackPanel stack = new StackPanel();

                        //texte boxe pour la description
                        TextBlock textblocktitle = new TextBlock();

                        //récupération du titre
                        textblocktitle.Text = h.titre;
                        textblocktitle.Width = 350;
                        stack.Name = "stack" + h.id;

                        //ajouter l'element au contenant
                        stack.Children.Add(textblocktitle);

                        //initialisation des variables pour ordonner les diapos

                        List<DiapoModel> listeOrdreDiapo = new List<DiapoModel>();
                        int numdiapo = 1;
                        foreach (DiapoModel n in h.ListeDiapo)
                        {
                            //recherche des dipo dans l'ordre
                            foreach (DiapoModel m in h.ListeDiapo)
                            {
                                if (numdiapo == m.id)
                                {
                                    //ajout de la diapo a la liste ordonné
                                    listeOrdreDiapo.Add(m);
                                    break;
                                }
                            }
                            numdiapo++;
                        }

                        //recupération de la liste de diapo
                        h.ListeDiapo = listeOrdreDiapo;

                        //détection de la selection
                        stack.MouseUp += (sender, e) => launchExpo(sender, e);
                        stack.TouchDown += (sender, e) => launchExpo(sender, e);

                        //ajout de l'expo a la liste ordonné
                        listeOrdre.Add(h);

                        //ajout a la liste l'element d'expo
                        listExpo.Items.Add(stack);
                        break;
                    }
                }
                //incrémentation du repère
                numExpo++;
            }
            //redonner une liste ordonné
            listeExpo = listeOrdre;
        }

        private void update_OnClose(object sender, CancelEventArgs e)
        {
            //recharger la page avec les modification
            listExpo.Items.Clear();
            listeExpo = new List<ModelExpo>();
            Load_List();
        }

        #endregion Private Methods
    }
}