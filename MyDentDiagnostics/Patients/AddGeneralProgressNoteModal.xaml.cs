using Controllers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for AddGeneralProgressNoteModal.xaml
    /// </summary>
    public partial class AddGeneralProgressNoteModal : Window
	{
        #region Instance variables
        private Model.Patient _patient;
        private string _noteType;
        #endregion

        #region Constructors
        public AddGeneralProgressNoteModal(Model.Patient patient, string noteType)
		{
			this.InitializeComponent();

            _noteType = noteType;
            _patient = patient;
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

            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Ingrese una descripción", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            Model.ProgressNote progressNoteToAdd = new Model.ProgressNote()
            {
                CreatedDate = DateTime.Now,
                PatientId = _patient.PatientId,
                IsDeleted = false,
                UserId = MainWindow.UserLoggedIn.UserId,
                Type = _noteType,
                TypeEnum = ProgressNoteType.GENERAL.ToString()
            };

            if (BusinessController.Instance.Add(progressNoteToAdd))
            {
                Dictionary<string, string> noteDetials = new Dictionary<string, string>();
                noteDetials.Add(Controllers.ProgressNoteDetail.GENERAL_VITAL_SIGNS.ToString(), vitalSigns);
                noteDetials.Add(Controllers.ProgressNoteDetail.GENERAL_DESCRIPTION.ToString(), description);

                if (!Utils.AddDetailsToProgressNote(progressNoteToAdd, noteDetials))
                {
                    MessageBox.Show("Error agregando los detalles a la nota de evolución", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo agregar la nota de evolución", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        #endregion
    }
}