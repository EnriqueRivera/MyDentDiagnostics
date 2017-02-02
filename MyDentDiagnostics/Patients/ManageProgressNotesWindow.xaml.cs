using System;
using System.Collections.Generic;
using System.Windows;
using Model;
using System.Linq;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

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
                List<Model.ProgressNote> progressNotes = new List<ProgressNote>();
                progressNotes.Add(selectedProgressNote);
                ExportNotes(progressNotes);
            }
        }

        private void btnExportAllToPDF_Click(object sender, RoutedEventArgs e)
        {
            List<Model.ProgressNote> progressNotes = _progressNotesViewModel.ObservableData.ToList();

            if (progressNotes.Count > 0)
                ExportNotes(progressNotes);
            else
                MessageBox.Show("No hay notas de evolución para exportar", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Window's logic
        private void ExportNotes(List<ProgressNote> progressNotes)
        {
            progressNotes = progressNotes.OrderBy(n => n.ProgressNoteId).ToList();
            Model.Patient patient = progressNotes.Count > 0 ? progressNotes[0].Patient : null;

            try
            {
                // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = string.Format("{0}_NotaEvolucion_{1}", patient.FullName, DateTime.Now.ToString("dd-MMMM-yyyy")); // Default file name
                dlg.DefaultExt = ".pdf"; // Default file extension
                dlg.Filter = "Text documents (.pdf)|*.pdf"; // Filter files by extension

                if (dlg.ShowDialog() == true)
                {
                    ExportToPdf(dlg.FileName, patient, progressNotes);
                    MessageBox.Show("PDF generado", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo generar el PDF.\n\n Detalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToPdf(string path, Model.Patient patient, List<ProgressNote> progressNotes)
        {
            iTextSharp.text.Paragraph pdfContent = GetProgressNotePdfContent(patient, progressNotes);

            //Create the PDF Document
            using (Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 30f, 30f))
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var writer = PdfWriter.GetInstance(pdfDoc, fs))
                    {
                        pdfDoc.Open();
                        pdfDoc.Add(pdfContent);
                        pdfDoc.Close();
                    }
                }
            }
        }

        private iTextSharp.text.Paragraph GetProgressNotePdfContent(Model.Patient patient, List<ProgressNote> progressNotes)
        {
            var paragraph = new iTextSharp.text.Paragraph("");

            paragraph.Add(GetTitle());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetCurrentDate());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetPatinetInfo(patient));
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetProgressNotes(progressNotes));
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetFirma());

            return paragraph;
        }

        private IElement GetPatinetInfo(Patient patient)
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("Nombre del paciente: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(patient.FullName, MainWindow.Font));

            return paragraph;
        }

        private IElement GetFirma()
        {
            var paragraph = new iTextSharp.text.Paragraph();
            paragraph.Alignment = Element.ALIGN_RIGHT;
            paragraph.Add(new Chunk("_____________________________________\nNombre y firma del odontólogo tratante", MainWindow.BoldFont));

            return paragraph;
        }

        private IElement GetProgressNotes(List<ProgressNote> progressNotes)
        {
            var paragraph = new iTextSharp.text.Paragraph();

            foreach (var note in progressNotes)
            {
                paragraph.Add(new Chunk("Número de folio: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.ProgressNoteId.ToString(), MainWindow.Font));

                paragraph.Add("\n");

                paragraph.Add(new Chunk("Fecha y hora de creación: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.CreatedDate.ToString(), MainWindow.Font));

                paragraph.Add("\n");

                paragraph.Add(new Chunk("Signos vitales: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.VitalSigns, MainWindow.Font));

                paragraph.Add("\n");

                paragraph.Add(new Chunk("Descripción: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.Description, MainWindow.Font));

                paragraph.Add(new iTextSharp.text.Paragraph(" "));
            }

            return paragraph;
        }

        private IElement GetCurrentDate()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk(DateTime.Now.ToString("D"), MainWindow.Font));
            paragraph.Alignment = Element.ALIGN_RIGHT;
            return paragraph;
        }

        private IElement GetTitle()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("NOTAS DE EVOLUCIÓN", MainWindow.Font));
            paragraph.Alignment = Element.ALIGN_CENTER;
            return paragraph;
        }

        private void UpdateGrid()
        {
            _progressNotesViewModel = new Controllers.CustomViewModel<Model.ProgressNote>(a => a.PatientId == _patient.PatientId && !a.IsDeleted, "ProgressNoteId", "asc");
            dgProgressNotes.DataContext = _progressNotesViewModel;
        }
        #endregion      
    }
}