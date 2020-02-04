using CommonSurface.Other;
using Microsoft.Surface.Presentation.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppPalaisRois
{
    public class BoutonBarreMenu
    {
        #region Private Fields

        private RoutedEventHandler handler;
        private SurfaceButton quitButton;
        private Image quitButtonImg;
        private String url;

        #endregion Private Fields

        #region Public Constructors

        public BoutonBarreMenu(RoutedEventHandler handler, String url)
        {
            this.handler = handler;
            this.url = url;
            quitButton = new SurfaceButton
            {
                Background = Brushes.Black
            };
            setImg(this.url);
            quitButton.Click += this.handler;
        }

        #endregion Public Constructors

        #region Public Methods

        public SurfaceButton getButton()
        {
            return quitButton;
        }

        public Image getImg()
        {
            return quitButtonImg;
        }

        public void setImg(String url)
        {
            quitButtonImg = new Image();
            quitButtonImg.Stretch = Stretch.Uniform;
            quitButtonImg.Source = ResourceAccessor.loadImage(url);
            quitButton.Content = quitButtonImg;
        }

        #endregion Public Methods
    }
}