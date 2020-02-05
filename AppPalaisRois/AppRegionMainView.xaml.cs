using AppPalaisRois.ViewModel;
using CommonSurface.Model;
using CommonSurface.Other;
using FluidKit.Controls;
using Microsoft.Surface.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AppPalaisRois
{
    public partial class AppRegionMainView : Window
    {
        #region Private Fields

        private int actualIndex = 0;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private Dictionary<PlaceHolder, List<ScatterViewItem>> displayedItems;
        private List<ScatterViewItem> itemsGarbage = new List<ScatterViewItem>();
        private ResourceDictionary myresourcedictionary;
        private Storyboard sbFloat, sbDefloat, sbHide;
        private List<object> selectedFramework = new List<object>();
        private List<PlaceHolder> selectedOnes;
        private AppRegionMainViewModel ViewModel;

        #endregion Private Fields

        #region Public Constructors

        public AppRegionMainView()
        {
            InitializeComponent();

            ViewModel = new AppRegionMainViewModel();
            this.DataContext = ViewModel;

            displayedItems = new Dictionary<PlaceHolder, List<ScatterViewItem>>();

            //// Récupération de la frise
            imageFond.Source = ResourceAccessor.loadImage("/CommonSurface;component/Resources/fond_sans_griffe.jpg");
            returnRegion.Source = ResourceAccessor.loadImage("/CommonSurface;component/Resources/return.png");

            ///* ANIM PART*/
            ///* EFFECTS RESOURCE DICTIONARY */
            myresourcedictionary = new ResourceDictionary();
            myresourcedictionary.Source = new Uri("/CommonSurface;component/XAML/Effects.xaml", UriKind.RelativeOrAbsolute);
            sbHide = myresourcedictionary["hideAnimSec"] as Storyboard;

            ///* GRAB STORYBOARDS FROM RD */
            sbFloat = myresourcedictionary["animFloat"] as Storyboard;
            sbDefloat = myresourcedictionary["animDefloat"] as Storyboard;

            selectedOnes = new List<PlaceHolder>();
            flowCarte.Layout = new Rolodex();

            List<Map> temp = new List<Map>(ViewModel.Maps);
            temp.Sort((a, b) => (a.ID.CompareTo(b.ID)));
            ViewModel.Maps = new ObservableCollection<Map>(temp);

            StringCollection itemssource = new StringCollection();
            foreach (Map map in ViewModel.Maps)
            {
                itemssource.Add(map.Background);
            }

            flowCarte.SelectedIndex = 0;
            flowCarte.ItemsSource = itemssource;
            listboxMaps.SelectedIndex = 0;
        }

        #endregion Public Constructors

        #region Private Destructors

        ~AppRegionMainView()
        {
            actualIndex = 0;
            ViewModel = null;
            displayedItems = null;
            itemsGarbage = null;
            selectedOnes = null;
            selectedFramework = null;
            myresourcedictionary = null;
            sbFloat = sbDefloat = sbHide = null;
            flowCarte = null;
        }

        #endregion Private Destructors

        #region Public Methods

        /// <summary>
        /// Fermeture du media
        /// </summary>
        /// <param name="svi"></param>
        public void CloseMedia(ScatterViewItem svi)
        {
            svi.BeginAnimation(ScatterViewItem.WidthProperty, new DoubleAnimation()
            {
                From = svi.ActualWidth,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(100),
                FillBehavior = FillBehavior.HoldEnd
            });

            svi.BeginAnimation(ScatterViewItem.HeightProperty, new DoubleAnimation()
            {
                From = svi.ActualHeight,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(100),
                FillBehavior = FillBehavior.HoldEnd
            });
            svi.BeginAnimation(ScatterViewItem.OpacityProperty, new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                FillBehavior = FillBehavior.HoldEnd
            });

            itemsGarbage.Add(svi);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            DataContext = null;
            ViewModel = null;
            foreach (var item in displayedItems.Keys)
            {
                displayedItems[item].Clear();
                displayedItems.Remove(item);
            }
            itemsGarbage.Clear();
            selectedOnes.Clear();
            selectedFramework.Clear();
            ScatterView.Items.Clear();
        }

        #endregion Protected Methods

        #region Events

        /// <summary>
        /// Animation terminée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void anim_Completed(object sender, EventArgs e)
        {
            List<ScatterViewItem> items = new List<ScatterViewItem>();
            foreach (ScatterViewItem item in itemsGarbage)
            {
                if (item.Opacity <= 0)
                {
                    ScatterView.Items.Remove(item);
                    items.Add(item);
                }
            }
            foreach (ScatterViewItem item in items)
            {
                itemsGarbage.Remove(item);
            }
        }

        //bouton de fermeture et ouverture du menu principale
        private void BoutonQuit_click(object sender, RoutedEventArgs e)
        {
            //Effet de fermeture
            Storyboard.SetTarget(sbHide, regionPanel);
            sbHide.Completed += (s, t) =>
            {
                //Fermeture et ouverture des fenetres
                MainWindow fenetre = new MainWindow();
                fenetre.Show();
                this.Close();
            };
            sbHide.Begin();
        }

        /// <summary>
        /// Activation d'un conteneur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void item_ContainerActivated(object sender, RoutedEventArgs e)
        {
            ScatterViewItem item = (ScatterViewItem)sender;
            Grid grid = (Grid)item.Content;
            foreach (UIElement uiel in grid.Children)
            {
                if (uiel is Label)
                {
                    uiel.BeginAnimation(Label.OpacityProperty, new DoubleAnimation()
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromMilliseconds(500))
                    });
                    break;
                }
            }
        }

        /// <summary>
        /// Désactivation du conteneur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void item_ContainerDeactivated(object sender, RoutedEventArgs e)
        {
            ScatterViewItem item = (ScatterViewItem)sender;
            Grid grid = (Grid)item.Content;
            foreach (UIElement uiel in grid.Children)
            {
                if (uiel is Label)
                {
                    uiel.BeginAnimation(Label.OpacityProperty, new DoubleAnimation()
                    {
                        From = 1,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(500))
                    });
                    break;
                }
            }
        }

        /// <summary>
        /// Relance le media lorsqu'il se termine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
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

        /// <summary>
        /// Apparition du media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void OnOpened(object sender, RoutedEventArgs e)
        {
            // Variables de taille du media
            float MinHeight = 180;
            float MinWidth = 320;

            float FinalHeight = 0;
            float FinalWidth = 0;

            float MaxHeight = 880;
            float MaxWidth = 1720;

            // Récupère le MediaElement
            MediaElement mediaElement = (MediaElement)sender;

            // S'il s'agit d'un MediaElement
            if (mediaElement.Parent is Grid)
            {
                Grid grid = (Grid)mediaElement.Parent;
                mediaElement.Stretch = Stretch.UniformToFill;

                // Recupere le scatterViewItem
                ScatterViewItem scatterViewItem = (ScatterViewItem)grid.Parent;

                // Recuperer la taille du media
                float CurrentWidth = mediaElement.NaturalVideoWidth;
                float CurrentHeight = mediaElement.NaturalVideoHeight;

                // Si la largeur est supérieur à la hauteur => Mode paysage
                if (CurrentWidth >= CurrentHeight)
                {
                    // Limitation de la taille max du media
                    scatterViewItem.MaxHeight = 900;
                    scatterViewItem.MaxWidth = 1600;

                    // Dimension minimal du media
                    MinHeight = 180;
                    MinWidth = 320;

                    float Height = (CurrentHeight / CurrentWidth) * MinWidth;

                    FinalWidth = MinWidth;
                    FinalHeight = Height;

                    if (Height < MinHeight)
                    {
                        float Width = (CurrentWidth / CurrentHeight) * MinHeight;

                        FinalWidth = Width;
                        FinalHeight = MinHeight;
                    }
                }
                // Sinon Mode portrait
                else
                {
                    // Limitation de la taille max du media
                    scatterViewItem.MaxHeight = 1600;
                    scatterViewItem.MaxWidth = 900;

                    // Dimension minimal du media
                    MinHeight = 320;
                    MinWidth = 180;

                    float Width = (CurrentWidth / CurrentHeight) * MinHeight;

                    FinalWidth = Width;
                    FinalHeight = MinHeight;

                    if (Width < MinWidth)
                    {
                        float Height = (CurrentHeight / CurrentWidth) * MinWidth;

                        FinalWidth = MinWidth;
                        FinalHeight = Height;
                    }
                }

                // Limiter la largeur max
                if (FinalWidth > MaxWidth)
                {
                    FinalWidth = MaxWidth;
                }

                if (FinalWidth == float.NaN)
                {
                    FinalWidth = 0;
                }

                // Limiter la hauteur max
                if (FinalHeight > MaxHeight)
                {
                    FinalHeight = MaxHeight;
                }

                if (FinalHeight == float.NaN)
                {
                    FinalHeight = 0;
                }

                // Animation de la largeur
                scatterViewItem.BeginAnimation(ScatterViewItem.WidthProperty, new DoubleAnimation()
                {
                    From = scatterViewItem.ActualWidth,
                    To = FinalWidth,
                    Duration = TimeSpan.FromMilliseconds(500),
                    FillBehavior = FillBehavior.Stop
                });

                // Animation de la hauteur
                scatterViewItem.BeginAnimation(ScatterViewItem.HeightProperty, new DoubleAnimation()
                {
                    From = scatterViewItem.ActualHeight,
                    To = FinalHeight,
                    Duration = TimeSpan.FromMilliseconds(500),
                    FillBehavior = FillBehavior.Stop
                });

                // Permettre la manipulation du media
                scatterViewItem.CanScale = true;
                scatterViewItem.CanMove = true;
                scatterViewItem.CanRotate = true;
                scatterViewItem.IsManipulationEnabled = true;
            }
        }

        /// <summary>
        /// Sélection d'un placeholder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void selectOne(object sender, RoutedEventArgs e)
        {
            PlaceHolder selectedOne = (PlaceHolder)((sender as FrameworkElement)).DataContext;
            FrameworkElement buttonSelected = (sender as FrameworkElement);

            if (selectedOnes.Contains(selectedOne))
            {
                Storyboard.SetTarget(sbDefloat, buttonSelected);
                buttonSelected.BeginStoryboard(sbDefloat);
                selectedOnes.Remove(selectedOne);
                HidePlaceHolder(selectedOne);
                if (selectedFramework.Contains(buttonSelected))
                {
                    selectedFramework.Remove(sender);
                }
            }
            else
            {
                Storyboard.SetTarget(sbFloat, buttonSelected);
                buttonSelected.BeginStoryboard(sbFloat);
                selectedOnes.Add(selectedOne);
                DisplayPlaceHolder(selectedOne);
                if (!selectedFramework.Contains(buttonSelected))
                {
                    selectedFramework.Add(sender);
                }
            }
        }

        #endregion Events

        #region Private Methods

        /// <summary>
        /// Changement de la carte
        /// </summary>
        private void changeMap()
        {
            if (itemsGarbage.Count > 0 || listboxMaps.SelectedItem == null)
            {
                return;
            }

            // On vide les listes
            displayedItems.Clear();
            selectedOnes.Clear();
            itemsGarbage.Clear();
            ScatterView.Items.Clear();

            // Détection du changement de l'index
            if (listboxMaps.SelectedIndex != actualIndex)
            {
                flowCarte.SelectedIndex = listboxMaps.SelectedIndex;
                actualIndex = listboxMaps.SelectedIndex;
            }
            else
            {
                listboxMaps.SelectedIndex = flowCarte.SelectedIndex;
                actualIndex = flowCarte.SelectedIndex;
            }

            // Modèle contenant la carte avec les points
            ViewModel.Selected = null;

            // Délai dans l'affichage des placeholders
            dispatcherTimer.Tick += (s, t) =>
            {
                ViewModel.Selected = listboxMaps.Items[actualIndex] as Map;
                dispatcherTimer.Stop();
            };
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Supprssion des medias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void clearElements()
        {
            foreach (var el in selectedFramework)
            {
                PlaceHolder ph = (el as FrameworkElement).DataContext as PlaceHolder;
                if (selectedOnes.Contains(ph))
                {
                    PlaceHolder p = (PlaceHolder)((el as FrameworkElement)).DataContext;
                    FrameworkElement buttonSelected = (el as FrameworkElement);

                    if (selectedOnes.Contains(p))
                    {
                        Storyboard.SetTarget(sbDefloat, buttonSelected);
                        buttonSelected.BeginStoryboard(sbDefloat);
                        selectedOnes.Remove(p);

                        /* * * * * * * * * * * * * * *  Retirement des medias  * * * * * * * * * * * * * */
                        if (displayedItems.ContainsKey(p))
                        {
                            foreach (ScatterViewItem svi in displayedItems[p])
                            {
                                PointAnimation anim = new PointAnimation();
                                anim.From = new Point(svi.ActualCenter.X, svi.ActualCenter.Y);
                                anim.To = new Point(p.X, p.Y);
                                anim.Duration = new Duration(TimeSpan.FromMilliseconds(450));
                                anim.Completed += (a, b) =>
                                {
                                    anim_Completed(null, null);
                                    changeMap();
                                };

                                svi.BeginAnimation(ScatterViewItem.HeightProperty, new DoubleAnimation()
                                {
                                    From = svi.ActualHeight,
                                    To = 0,
                                    Duration = new Duration(TimeSpan.FromMilliseconds(450))
                                });
                                svi.BeginAnimation(ScatterViewItem.WidthProperty, new DoubleAnimation()
                                {
                                    From = svi.ActualWidth,
                                    To = 0,
                                    Duration = new Duration(TimeSpan.FromMilliseconds(450))
                                });
                                svi.BeginAnimation(ScatterViewItem.OpacityProperty, new DoubleAnimation()
                                {
                                    From = 1,
                                    To = 0,
                                    Duration = new Duration(TimeSpan.FromMilliseconds(450))
                                });
                                svi.BeginAnimation(ScatterViewItem.CenterProperty, anim);
                                itemsGarbage.Add(svi);
                            }
                            displayedItems.Remove(p);
                        }
                        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
                    }
                }
            }
            selectedFramework.Clear();
        }

        /// <summary>
        /// Désactivation la sélection via la molette
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void disableSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Force l'index de la carte à celle de la liste
            //flowCarte.SelectedIndex = listboxMaps.SelectedIndex;
        }

        /// <summary>
        /// Display the given media placeholder relative to the original placeholder
        /// </summary>
        private void DisplayPlaceHolder(PlaceHolder placeholder)
        {
            if (displayedItems.ContainsKey(placeholder))
            {
                Console.WriteLine("Trying to display something already displayed...");
                return;
            }
            List<ScatterViewItem> items = new List<ScatterViewItem>();

            int totalMedia = placeholder.Media.Count;
            int currentMedia = 0;
            foreach (Media media in placeholder.Media)
            {
                ScatterViewItem item = new ScatterViewItem();
                item.SetRelativeZIndex(RelativeScatterViewZIndex.Topmost);

                item.ContainerActivated += new RoutedEventHandler(item_ContainerActivated);
                item.ContainerDeactivated += new RoutedEventHandler(item_ContainerDeactivated);
                Grid content = new Grid();
                content.VerticalAlignment = VerticalAlignment.Stretch;
                content.HorizontalAlignment = HorizontalAlignment.Stretch;

                MediaElement mediaElement = new MediaElement();
                mediaElement.BeginInit();
                try
                {
                    // Image ou vidéo
                    content.Children.Add(mediaElement);
                    mediaElement.Source = new Uri(media.Path);
                    mediaElement.Stretch = Stretch.Uniform;
                    mediaElement.Volume = 0;
                    mediaElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                    mediaElement.VerticalAlignment = VerticalAlignment.Stretch;
                    mediaElement.MediaOpened += new RoutedEventHandler(OnOpened);
                    mediaElement.MediaEnded += new RoutedEventHandler(Media_Ended);
                    mediaElement.EndInit();

                    // Nom du media
                    Label lblMediaName = new Label();
                    lblMediaName.Content = media.Name;
                    lblMediaName.HorizontalAlignment = HorizontalAlignment.Stretch;
                    lblMediaName.VerticalAlignment = VerticalAlignment.Bottom;
                    lblMediaName.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#96000000"));
                    lblMediaName.Foreground = new SolidColorBrush(Colors.White);
                    lblMediaName.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Luciole");
                    lblMediaName.Opacity = 0;
                    content.Children.Add(lblMediaName);

                    // Bouton pour fermer le media
                    Image playImg = new Image();

                    try
                    {
                        playImg.Source = Imaging.CreateBitmapSourceFromHBitmap(
                           CommonSurface.Properties.Resources.closebutton.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
                    }
                    catch (Exception)
                    {
                        playImg.Source = null;
                        continue;
                    }

                    playImg.HorizontalAlignment = HorizontalAlignment.Center;
                    playImg.VerticalAlignment = VerticalAlignment.Center;
                    playImg.Stretch = Stretch.Uniform;
                    playImg.Height = 15;
                    playImg.Width = 15;

                    Button closeBut = new Button();
                    closeBut.VerticalAlignment = VerticalAlignment.Top;
                    closeBut.HorizontalAlignment = HorizontalAlignment.Right;
                    closeBut.Height = 20;
                    closeBut.Width = 20;
                    closeBut.Background = Brushes.Black;
                    closeBut.Content = playImg;
                    closeBut.BorderBrush = new SolidColorBrush(Colors.White);

                    closeBut.TouchDown += (s, t) =>
                    {
                        CloseMedia(item);
                    };
                    closeBut.Click += (s, t) =>
                    {
                        CloseMedia(item);
                    };
                    content.Children.Add(closeBut);
                    item.Content = content;

                    items.Add(item);
                    ScatterView sv = ScatterView;

                    item.CanMove = true;
                    item.CanRotate = true;
                    item.CanScale = true;

                    sv.Items.Add(item);

                    //animation
                    double teta = Math.PI * (currentMedia / (float)totalMedia * 360) / 180;
                    double x = 200 * Math.Cos(teta);
                    double y = 200 * Math.Sin(teta);

                    double endX = placeholder.X + x;
                    double endY = placeholder.Y + y;

                    item.BeginAnimation(ScatterViewItem.CenterProperty, new PointAnimation()
                    {
                        From = new Point(placeholder.X, placeholder.Y),
                        To = new Point(endX, endY),
                        Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                        FillBehavior = FillBehavior.Stop
                    });
                    item.BeginAnimation(ScatterViewItem.OpacityProperty, new DoubleAnimation()
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                        FillBehavior = FillBehavior.Stop
                    });

                    currentMedia++;
                }
                catch (UriFormatException)
                {
                }
            }
            displayedItems.Add(placeholder, items);
        }

        /// <summary>
        /// Hide media from the given placeholder
        /// </summary>
        private void HidePlaceHolder(PlaceHolder p)
        {
            if (displayedItems.ContainsKey(p))
            {
                foreach (ScatterViewItem svi in displayedItems[p])
                {
                    PointAnimation anim = new PointAnimation();
                    anim.From = new Point(svi.ActualCenter.X, svi.ActualCenter.Y);
                    anim.To = new Point(p.X, p.Y);
                    anim.Duration = new Duration(TimeSpan.FromMilliseconds(450));
                    anim.Completed += new EventHandler(anim_Completed);

                    svi.BeginAnimation(ScatterViewItem.HeightProperty, new DoubleAnimation()
                    {
                        From = svi.ActualHeight,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(450))
                    });
                    svi.BeginAnimation(ScatterViewItem.WidthProperty, new DoubleAnimation()
                    {
                        From = svi.ActualWidth,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(450))
                    });
                    svi.BeginAnimation(ScatterViewItem.OpacityProperty, new DoubleAnimation()
                    {
                        From = 1,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(450))
                    });
                    svi.BeginAnimation(ScatterViewItem.CenterProperty, anim);
                    itemsGarbage.Add(svi);
                }
                displayedItems.Remove(p);
            }
            else
            {
                Console.WriteLine("Trying to hide something that is not displayed");
            }
        }

        /// <summary>
        /// Change la carte sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void mapSelection(object sender, SelectionChangedEventArgs e)
        {
            if (selectedFramework.Count > 0)
            {
                clearElements();
            }
            else
            {
                changeMap();
            }
        }

        #endregion Private Methods
    }
}