using CommonSurface.Model;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace AppAdministrationWPF.View
{
    /// <summary>
    /// Logique d'interaction pour ButtonDialog.xaml
    /// </summary>
    public partial class MenuIconEditDialog : Window
    {
        #region Private Fields

        private Icon _new_icon;
        private Icon _old_icon;

        #endregion Private Fields

        #region Public Constructors

        public MenuIconEditDialog(Icon icon)
        {
            InitializeComponent();

            // Initialise la preview si un bouton à été sélectionné précédemment
            if (icon != null)
            {
                this._old_icon = icon;
                this._new_icon = new Icon(icon);

                // Image
                txtImageURI.Text = _new_icon.Source;

                // Texte du label
                txtImageLabel.Text = _new_icon.Text;

                this.DataContext = _new_icon;
            }
            else
            {
                this.Close();
            }
        }

        #endregion Public Constructors

        #region Private Methods

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Sélection de l'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void buttonSearchFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Photos (*.jpg, *.png)|*.jpg;*.png";
            fileDialog.FilterIndex = 0;
            fileDialog.Multiselect = false;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                _new_icon.Source = txtImageURI.Text = fileDialog.FileName;
            }
        }

        private void buttonValidate_Click(object sender, RoutedEventArgs e)
        {
            _old_icon.Text = _new_icon.Text;
            _old_icon.Source = _new_icon.Source;

            this.Close();
        }

        /// <summary>
        /// Met à jour le label de la preview lorsque le texte est modifié
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void updateLabelPreview(object sender, KeyEventArgs e)
        {
            _new_icon.Text = txtImageLabel.Text;
        }

        #endregion Private Methods
    }
}