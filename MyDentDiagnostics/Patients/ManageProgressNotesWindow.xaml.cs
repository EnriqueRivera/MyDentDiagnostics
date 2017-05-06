using System;
using System.Collections.Generic;
using System.Windows;
using Model;
using System.Linq;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using Controllers;

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
            new SelectProgressNoteTypeWindow(_patient).ShowDialog();
            UpdateGrid();
		}

		private void btnExportToPDF_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            List<Model.ProgressNote> selectedProgressNotes = dgProgressNotes.SelectedItems.Cast<Model.ProgressNote>().ToList();

            if (selectedProgressNotes.Count == 0)
                MessageBox.Show("Seleccione una nota de evolución", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                ExportNotes(selectedProgressNotes);
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
            Paragraph pdfContent = GetProgressNotePdfContent(patient, progressNotes);

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

        private Paragraph GetProgressNotePdfContent(Model.Patient patient, List<ProgressNote> progressNotes)
        {
            var paragraph = new Paragraph("");

            paragraph.Add(GetTitle());
            paragraph.Add(new Paragraph(" "));
            paragraph.Add(GetPatinetInfo(patient));
            paragraph.Add(new Paragraph(" "));
            paragraph.Add(GetProgressNotes(progressNotes));
            paragraph.Add(new Paragraph(" "));

            return paragraph;
        }

        private IElement GetPatinetInfo(Patient patient)
        {
            var paragraph = new Paragraph(new Chunk("Nombre del paciente: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(patient.FullName, MainWindow.Font));
            paragraph.Add(new Chunk("\nExpediente No. ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(patient.AssignedPatientId.ToString(), MainWindow.Font));

            return paragraph;
        }

        private IElement GetProgressNotes(List<ProgressNote> progressNotes)
        {
            //TODO: Return progress note content depending the type 

            var paragraph = new Paragraph();

            foreach (var note in progressNotes)
            {
                paragraph.Add(new Chunk("Número de folio: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.ProgressNoteId.ToString(), MainWindow.Font));

                paragraph.Add("\n");

                paragraph.Add(new Chunk("Fecha y hora de creación: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.CreatedDate.ToString(), MainWindow.Font));

                paragraph.Add("\n");

                paragraph.Add(new Chunk("Tipo de nota: ", MainWindow.BoldFont));
                paragraph.Add(new Chunk(note.Type, MainWindow.Font));

                paragraph.Add(GetProgressNoteDetails(note));

                paragraph.Add("\n");
                paragraph.Add("\n");

                var signatureParagraph = new Paragraph();
                signatureParagraph.Alignment = Element.ALIGN_RIGHT;
                signatureParagraph.Add(new Chunk("_____________________________________\nNombre y firma del odontólogo tratante", MainWindow.BoldFont));

                paragraph.Add(signatureParagraph);

                paragraph.Add(new Paragraph(" "));
            }

            return paragraph;
        }

        private Paragraph GetProgressNoteDetails(ProgressNote note)
        {
            var paragraph = new Paragraph();
            ProgressNoteType noteType;
            Enum.TryParse(note.TypeEnum, out noteType);

            switch (noteType)
            {
                case ProgressNoteType.GENERAL:
                    paragraph = GetProgressNoteDetailGeneral(note);
                    break;
                case ProgressNoteType.BIOPSIA_DE_TEJIDOS_BLANDOS:
                case ProgressNoteType.CIRUGIA_BUCAL:
                case ProgressNoteType.DETARTRAJE_Y_CURETAJE:
                case ProgressNoteType.OBTURACION_DENTAL:
                case ProgressNoteType.ODONTECTOMIA:
                case ProgressNoteType.PROFILAXIS_DENTAL:
                case ProgressNoteType.PROTESIS_PARCIAL_FIJA:
                    paragraph = GetProgressNoteDetailSpecific(note);
                    break;
                default:
                    break;
            }

            return paragraph;
        }

        private Paragraph GetProgressNoteDetailSpecific(ProgressNote note)
        {
            var paragraph = new Paragraph();
            string procedimiento = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_PROCEDIMIENTO);
            string indicaciones = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_INDICACIONES);
            string medicamento = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_MEDICAMENTO);
            string hallazgos = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_HALLAZGOS);
            string pronostico = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_PRONOSTICOS);
            string diagnostico = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_DIAGNOSTICO);
            string tratamientoProxCita = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.ESPECIFICA_TRATAMIENTO_PROX_CITA);

            paragraph.Add(new Chunk("Procedimiento realizado: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(procedimiento, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Se dan las siguientes indicaciones al paciente: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(indicaciones, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Medicamentos: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(medicamento, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Hallazgos: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(hallazgos, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Pronóstico: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(pronostico, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Diagnóstico: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(diagnostico, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Tratamiento a realizar en la próxima cita: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(tratamientoProxCita, MainWindow.Font));

            return paragraph;
        }

        private Paragraph GetProgressNoteDetailGeneral(ProgressNote note)
        {
            var paragraph = new Paragraph();
            string vitalSigns = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.GENERAL_VITAL_SIGNS);
            string description = Utils.GetNoteDetail(note, Controllers.ProgressNoteDetail.GENERAL_DESCRIPTION);

            paragraph.Add(new Chunk("Signos vitales: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(vitalSigns, MainWindow.Font));

            paragraph.Add("\n");

            paragraph.Add(new Chunk("Descripción: ", MainWindow.BoldFont));
            paragraph.Add(new Chunk(description, MainWindow.Font));

            return paragraph;
        }

        private IElement GetTitle()
        {
            var paragraph = new Paragraph(new Chunk("NOTAS DE EVOLUCIÓN", MainWindow.Font));
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