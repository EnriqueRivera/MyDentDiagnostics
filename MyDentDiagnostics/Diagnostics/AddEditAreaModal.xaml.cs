using System;
using System.Collections.Generic;
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
	/// Interaction logic for AddEditAreaModal.xaml
	/// </summary>
	public partial class AddEditAreaModal : Window
	{
        #region Instance variables
        private Model.Area _selectedArea;
        private bool _isUpdateArea;
        #endregion

        #region Constructors
        public AddEditAreaModal(Model.Area selectedArea)
        {
            this.InitializeComponent();

            _selectedArea = selectedArea;
            _isUpdateArea = selectedArea != null;

            if (_isUpdateArea)
                PrepareWindowForUpdates();
        }
        #endregion

        #region Window event handlers
        private void btnAddUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            string areaName = txtAreaName.Text.Trim();

            if (string.IsNullOrEmpty(areaName))
            {
                MessageBox.Show("Ingrese el nombre del área", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (_isUpdateArea)
            {
                _selectedArea.Name = areaName;

                if (Controllers.BusinessController.Instance.Update<Model.Area>(_selectedArea))
                    this.Close();
                else
                    MessageBox.Show("No se pudo actualizar el área", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Model.Area areaToAdd = new Model.Area()
                {
                    Name = areaName,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                if (Controllers.BusinessController.Instance.Add<Model.Area>(areaToAdd))
                    this.Close();
                else
                    MessageBox.Show("No se pudo agregar el área", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
		}

		private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            this.Close();
		}
        #endregion

        #region Window's logic
        private void PrepareWindowForUpdates()
        {
            txtAreaName.Text = _selectedArea.Name;
            this.Title = "Actualizar información del área";
            btnAddUpdate.Content = "Actualizar";
        }
        #endregion
    }
}