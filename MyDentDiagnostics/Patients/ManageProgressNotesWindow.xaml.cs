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
	/// Interaction logic for ManageProgressNotesWindow.xaml
	/// </summary>
	public partial class ManageProgressNotesWindow : Window
	{
        #region Instance variables
        private Model.Patient _patient;
        private Controllers.CustomViewModel<Model.ProgressNote> _progressNotesViewModel;
        #endregion

        #region Constructors
        public ManageProgressNotesWindow(Model.Patient patient)
		{
			this.InitializeComponent();

            _patient = patient;
            UpdateGrid();
		}
        #endregion

        #region Window event handlers
        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.ProgressNote selectedProgressNote = dgProgressNotes.SelectedItem == null ? null : dgProgressNotes.SelectedItem as Model.ProgressNote;

            if (selectedProgressNote == null)
                MessageBox.Show("Seleccione una nota de evolución", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (MessageBox.Show
                                (string.Format("¿Está seguro(a) que desea eliminar la nota de evolución número '{0}'?",
                                        selectedProgressNote.ProgressNoteId),
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                selectedProgressNote.IsDeleted = true;

                if (Controllers.BusinessController.Instance.Update<Model.ProgressNote>(selectedProgressNote))
                    UpdateGrid();
                else
                    MessageBox.Show("No se pudo eliminar la nota de evolución", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
		}

		private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.ProgressNote selectedProgressNote = dgProgressNotes.SelectedItem == null ? null : dgProgressNotes.SelectedItem as Model.ProgressNote;

            if (selectedProgressNote == null)
                MessageBox.Show("Seleccione una nota de evolución", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                new AddEditProgressNoteModal(selectedProgressNote, _patient).ShowDialog();
                UpdateGrid();
            }
		}

		private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            new AddEditProgressNoteModal(null, _patient).ShowDialog();
            UpdateGrid();
		}

		private void btnExportToPDF_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.ProgressNote selectedProgressNote = dgProgressNotes.SelectedItem == null ? null : dgProgressNotes.SelectedItem as Model.ProgressNote;

            if (selectedProgressNote == null)
                MessageBox.Show("Seleccione una nota de evolución", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                // TODO: Export progress note to PDF
            }
        }
        #endregion

        #region Window's logic
        private void UpdateGrid()
        {
            _progressNotesViewModel = new Controllers.CustomViewModel<Model.ProgressNote>(a => a.PatientId == _patient.PatientId && !a.IsDeleted, "ProgressNoteId", "asc");
            dgProgressNotes.DataContext = _progressNotesViewModel;
        }
        #endregion
    }
}