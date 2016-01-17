using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyDentDiagnostics
{
	/// <summary>
	/// Interaction logic for AddEditDiagnosticModal.xaml
	/// </summary>
	public partial class AddEditDiagnosticModal : Window
	{
        #region Instance variables
        private Model.Diagnostic _selectedDiagnostic;
        private Model.Area _selectedArea;
        private bool _isUpdateDiagnostic;
        #endregion

        #region Constructors
        public AddEditDiagnosticModal(Model.Diagnostic selectedDiagnostic, Model.Area selectedArea)
        {
            this.InitializeComponent();

            _selectedArea = selectedArea;
            _selectedDiagnostic = selectedDiagnostic;
            _isUpdateDiagnostic = selectedDiagnostic != null;

            btnSearchImage1.Tag = btnRemoveImage1.Tag = diagnosticImage1;
            btnSearchImage2.Tag = btnRemoveImage2.Tag = diagnosticImage2;

            if (_isUpdateDiagnostic)
                PrepareWindowForUpdates();
        }
        #endregion

        #region Window event handlers
        private void btnSearchImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SearchImage((sender as Button).Tag as Image);
        }

        private void btnRemoveImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Image imageControl = (sender as Button).Tag as Image;
            imageControl.Source = null;
            imageControl.ToolTip = null;
        }

        private void diagnosticImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Image imageControl = sender as Image;

                if (imageControl.ToolTip != null && string.IsNullOrEmpty(imageControl.ToolTip.ToString()) == false)
                    Process.Start(imageControl.ToolTip.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la imagen\n\nDetalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string diagnosticName = txtDiagnosticName.Text.Trim();
            string diagnosticDescription = txtDiagnosticDescription.Text.Trim();
            string picturePath1 = diagnosticImage1.ToolTip == null ? null : diagnosticImage1.ToolTip.ToString();
            string picturePath2 = diagnosticImage2.ToolTip == null ? null : diagnosticImage2.ToolTip.ToString();

            if (string.IsNullOrEmpty(diagnosticName))
            {
                MessageBox.Show("Ingrese el nombre del diagnóstico", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (_isUpdateDiagnostic)
            {
                _selectedDiagnostic.Name = diagnosticName;
                _selectedDiagnostic.Description = diagnosticDescription;
                _selectedDiagnostic.PicturePath1 = picturePath1;
                _selectedDiagnostic.PicturePath2 = picturePath2;

                if (Controllers.BusinessController.Instance.Update<Model.Diagnostic>(_selectedDiagnostic))
                    this.Close();
                else
                    MessageBox.Show("No se pudo actualizar el diagnóstico", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Model.Diagnostic diagnosticToAdd = new Model.Diagnostic()
                {
                    AreaId = _selectedArea.AreaId,
                    Name = diagnosticName,
                    Description = diagnosticDescription,
                    PicturePath1 = picturePath1,
                    PicturePath2 = picturePath2,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                if (Controllers.BusinessController.Instance.Add<Model.Diagnostic>(diagnosticToAdd))
                    this.Close();
                else
                    MessageBox.Show("No se pudo agregar el diagnóstico", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Window's logic
        private void SearchImage(Image imageControl)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = Controllers.Utils.GetFileDialogWithImageExtensions();

                // Get the selected file name and display in a TextBox 
                if (dlg.ShowDialog() == true)
                {
                    imageControl.Source = new BitmapImage(new Uri(dlg.FileName));
                    imageControl.ToolTip = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen seleccionada\n\nDetalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrepareWindowForUpdates()
        {
            txtDiagnosticName.Text = _selectedDiagnostic.Name;
            txtDiagnosticDescription.Text = _selectedDiagnostic.Description;
            this.Title = "Actualizar información del diagnóstico";
            btnAddUpdate.Content = "Actualizar";

            SetImage(_selectedDiagnostic.PicturePath1, diagnosticImage1);
            SetImage(_selectedDiagnostic.PicturePath2, diagnosticImage2);
        }

        private void SetImage(string imagePath, Image imageControl)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath) == false)
                {
                    imageControl.Source = new BitmapImage(new Uri(imagePath));
                    imageControl.ToolTip = imagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen\n\nDetalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}