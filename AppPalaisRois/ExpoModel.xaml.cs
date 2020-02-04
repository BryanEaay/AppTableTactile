using CommonSurface.Other;
using CommonSurface.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

namespace AppPalaisRois
{
    /// <summary>
    /// Logique d'interaction pour ExpoModel.xaml
    /// </summary>
    public partial class ExpoModel : Window
    {
        #region Public Fields

        public int thumbnail_height = 140;

        //declaration de la taille du media dans la liste
        public int thumbnail_width = 140;

        #endregion Public Fields

        #region Private Fields

        private string chemin = ConfigurationManager.AppSettings["cheminExpoVirtuelles"];

        private string cheminDefaut360 = ConfigurationManager.AppSettings["cheminDefautMiniature360"];

        private DiapoModel diapo = new DiapoModel();

        private ModelExpo ExpoElement = new ModelExpo();

        //declaration de id de l'expo demandé
        private int id;

        private List<ModelExpo> listeExpo = new List<ModelExpo>();

        //declaration du media et ajout de la source
        private MediaElement media = new MediaElement();

        private bool play = true;
        private DispatcherTimer timerVideoTime;
        private TimeSpan TotalTime;

        #endregion Private Fields

        #region Public Constructors

        public ExpoModel(object sender, EventArgs e)
        {
            InitializeComponent();

            // Récupération du retour
            returnExpo.Source = ResourceAccessor.loadImage("/CommonSurface;component/Resources/return.png");

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
            int num = 1;
            foreach (ModelExpo i in listeExpo)
            {
                //recherche de l'id ordonné
                foreach (ModelExpo h in listeExpo)
                {
                    //si il est correcte ajout a la liste ordonné
                    if (num == h.id)
                    {
                        //initialisation des variables pour ordonner les diapos
                        int numdiapo = 1;
                        List<DiapoModel> listeOrdreDiapo = new List<DiapoModel>();

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

                        //ajout de l'expo a la liste ordonné
                        listeOrdre.Add(h);
                        break;
                    }
                }
                //incrémentation du repère
                num++;
            }
            //redonner une liste ordonné
            listeExpo = listeOrdre;

            //lancement de l'expo
            StartExpo(sender, e);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        #endregion Public Constructors

        #region Public Methods

        //lecture du texte de description
        public static string ReadTxtFile(string path)
        {
            System.IO.StreamReader myFile = new System.IO.StreamReader(path, System.Text.Encoding.Default);
            string myString = myFile.ReadToEnd();

            myFile.Close();

            return myString;
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

        //avant l'ouverture de la video récuperation du temps maximum impossible autrement
        public void Me_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                //MediaElement media = (MediaElement)sender;
                TotalTime = media.NaturalDuration.TimeSpan;

                slider.Maximum = slider.Width;
                slider.TickFrequency = slider.Maximum / TotalTime.TotalSeconds;

                // Create a timer that will update the counters and the time slider
                timerVideoTime = new DispatcherTimer();
                timerVideoTime.Interval = TimeSpan.FromSeconds(1);
                timerVideoTime.Tick += new EventHandler(Timer_Tick);
                timerVideoTime.Start();
            }
            catch
            {
                //netoyer le contenue
                grid.Children.Clear();
                dock_main_photo.Children.Clear();

                TextBlock textblockSource = new TextBlock();

                //texte erreur de type de fichier
                textblockSource.Visibility = Visibility.Visible;
                textblockSource.FontSize = 30;
                textblockSource.Foreground = new SolidColorBrush(Colors.Red);
                textblockSource.Text = "Le type de fichier n'est pas lisible";
                textblockSource.HorizontalAlignment = HorizontalAlignment.Center;
                textblockSource.VerticalAlignment = VerticalAlignment.Center;
                textblockSource.TextAlignment = TextAlignment.Justify;
                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;

                //ajout de l'element créé pour le panorama
                dock_main_photo.Children.Add(textblockSource);
            }
        }

        //lancement de l'expo
        public void StartExpo(object sender, EventArgs e)
        {
            //déclaration du nom et recupération de l'id
            string stack_name = ((StackPanel)sender).Name;
            stack_name = stack_name.Replace("stack", "");
            int id_stack = Convert.ToInt32(stack_name);
            id = id_stack;

            currentTimeTextBlock.Text = "00:00:00 / 00:00:00";

            int number = 0;
            foreach (ModelExpo a in listeExpo)
            {
                if (id == a.id)
                {
                    foreach (DiapoModel z in a.ListeDiapo)
                    {
                        number++;
                        //element a ajouter a la liste
                        StackPanel stack = new StackPanel();
                        //grid contenant tous les elements
                        Grid grid1 = new Grid();

                        Image imageToAdd = new Image();
                        BitmapImage imgAdd = new BitmapImage();
                        MediaElement MediaToAdd = new MediaElement();
                        //element panorama a créé ici
                        Image imageTo360 = new Image();
                        BitmapImage imgAdd360 = new BitmapImage();

                        //image du cadre
                        Image imgGrey = new Image();

                        //récupération de l'image fond grey cadre1
                        try
                        {
                            imgGrey.Source = Imaging.CreateBitmapSourceFromHBitmap(
                               CommonSurface.Properties.Resources.CadreListeSlideDiapo.GetHbitmap(),
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());
                        }
                        catch (Exception) { }
                        imgGrey.Width = (thumbnail_width + 20);
                        imgGrey.Height = (thumbnail_height + 20);
                        imgGrey.HorizontalAlignment = HorizontalAlignment.Center;
                        imgGrey.VerticalAlignment = VerticalAlignment.Center;

                        //image de fond du cadre
                        BitmapDecoder decoder = BitmapDecoder.Create(
                            new Uri("pack://application:,,,/CommonSurface;component/Resources/FondTextBoxe.jpg", UriKind.RelativeOrAbsolute),
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.OnLoad);
                        ImageBrush brush = new ImageBrush(decoder.Frames[0]);
                        this.MASK3.Background = brush;

                        //assemblement du cadre et du media
                        grid1.Children.Add(imgGrey);

                        if (z.type == "photo")
                        {
                            try
                            {
                                //recupération de la source
                                imgAdd.BeginInit();
                                imgAdd.UriSource = new Uri(z.element);
                                imgAdd.EndInit();

                                //caractéristique de l'image
                                imageToAdd.Source = imgAdd;
                                imageToAdd.Height = thumbnail_height - 40;
                                imageToAdd.Width = thumbnail_width;
                                imageToAdd.MouseUp += (senderImg, f) => ShowImage(senderImg, f, z);
                                imageToAdd.MouseUp += (senderImg, f) => stack.Opacity = 5.5;
                                imageToAdd.TouchDown += (senderImg, f) => ShowImage(senderImg, f, z);
                                imageToAdd.TouchDown += (senderImg, f) => stack.Opacity = 5.5;
                                imageToAdd.Margin = new Thickness(10, 10, 10, 10);
                                imageToAdd.HorizontalAlignment = HorizontalAlignment.Stretch;
                                grid1.Children.Add(imageToAdd);
                            }
                            catch
                            {
                                TextBlock textblockSource = new TextBlock();

                                //texte erreur de type de fichier
                                textblockSource.Visibility = Visibility.Visible;
                                textblockSource.FontSize = 15;
                                textblockSource.Foreground = new SolidColorBrush(Colors.Red);
                                textblockSource.Text = "ERREUR";
                                textblockSource.HorizontalAlignment = HorizontalAlignment.Center;
                                textblockSource.VerticalAlignment = VerticalAlignment.Center;
                                textblockSource.TextAlignment = TextAlignment.Justify;
                                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;
                                grid1.Children.Add(textblockSource);
                            }
                        }
                        else if (z.type == "video")
                        {
                            try
                            {
                                //caractéristique de la video
                                MediaToAdd.Source = new Uri(z.element);
                                MediaToAdd.Height = thumbnail_height;
                                MediaToAdd.Width = thumbnail_width;
                                MediaToAdd.MouseUp += (senderImg, f) => ShowImage(senderImg, f, z);
                                MediaToAdd.MouseUp += (senderImg, f) => stack.Opacity = 5.5;
                                MediaToAdd.TouchDown += (senderImg, f) => ShowImage(senderImg, f, z);
                                MediaToAdd.TouchDown += (senderImg, f) => stack.Opacity = 5.5;
                                MediaToAdd.Margin = new Thickness(10, 10, 10, 10);
                                MediaToAdd.HorizontalAlignment = HorizontalAlignment.Stretch;
                                MediaToAdd.Volume = 0;
                                MediaToAdd.MediaEnded += new RoutedEventHandler(Media_Ended);
                                grid1.Children.Add(MediaToAdd);
                            }
                            catch
                            {
                                TextBlock textblockSource = new TextBlock();

                                //texte erreur de type de fichier
                                textblockSource.Visibility = Visibility.Visible;
                                textblockSource.FontSize = 15;
                                textblockSource.Foreground = new SolidColorBrush(Colors.Red);
                                textblockSource.Text = "ERREUR";
                                textblockSource.HorizontalAlignment = HorizontalAlignment.Center;
                                textblockSource.VerticalAlignment = VerticalAlignment.Center;
                                textblockSource.TextAlignment = TextAlignment.Justify;
                                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;
                                grid1.Children.Add(textblockSource);
                            }
                        }
                        else if (z.type == "panorama")
                        {
                            try
                            {
                                //recupération de la source
                                imgAdd360.BeginInit();
                                imgAdd360.UriSource = new Uri(cheminDefaut360);
                                imgAdd360.EndInit();

                                //caractéristique de l'image
                                imageTo360.Source = imgAdd360;
                                imageTo360.Height = thumbnail_height;
                                imageTo360.Width = thumbnail_width;
                                imageTo360.MouseUp += (senderImg, f) => ShowImage(senderImg, f, z);
                                imageTo360.MouseUp += (senderImg, f) => stack.Opacity = 5.5;
                                imageTo360.TouchDown += (senderImg, f) => ShowImage(senderImg, f, z);
                                imageTo360.TouchDown += (senderImg, f) => stack.Opacity = 5.5;
                                imageTo360.Margin = new Thickness(10, 10, 10, 10);
                                imageTo360.HorizontalAlignment = HorizontalAlignment.Stretch;
                                grid1.Children.Add(imageTo360);
                            }
                            catch
                            {
                                TextBlock textblockSource = new TextBlock();

                                //texte erreur de type de fichier
                                textblockSource.Visibility = Visibility.Visible;
                                textblockSource.FontSize = 15;
                                textblockSource.Foreground = new SolidColorBrush(Colors.Red);
                                textblockSource.Text = "ERREUR";
                                textblockSource.HorizontalAlignment = HorizontalAlignment.Center;
                                textblockSource.VerticalAlignment = VerticalAlignment.Center;
                                textblockSource.TextAlignment = TextAlignment.Justify;
                                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;
                                grid1.Children.Add(textblockSource);
                            }
                        }

                        stack.Orientation = Orientation.Horizontal;
                        stack.Children.Add(grid1);

                        //Ajout d'une diapo
                        this.dock_expo.Items.Add(stack);
                    }
                }
            }
            //déclaration de la taille du cadre
            this.dock_expo.Width = (thumbnail_width + 40) * number;

            this.Visibility = Visibility.Visible;
        }

        #endregion Public Methods

        #region Private Methods

        //bouton play et stop de la video
        private void BoutonPlayStop(object sender, RoutedEventArgs e)
        {
            Image imgButton = new Image();
            if (play)
            {
                play = false;
                media.LoadedBehavior = MediaState.Manual;
                media.Pause();
                gridplay.Children.Clear();
                //recupération de la sourcebutton
                imgButton.Source = Imaging.CreateBitmapSourceFromHBitmap(
                       CommonSurface.Properties.Resources.play.GetHbitmap(),
                       IntPtr.Zero,
                       Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                play = true;
                media.LoadedBehavior = MediaState.Manual;
                media.Play();
                gridplay.Children.Clear();
                //recupération de la sourcebutton
                imgButton.Source = Imaging.CreateBitmapSourceFromHBitmap(
                       CommonSurface.Properties.Resources.pause.GetHbitmap(),
                       IntPtr.Zero,
                       Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
            }

            //initialisation de l'image
            imgButton.Width = 40;
            imgButton.Height = 40;
            imgButton.HorizontalAlignment = HorizontalAlignment.Center;
            imgButton.VerticalAlignment = VerticalAlignment.Center;
            gridplay.Children.Add(imgButton);
        }

        //bouton retour et de fermeture de la fenetre
        private void BoutonQuit_click(object sender, RoutedEventArgs e)
        {
            ExpoWindow fenetre = new ExpoWindow();
            fenetre.Show();
            this.Close();
        }

        //fonction de lecture et de création d'une diapo
        private void FonctionDiapo(string value, string nameElement)
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

                case "image1":
                    diapo.image1 = value;
                    break;

                case "image2":
                    diapo.image2 = value;
                    break;

                case "source":
                    diapo.source = value;
                    break;
            }
        }

        //execution au debut
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //declaration de text
            grid.Children.Clear();
            try
            {
                TextBlock textblocktitle = new TextBlock();
                TextBlock textblockAll = new TextBlock();
                TextBlock textblockSource = new TextBlock();
                Grid grid1 = new Grid();
                Grid grid2 = new Grid();
                Grid grid3 = new Grid();
                System.Windows.Controls.Image imgButton = new System.Windows.Controls.Image();
                WebBrowser myBrowser = new WebBrowser();

                foreach (ModelExpo i in listeExpo)
                {
                    if (i.id == id)
                    {
                        if (i.ListeDiapo[0].type == "photo")
                        {
                            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                            BitmapImage imgAdd = new BitmapImage();

                            //cacher le button play
                            PlayStop.Visibility = Visibility.Hidden;
                            gridplay.Visibility = Visibility.Hidden;
                            slider.Visibility = Visibility.Hidden;
                            currentTimeTextBlock.Visibility = Visibility.Hidden;

                            //recupération de la source
                            imgAdd.BeginInit();
                            imgAdd.UriSource = new Uri(i.ListeDiapo[0].element);
                            imgAdd.EndInit();

                            //initialisation de l'image
                            img.Source = imgAdd;
                            img.Height = 500;
                            img.Width = this.dock_main_photo.Width;
                            img.HorizontalAlignment = HorizontalAlignment.Center;
                            img.Height = this.dock_main_photo.Height;

                            //Affichage de l'image au lancement de l'expo
                            dock_main_photo.Children.Add(img);
                        }
                        else if (i.ListeDiapo[0].type == "video")
                        {
                            play = true;
                            //button play visible
                            PlayStop.Visibility = Visibility.Visible;
                            gridplay.Visibility = Visibility.Visible;
                            slider.Visibility = Visibility.Visible;
                            currentTimeTextBlock.Visibility = Visibility.Visible;

                            media = new MediaElement();

                            //initialisation de la video
                            media.BeginInit();
                            media.Source = new Uri(i.ListeDiapo[0].element);
                            media.MediaOpened += new RoutedEventHandler(Me_MediaOpened);
                            media.Height = 500;
                            media.Width = this.dock_main_photo.Width;
                            media.HorizontalAlignment = HorizontalAlignment.Center;
                            media.Height = this.dock_main_photo.Height;
                            media.MediaEnded += new RoutedEventHandler(Media_Ended);
                            media.LoadedBehavior = MediaState.Manual;
                            media.Play();
                            media.EndInit();

                            //Affichage de la video au lancement de l'expo
                            dock_main_photo.Children.Add(media);
                        }
                        else if (i.ListeDiapo[0].type == "panorama")
                        {
                            //cacher le button play
                            PlayStop.Visibility = Visibility.Hidden;
                            gridplay.Visibility = Visibility.Hidden;
                            slider.Visibility = Visibility.Hidden;
                            currentTimeTextBlock.Visibility = Visibility.Hidden;
                            //Affichage de l'element panorama au lancement de l'expo
                            myBrowser.Source = new Uri(i.ListeDiapo[0].element);
                            myBrowser.Height = 500;
                            myBrowser.Width = this.dock_main_photo.Width;
                            myBrowser.HorizontalAlignment = HorizontalAlignment.Center;
                            myBrowser.Height = this.dock_main_photo.Height;
                            dock_main_photo.Children.Add(myBrowser);
                        }

                        //recupération de la sourcebutton
                        imgButton.Source = Imaging.CreateBitmapSourceFromHBitmap(
                               CommonSurface.Properties.Resources.pause.GetHbitmap(),
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());

                        //initialisation de l'image
                        imgButton.Width = 40;
                        imgButton.Height = 40;
                        imgButton.HorizontalAlignment = HorizontalAlignment.Center;
                        imgButton.VerticalAlignment = VerticalAlignment.Center;
                        gridplay.Children.Add(imgButton);

                        //grid titre
                        grid1.Margin = new Thickness(10, 30, 10, 30);
                        grid1.HorizontalAlignment = HorizontalAlignment.Center;
                        grid1.VerticalAlignment = VerticalAlignment.Top;

                        //titre
                        textblocktitle.Visibility = Visibility.Visible;
                        textblocktitle.FontSize = 25;
                        textblocktitle.Foreground = new SolidColorBrush(Colors.White);
                        textblocktitle.Text = i.ListeDiapo[0].titre;
                        textblocktitle.HorizontalAlignment = HorizontalAlignment.Center;
                        textblocktitle.TextAlignment = TextAlignment.Justify;
                        textblocktitle.TextWrapping = TextWrapping.WrapWithOverflow;

                        //grid Texte
                        grid2.Margin = new Thickness(10, 150, 10, 60);
                        grid2.HorizontalAlignment = HorizontalAlignment.Left;
                        grid2.VerticalAlignment = VerticalAlignment.Stretch;

                        // Texte
                        textblockAll.Visibility = Visibility.Visible;
                        textblockAll.FontSize = 15;
                        textblockAll.Foreground = new SolidColorBrush(Colors.White);
                        textblockAll.Text = i.ListeDiapo[0].text;
                        textblockAll.HorizontalAlignment = HorizontalAlignment.Left;
                        textblockAll.TextAlignment = TextAlignment.Justify;
                        textblockAll.TextWrapping = TextWrapping.WrapWithOverflow;

                        // Image 1 Variables
                        Image img1 = new Image();
                        BitmapImage imgAdd1 = new BitmapImage();
                        // S'il y a une image dans image 1
                        if (i.ListeDiapo[0].image1 != null)
                        {
                            //recupération de la source
                            imgAdd1.BeginInit();
                            imgAdd1.UriSource = new Uri(i.ListeDiapo[0].image1);
                            imgAdd1.EndInit();

                            //initialisation de l'elements media
                            img1.Source = imgAdd1;

                            //ajoute du text block contenu image 1
                            textblockAll.Inlines.Add(img1);
                        }
                        // Image 2 Variables
                        Image img2 = new Image();
                        BitmapImage imgAdd2 = new BitmapImage();
                        // S'il y a une image dans image 2
                        if (i.ListeDiapo[0].image2 != null)
                        {
                            //recupération de la source
                            imgAdd2.BeginInit();
                            imgAdd2.UriSource = new Uri(i.ListeDiapo[0].image2);
                            imgAdd2.EndInit();

                            //initialisation de l'elements media
                            img2.Source = imgAdd2;
                            //img2.HorizontalAlignment = HorizontalAlignment.Center;

                            //ajoute du text block contenu image 2
                            textblockAll.Inlines.Add(img2);
                        }

                        //grid Source
                        grid3.Margin = new Thickness(20, 200, 10, 30);
                        grid3.HorizontalAlignment = HorizontalAlignment.Left;
                        grid3.VerticalAlignment = VerticalAlignment.Bottom;

                        //titre Source
                        textblockSource.Visibility = Visibility.Visible;
                        textblockSource.FontSize = 15;
                        textblockSource.Foreground = new SolidColorBrush(Colors.White);
                        textblockSource.Text = i.ListeDiapo[0].source;
                        textblockSource.HorizontalAlignment = HorizontalAlignment.Left;
                        textblockSource.TextAlignment = TextAlignment.Justify;
                        textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;

                        //ajoute de tous les textes blocks
                        grid1.Children.Add(textblocktitle);
                        grid2.Children.Add(textblockAll);
                        grid3.Children.Add(textblockSource);

                        //ajoute de tous les grid
                        grid.Children.Add(grid1);
                        grid.Children.Add(grid2);
                        grid.Children.Add(grid3);
                    }
                }
            }
            catch
            {
                TextBlock textblockSource = new TextBlock();

                //texte erreur de type de fichier
                textblockSource.Visibility = Visibility.Visible;
                textblockSource.FontSize = 30;
                textblockSource.Foreground = new SolidColorBrush(Colors.Red);
                textblockSource.Text = "Le type de fichier n'est pas lisible";
                textblockSource.HorizontalAlignment = HorizontalAlignment.Center;
                textblockSource.VerticalAlignment = VerticalAlignment.Center;
                textblockSource.TextAlignment = TextAlignment.Justify;
                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;

                //ajout de l'element créé pour le panorama
                dock_main_photo.Children.Add(textblockSource);
            }
        }

        //relancer la video quand elle se termine
        private void Media_Ended(object sender, RoutedEventArgs e)
        {
            if (sender is MediaElement)
            {
                MediaElement media = (MediaElement)sender;
                media.LoadedBehavior = MediaState.Manual;
                media.Position = new TimeSpan(0, 0, 0);
                media.Play();
            }
        }

        //afficher l'image selectionner dans la liste
        private void ShowImage(object senderImg, EventArgs f, DiapoModel Diapo)
        {
            //netoyer le contenue
            grid.Children.Clear();
            dock_main_photo.Children.Clear();

            try
            {
                //declaration des espaces de textes
                TextBlock textblocktitle = new TextBlock();
                TextBlock textblockAll = new TextBlock();
                TextBlock textblockSource = new TextBlock();
                Grid grid1 = new Grid();
                Grid grid2 = new Grid();
                Grid grid3 = new Grid();
                WebBrowser myBrowser = new WebBrowser();

                //declaration de l'elements média
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                BitmapImage imgAdd = new BitmapImage();

                if (Diapo.type == "photo")
                {
                    //recupération de la source
                    imgAdd.BeginInit();
                    imgAdd.UriSource = new Uri(Diapo.element);
                    imgAdd.EndInit();

                    //initialisation de l'elements media
                    img.Source = imgAdd;
                    img.Height = 500;
                    img.Width = this.dock_main_photo.Width;
                    img.HorizontalAlignment = HorizontalAlignment.Center;
                    img.Height = this.dock_main_photo.Height;
                }
                else if (Diapo.type == "video")
                {
                    media = new MediaElement();
                    //recuperation de la source
                    media.Source = new Uri(Diapo.element);
                    media.MediaOpened += new RoutedEventHandler(Me_MediaOpened);
                    media.Height = 500;
                    media.Width = this.dock_main_photo.Width;
                    media.HorizontalAlignment = HorizontalAlignment.Center;
                    media.Height = this.dock_main_photo.Height;
                    media.MediaEnded += new RoutedEventHandler(Media_Ended);
                }
                else if (Diapo.type == "panorama")
                {
                    //inseré le code pour le lancé en cliquent dans le slide
                    myBrowser.Source = new Uri(Diapo.element);
                    myBrowser.Height = this.dock_main_photo.Height;
                    myBrowser.Width = this.dock_main_photo.Width;
                    myBrowser.HorizontalAlignment = HorizontalAlignment.Center;
                }

                //ecriture du texte sur la texte boxe
                string expo_title = Diapo.type + " " + Diapo.element;

                //grid titre
                grid1.Margin = new Thickness(10, 30, 10, 30);
                grid1.HorizontalAlignment = HorizontalAlignment.Center;
                grid1.VerticalAlignment = VerticalAlignment.Top;

                //titre
                textblocktitle.Visibility = Visibility.Visible;
                textblocktitle.FontSize = 25;
                textblocktitle.Foreground = new SolidColorBrush(Colors.White);
                textblocktitle.Text = Diapo.titre;
                textblocktitle.HorizontalAlignment = HorizontalAlignment.Center;
                textblocktitle.TextAlignment = TextAlignment.Justify;
                textblocktitle.TextWrapping = TextWrapping.WrapWithOverflow;

                //ajoute du text block titre
                grid1.Children.Add(textblocktitle);

                //grid Contenu
                grid2.Margin = new Thickness(10, 150, 10, 60);
                grid2.HorizontalAlignment = HorizontalAlignment.Left;
                grid2.VerticalAlignment = VerticalAlignment.Stretch;

                // Texte
                textblockAll.Visibility = Visibility.Visible;
                textblockAll.FontSize = 15;
                textblockAll.Foreground = new SolidColorBrush(Colors.White);
                textblockAll.Text = Diapo.text;
                textblockAll.HorizontalAlignment = HorizontalAlignment.Left;
                textblockAll.TextAlignment = TextAlignment.Justify;
                textblockAll.TextWrapping = TextWrapping.WrapWithOverflow;

                // Image 1 Variables
                Image img1 = new Image();
                BitmapImage imgAdd1 = new BitmapImage();
                // S'il y a une image dans image 1
                if (Diapo.image1 != null)
                {
                    //recupération de la source
                    imgAdd1.BeginInit();
                    imgAdd1.UriSource = new Uri(Diapo.image1);
                    imgAdd1.EndInit();

                    //initialisation de l'elements media
                    img1.Source = imgAdd1;
                    //img1.HorizontalAlignment = HorizontalAlignment.Center;

                    //ajoute du text block contenu image 1
                    textblockAll.Inlines.Add(img1);
                }
                // Image 2 Variables
                Image img2 = new Image();
                BitmapImage imgAdd2 = new BitmapImage();
                // S'il y a une image dans image 2
                if (Diapo.image2 != null)
                {
                    //recupération de la source
                    imgAdd2.BeginInit();
                    imgAdd2.UriSource = new Uri(Diapo.image2);
                    imgAdd2.EndInit();

                    //initialisation de l'elements media
                    img2.Source = imgAdd2;
                    //img2.HorizontalAlignment = HorizontalAlignment.Center;

                    //ajoute du text block contenu image 2
                    textblockAll.Inlines.Add(img2);
                }

                //ajoute du text block contenu texte
                grid2.Children.Add(textblockAll);

                //grid Source
                grid3.Margin = new Thickness(20, 200, 10, 30);
                grid3.HorizontalAlignment = HorizontalAlignment.Left;
                grid3.VerticalAlignment = VerticalAlignment.Bottom;

                //titre Source
                textblockSource.Visibility = Visibility.Visible;
                textblockSource.FontSize = 15;
                textblockSource.Foreground = new SolidColorBrush(Colors.White);
                textblockSource.Text = Diapo.source;
                textblockSource.HorizontalAlignment = HorizontalAlignment.Left;
                textblockSource.TextAlignment = TextAlignment.Justify;
                textblockSource.TextWrapping = TextWrapping.WrapWithOverflow;

                //ajoute du text block Source
                grid3.Children.Add(textblockSource);

                //ajoute de tous les grid
                grid.Children.Add(grid1);
                grid.Children.Add(grid2);
                grid.Children.Add(grid3);

                if (Diapo.type == "photo")
                {
                    //ajout de l'image
                    dock_main_photo.Children.Add(img);
                    //cacher le button play
                    PlayStop.Visibility = Visibility.Hidden;
                    gridplay.Visibility = Visibility.Hidden;
                    slider.Visibility = Visibility.Hidden;
                    currentTimeTextBlock.Visibility = Visibility.Hidden;
                }
                else if (Diapo.type == "video")
                {
                    play = true;
                    media.LoadedBehavior = MediaState.Manual;
                    media.Play();
                    //timer.Start();

                    //ajout du media
                    dock_main_photo.Children.Add(media);
                    //button play visible
                    PlayStop.Visibility = Visibility.Visible;
                    gridplay.Visibility = Visibility.Visible;
                    slider.Visibility = Visibility.Visible;
                    currentTimeTextBlock.Visibility = Visibility.Visible;
                }
                else if (Diapo.type == "panorama")
                {
                    //ajout de l'element créé pour le panorama
                    dock_main_photo.Children.Add(myBrowser);
                    //cacher le button play
                    PlayStop.Visibility = Visibility.Hidden;
                    gridplay.Visibility = Visibility.Hidden;
                    slider.Visibility = Visibility.Hidden;
                    currentTimeTextBlock.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                TextBlock textblockSource = new TextBlock
                {
                    //texte erreur de type de fichier
                    Visibility = Visibility.Visible,
                    FontSize = 30,
                    Foreground = new SolidColorBrush(Colors.Red),
                    Text = "Le type de fichier n'est pas lisible",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Justify,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };

                //ajout de l'element créé pour le panorama
                dock_main_photo.Children.Add(textblockSource);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TotalTime.TotalSeconds > 0)
            {
                media.Position = TimeSpan.FromSeconds(slider.Value / slider.TickFrequency);
            }
        }

        //changement de la valeur du slide au fure et a mesure du temps qui passe
        private void Timer_Tick(object sender, EventArgs e)
        {
            //MediaElement media = (MediaElement)sender;
            if (media.NaturalDuration.HasTimeSpan)
            {
                if (media.NaturalDuration.TimeSpan.TotalSeconds > 0)
                {
                    if (TotalTime.TotalSeconds > 0)
                    {
                        // Updating time slider
                        slider.Value = media.Position.TotalSeconds * slider.TickFrequency;

                        currentTimeTextBlock.Text = media.Position.ToString(@"hh\:mm\:ss") + " / " + TotalTime.ToString(@"hh\:mm\:ss");
                    }
                }
            }
        }

        #endregion Private Methods
    }
}