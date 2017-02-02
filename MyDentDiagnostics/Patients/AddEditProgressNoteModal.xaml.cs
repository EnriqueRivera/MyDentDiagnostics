using System;
using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for AddEditProgressNoteModal.xaml
    /// </summary>
    public partial class AddEditProgressNoteModal : Window
	{
        #region Instance variables
        private Model.ProgressNote _selectedProgressNote;
        private bool _isUpdateProgressNote;
        private Model.Patient _patient;
        #endregion

        #region Constructors
        public AddEditProgressNoteModal(Model.ProgressNote selectedProgressNote, Model.Patient patient)
		{
			this.InitializeComponent();

            _patient = patient;
            _selectedProgressNote = selectedProgressNote;
            _isUpdateProgressNote = selectedProgressNote != null;

            if (_isUpdateProgressNote)
                PrepareWindowForUpdates();
		}
        #endregion

        #region Window event handlers
        private void btnAddUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            if (MessageBox.Show("Después de guardar una nota de evolución no será posible editarla o eliminarla."
                                + "\n¿Seguro(a) que desea guardar esta nota de evolución?",
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                SaveProgressNote();
            }
		}

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            this.Close();
        }
        #endregion

        #region Window's logic
        private void SaveProgressNote()
        {
            string vitalSigns = txtVitalSigns.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(vitalSigns))
            {
                MessageBox.Show("Ingrese los signos vitales", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Ingrese una descripción", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (_isUpdateProgressNote)
            {
                _selectedProgressNote.VitalSigns = vitalSigns;
                _selectedProgressNote.Description = description;

                if (Controllers.BusinessController.Instance.Update<Model.ProgressNote>(_selectedProgressNote))
                    this.Close();
                else
                    MessageBox.Show("No se pudo actualizar la nota de evolución", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Model.ProgressNote progressNoteToAdd = new Model.ProgressNote()
                {
                    CreatedDate = DateTime.Now,
                    VitalSigns = vitalSigns,
                    Description = description,
                    PatientId = _patient.PatientId,
                    IsDeleted = false,
                    UserId = MainWindow.UserLoggedIn.UserId
                };

                if (Controllers.BusinessController.Instance.Add<Model.ProgressNote>(progressNoteToAdd))
                    this.Close();
                else
                    MessageBox.Show("No se pudo agregar la nota de evolución", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrepareWindowForUpdates()
        {
            txtVitalSigns.Text = _selectedProgressNote.VitalSigns;
            txtDescription.Text = _selectedProgressNote.Description;
            this.Title = "Actualizar nota de evolución";
            btnAddUpdate.Content = "Actualizar";
        }
        #endregion
    }
}