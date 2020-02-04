using AppAdministrationWPF.ViewModel;
using CommonSurface.Model;
using Microsoft.Win32;
using System;
using System.Windows;

namespace AppAdministrationWPF.View
{
    /// <summary>
    /// Logique d'interaction pour MediaDialog.xaml
    /// </summary>
    public partial class MediaDialog : Window
    {
        #region Private Fields

        private MediaDialogModel _viewModel;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Use a COPY
        /// </summary>
        /// <param name="media"></param>
        public MediaDialog(MediaDialogModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
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

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
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
                    ViewModel.Name = name;
                }
                catch (Exception) { }
            }
        }

        #endregion Private Methods
    }
}