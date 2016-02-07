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
	/// Interaction logic for ManagePatientsWindow.xaml
	/// </summary>
	public partial class ManagePatientsWindow : Window
    {
        #region Instance variables
        private Controllers.CustomViewModel<Model.Patient> _patients;
        private Controllers.CustomViewModel<Model.Patient> _patientsDcm;
        private bool _lastSearchAllPatients = true;
        #endregion

        #region Constructors
        public ManagePatientsWindow()
		{
			this.InitializeComponent();

            for (int i = tcPatients.Items.Count - 1; i >= 0; i--)
            {
                tcPatients.SelectedIndex = i;
                UpdateGrid();
            }  
		}
        #endregion

        #region Window event handlers
        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            _lastSearchAllPatients = false;
            UpdateGrid();
		}

		private void btnViewAll_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            _lastSearchAllPatients = true;
            UpdateGrid();
		}

		private void btnViewNotes_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.Patient selectedPatient = GetSelectedPatient();

            if (selectedPatient != null)
            {
                //TODO: View Patients notes
            }
		}

		private void btnExportToPdf_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.Patient selectedPatient = GetSelectedPatient();

            if (selectedPatient != null)
            {
                //TODO: Export Initial Dental Note to PDF
            }
		}

		private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            new AddEditInitialDentalNoteWindow(null).ShowDialog();
            UpdateGrid();
		}

		private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.Patient selectedPatient = GetSelectedPatient();

            if (selectedPatient != null)
            {
                new AddEditInitialDentalNoteWindow(selectedPatient);
                UpdateGrid();
            }
		}

		private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.Patient selectedPatient = GetSelectedPatient();

            if (selectedPatient != null 
                && MessageBox.Show
                                (string.Format("¿Está seguro(a) que desea eliminar el paciente '{0}'?",
                                        selectedPatient.FullName),
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                selectedPatient.IsDeleted = true;

                if (Controllers.BusinessController.Instance.Update<Model.Patient>(selectedPatient))
                    UpdateGrid();
                else
                    MessageBox.Show("No se pudo eliminar el paciente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Window's logic
        private Model.Patient GetSelectedPatient()
        {
            DataGrid dgAux = tcPatients.SelectedIndex == 0 ? dgPatients : dgPatientsDcm;

            Model.Patient selectedPatient = dgAux.SelectedItem == null ? null : dgAux.SelectedItem as Model.Patient;

            if (selectedPatient == null)
                MessageBox.Show("Seleccione un paciente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            return selectedPatient;
        }

        private void UpdateGrid()
        {
            if (_lastSearchAllPatients)
                UpdateGridAllPatients();
            else
                UpdateGridFilteredPatients();
        }

        private void UpdateGridFilteredPatients()
        {
            string searchTerm = txtSearchTerm.Text;

            if (tcPatients.SelectedIndex == 0)
            {
                _patients = new Controllers.CustomViewModel<Model.Patient>(p => !p.IsDCM && !p.IsDeleted && p.FullName.Contains(searchTerm), "PatientId", "asc");
                dgPatients.DataContext = _patients;
            }
            else
            {
                _patientsDcm = new Controllers.CustomViewModel<Model.Patient>(p => p.IsDCM && !p.IsDeleted && p.FullName.Contains(searchTerm), "PatientId", "asc");
                dgPatientsDcm.DataContext = _patientsDcm;
            }
        }

        private void UpdateGridAllPatients()
        {
            if (tcPatients.SelectedIndex == 0)
            {
                _patients = new Controllers.CustomViewModel<Model.Patient>(p => !p.IsDCM && !p.IsDeleted, "PatientId", "asc");
                dgPatients.DataContext = _patients;
            }
            else
            {
                _patientsDcm = new Controllers.CustomViewModel<Model.Patient>(p => p.IsDCM && !p.IsDeleted, "PatientId", "asc");
                dgPatientsDcm.DataContext = _patientsDcm;
            }
        }
        #endregion
    }
}