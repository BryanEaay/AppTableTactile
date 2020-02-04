using AppAdministrationWPF.Utils;
using CommonSurface.Other;
using System;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AppAdministrationWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        #region Public Constructors

        public AdminWindow()
        {
            InitializeComponent();
            this.DataContext = ServiceLocator.MainPageViewModel;

            Action<string> initBar = (string name) =>
            {
                ImageBrush menu = this.FindName("menu" + name) as ImageBrush;
                Label label = this.FindName("label" + name) as Label;
                Button button = this.FindName("button" + name) as Button;

                menu.ImageSource = ResourceAccessor.loadImage(ConfigurationManager.AppSettings["cheminBandeau" + name]);
                label.Content = ConfigurationManager.AppSettings["valeurBandeau" + name];
                button.Visibility = Visibility.Visible;
                label.Visibility = Visibility.Visible;
            };

            // Initialisation des barres
            initBar("Menu");
            initBar("Visite");
            initBar("Region");
            initBar("Mediatheque");
            initBar("Memory");
            initBar("Puzzle");
        }

        #endregion Public Constructors

        #region Private Methods

        private void hoverOn(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button but = (System.Windows.Controls.Button)(sender as FrameworkElement);
            Storyboard sbAddSel = this.Resources["animHover"] as Storyboard;

            Storyboard.SetTarget(sbAddSel, ((Button)sender));
            sbAddSel.Begin();
        }

        private void selChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button but = (System.Windows.Controls.Button)(sender as FrameworkElement);
            Storyboard sbRemoveSel = this.Resources["animClickLeft"] as Storyboard, sbAddSel = this.Resources["animClick"] as Storyboard;

            foreach (StackPanel b in GridMenu.Children.OfType<StackPanel>())
            {
                Storyboard.SetTarget(sbRemoveSel, b.Children.OfType<Button>().First());
                sbRemoveSel.Begin();
            }
            Storyboard.SetTarget(sbAddSel, ((Button)sender));
            sbAddSel.Begin();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServiceLocator.MainPageViewModel.CentralElement = null;
            Application.Current.Shutdown();
        }

        #endregion Private Methods
    }
}