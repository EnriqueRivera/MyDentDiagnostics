using Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

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
        private static BaseFont _baseFont = BaseFont.CreateFont("arialuni.ttf", BaseFont.CP1252, BaseFont.EMBEDDED, BaseFont.CACHED, MyDentDiagnostics.Properties.Resources.ARIALUNI, null);
        private static iTextSharp.text.Font _font = new iTextSharp.text.Font(_baseFont, 11, iTextSharp.text.Font.NORMAL);
        private static iTextSharp.text.Font _boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        private List<Model.InitialDentalNote> _initialDentalNote = new List<Model.InitialDentalNote>();
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
                new ManageProgressNotesWindow(selectedPatient).ShowDialog();
            }
		}

		private void btnExportToPdf_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Model.Patient selectedPatient = GetSelectedPatient();

            if (selectedPatient != null)
            {
                try
                {
                    // Configure save file dialog box
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = selectedPatient.FullName + "_" + DateTime.Now.ToString("dd-MMMM-yyyy"); // Default file name
                    dlg.DefaultExt = ".pdf"; // Default file extension
                    dlg.Filter = "Text documents (.pdf)|*.pdf"; // Filter files by extension

                    if (dlg.ShowDialog() == true)
                    {
                        ExportToPdf(dlg.FileName, selectedPatient);
                        MessageBox.Show("PDF generado", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo generar el PDF.\n\n Detalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                new AddEditInitialDentalNoteWindow(selectedPatient).ShowDialog();
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

        private void ExportToPdf(string path, Model.Patient selectedPatient)
        {
            _initialDentalNote = BusinessController.Instance.FindBy<Model.InitialDentalNote>(p => p.PatientId == selectedPatient.PatientId).ToList();

            iTextSharp.text.Paragraph pdfContent = GetNotaInicialDentalPdfContent(selectedPatient); 

            //Create the PDF Document
            using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var writer = PdfWriter.GetInstance(pdfDoc, fs))
                    {
                        pdfDoc.Open();

                        //byte[] pngByteArrayImage = MainWindow.ImageToByteArray("pack://application:,,,/MyDentApplication;component/Images/Budget_image_1.PNG");
                        //iTextSharp.text.Image pngImage = iTextSharp.text.Image.GetInstance(pngByteArrayImage);
                        //pngImage.ScalePercent(80f);
                        //pngImage.SetAbsolutePosition(10f, pdfDoc.PageSize.Height - 90f);
                        //pdfDoc.Add(pngImage);

                        pdfDoc.Add(pdfContent);

                        pdfDoc.Close();
                    }
                }
            }
        }

        #endregion

        #region Nota inicial dental
        private iTextSharp.text.Paragraph GetNotaInicialDentalPdfContent(Model.Patient selectedPatient)
        {
            string gender = string.Empty;
            var paragraph = new iTextSharp.text.Paragraph("");

            paragraph.Add(GetTitle());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetPatinetInfo(selectedPatient, out gender));
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetAntecedentesHeredofamiliares());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetAntecedentesPersonalesNoPatologicos());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetAntecedentesGinecoObstetricos(gender));
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetAntecedentesPersonalesPatologicos());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetAntecedentesPadecimientoActual());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetExploracionFisica());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetMusculosPalpacion());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetExploracionFisica2());
            paragraph.Add(GetFluorosis());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetMaloclusiones());
            paragraph.Add(GetRetencionesDentarias());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetTratamientos());
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(GetFirma());

            return paragraph;
        }

        private IElement GetFirma()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("\n_______________________\nFirma", _boldFont));
            paragraph.Alignment = Element.ALIGN_RIGHT;
            return paragraph;
        }

        private IElement GetTratamientos()
        {
            var paragraph = new iTextSharp.text.Paragraph();


            string tratamientos = string.Format("Se trata de paciente de la {0}, con diagnostico de {1}, " + 
                                                "requiere tratamiento de: {2}",
                                                GetNote("Decada de vida"),
                                                GetNote("Diagnosticos").Replace("|", ", "),
                                                GetNote("Requiere tratamiento de")
                                                );


            paragraph.Add(new Chunk(tratamientos, _font));

            return paragraph;
        }

        private IElement GetRetencionesDentarias()
        {
            var paragraph = new iTextSharp.text.Paragraph("");
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(new Chunk("CLASIFICACIÓN DE LAS RETENCIONES DENTARIAS", _boldFont));

            string observaciones = GetNote("Retenciones dentarias comentario");
            string retenciones = string.Format("{0}{1}{2}{3}",
                                                GetRetencion("OD18"),
                                                GetRetencion("OD28"),
                                                GetRetencion("OD38"),
                                                GetRetencion("OD48")
                                                );


            paragraph.Add(new Chunk(retenciones, _font));

            if (!string.IsNullOrEmpty(observaciones))
            {
                paragraph.Add(new Chunk("\nObservaciones: " + observaciones, _font));
            }

            return string.IsNullOrEmpty(observaciones) && string.IsNullOrEmpty(retenciones)
                    ? new iTextSharp.text.Paragraph()
                    : paragraph;
        }

        private object GetRetencion(string retencion)
        {
            if (Convert.ToBoolean(GetNote(retencion)))
            {
                return string.Format("\nRetención tipo {0} de {1} {2} {3} según Sanchez Torres, " +
                                     "{4} según Pell y Gregory, posición {5} según Winter.",
                                    GetNote(retencion + " Retencion"),
                                    retencion,
                                    GetNote(retencion + " Sanchez Posicion"),
                                    GetNote(retencion + " Sanchez Clase"),
                                    GetNote(retencion + " Pell"),
                                    GetNote(retencion + " Winter")
                                    );
            }

            return string.Empty;
        }

        private IElement GetMaloclusiones()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("CLASIFICACIÓN DE LAS MALOCLUSIONES\n", _boldFont));

            string maloclusion = string.Format("Maloclusión {0} de Angle, relación canina {1}.",
                                            GetNote("Clasificacion Angle"),
                                            GetNote("Relacion Canina")
                                            );

            paragraph.Add(new Chunk(maloclusion, _font));

            return paragraph;
        }

        private IElement GetFluorosis()
        {
            var paragraph = new iTextSharp.text.Paragraph("");
            paragraph.Add(new iTextSharp.text.Paragraph(" "));
            paragraph.Add(new Chunk("FLUOROSIS", _boldFont));

            string observaciones = GetNote("Fluorosis comentario");
            string escalaDean = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                                            Convert.ToBoolean(GetNote("Fluorosis normal")) ? ", TF0" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis cuestionable")) ? ", TF1" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis muy leve")) ? ", TF2" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis leve")) ? ", TF3" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis moderado")) ? ", TF4" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis severo 5")) ? ", TF5" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis severo 6")) ? ", TF6" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis severo 7")) ? ", TF7" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis severo 8")) ? ", TF8" : "",
                                            Convert.ToBoolean(GetNote("Fluorosis severo 9")) ? ", TF9" : ""
                                            );

            bool flourosisPresent = escalaDean.Length > 0;
            if (flourosisPresent)
            {
                escalaDean = escalaDean.Substring(1);
                paragraph.Add(new Chunk("\nFluorosis con" + escalaDean + " según escala de Dean.", _font));
            }

            if (!string.IsNullOrEmpty(observaciones))
            {
                paragraph.Add(new Chunk("\nObservaciones: " + observaciones, _font));
            }

            return string.IsNullOrEmpty(observaciones) && !flourosisPresent
                    ? new iTextSharp.text.Paragraph()
                    : paragraph;
        }

        private IElement GetExploracionFisica2()
        {
            var paragraph = new iTextSharp.text.Paragraph();

            string explorationInfo = string.Format("\nMapeo del dolor con alteración en {0} de lado derecho y en {1} de lado izquierdo. " +
                                                    "{2}" +
                                                    "\nPlaca dentobacteriana {3} en {4} cantidad, de distribución {5}; " +
                                                    "calculo dental {6} en {7} cantidad, de distribucion {8} " +
                                                    "y localización {9}, higiene bucal {10} con un índice " + 
                                                    "de Higiene Oral Simplificado de {11}." +
                                                    "\nDentición {12} {13}{14}. Órganos dentales con {15} {16},{17} {18}{19} {20}," +
                                                    " dientes con movilidad {21}, {22}{23} " +
                                                    "de intensidad {24}, {25} {26}, {27}{28} {29}{30}.",
                                                    GetNote("Mapeo alteracion derecho"),
                                                    GetNote("Mapeo alteracion izquierdo"),
                                                    GetExploracionIntraoral(),
                                                    GetNote("Placa dentobacteriana"),
                                                    GetNote("Placa dentobacteriana cantidad"),
                                                    GetNote("Placa dentobacteriana distribucion"),
                                                    GetNote("Calculo dental"),
                                                    GetNote("Calculo dental cantidad"),
                                                    GetNote("Calculo dental distribucion"),
                                                    GetNote("Calculo dental localizacion"),
                                                    GetNote("Higiene bucal"),
                                                    GetNote("Higiene bucal indice"),
                                                    GetNote("Denticion temporal"),
                                                    GetNote("Denticion completa"),
                                                    GetNote("Denticion completa comentario", " a expensas de: "),
                                                    GetNote("Organos dentales"),
                                                    GetNote("Organos dentales comentario"),
                                                    GetPosicionDental(),
                                                    GetNote("Denticion posicion alteracion"),
                                                    GetNote("Denticion posicion alteracion comentario", " "),
                                                    GetNote("Denticion posicion alteracion color"),
                                                    GetNote("Dientes movilidad grado"),
                                                    GetNote("Denticion movilidad dolor"),
                                                    GetNote("Denticion movilidad dolor comentario", " "),
                                                    GetNote("Dolor dientes intensidad"),
                                                    GetNote("Dolor dientes palpacion"),
                                                    GetNote("Dolor dientes palpacion comentario"),
                                                    GetNote("Dolor dientes restauraciones"),
                                                    GetNote("Dolor Dientes Restauraciones Comentario", " "),
                                                    GetNote("Dolor dientes caries"),
                                                    GetNote("Dolor Dientes Caries Comentario", " ")
                                                    );

            paragraph.Add(new Chunk(explorationInfo, _font));

            return paragraph;
        }

        private object GetPosicionDental()
        {
            string posicion = string.Format("{0}{1}{2}{3}",
                                            Convert.ToBoolean(GetNote("Posicion apinamiento")) ? ", apiñamiento" : "",
                                            Convert.ToBoolean(GetNote("Posicion diastemas")) ? ", diastemas" : "",
                                            Convert.ToBoolean(GetNote("Posicion mesializacion")) ? ", mesializacion" : "",
                                            Convert.ToBoolean(GetNote("Posicion distalizacion")) ? ", distalizacion" : ""
                                            );
            if (posicion.Length > 0)
            {
                posicion = posicion.Substring(1);
                return " posición" + posicion;
            }

            return string.Empty;
        }

        private string GetExploracionIntraoral()
        {
            string exploracion = string.Format("{0}{1}{2}{3}{4}{5}",
                                            GetNote("Exploracion lengua", ", lengua "),
                                            GetNote("Paladar duro", ", paladar duro "),
                                            GetNote("Paladar blando", ", paladar blando "),
                                            GetNote("Vestibulos", ", vestíbulos "),
                                            GetNote("Piso de boca", ", piso de boca "),
                                            GetNote("Glandulas salivales", ", glándulas salivales ")
                                            );
            if (exploracion.Length > 0)
            {
                exploracion = exploracion.Substring(1);
                return "A la exploración intraoral se observa" + exploracion + ".";
            }

            return string.Empty;
        }

        private IElement GetMusculosPalpacion()
        {
            var musculos = new iTextSharp.text.Paragraph("");

            musculos.Add(GetMusculoPalpacion("Masetero superficial"));
            musculos.Add(GetMusculoPalpacion("Masetero profundo"));
            musculos.Add(GetMusculoPalpacion("Temporal fibras anteriores"));
            musculos.Add(GetMusculoPalpacion("Temporal fibras medias"));
            musculos.Add(GetMusculoPalpacion("Temporal fibras posteriores"));
            musculos.Add(GetMusculoPalpacion("Esternocleidomastoideo superficial"));
            musculos.Add(GetMusculoPalpacion("Esternocleidomastoideo profundo"));
            musculos.Add(GetMusculoPalpacion("Esternocleidomastoideo trapecio"));
            musculos.Add(GetMusculoPalpacion("Escaleno anterior"));
            musculos.Add(GetMusculoPalpacion("Escaleno medio"));
            musculos.Add(GetMusculoPalpacion("Escaleno posterior"));

            string header = string.Format("Músculos {0}{1}",
                                            GetNote("Musculos palpacion"),
                                            musculos.ToString() == "" ? "" : " en ");

            var paragraph = new iTextSharp.text.Paragraph(new Chunk(header, _font));
            paragraph.Add(musculos);

            return paragraph;
        }

        private iTextSharp.text.Paragraph GetMusculoPalpacion(string nombreMusculo)
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("", _font));
            
            string izquierdo = GetNote(nombreMusculo + " izquierdo");
            string derecho = GetNote(nombreMusculo + " derecho");

            izquierdo = (string.IsNullOrEmpty(izquierdo)) ? "" : "\nIzquierdo: " + izquierdo;
            derecho = (string.IsNullOrEmpty(derecho)) ? "" : "\nDerecho: " + derecho;

            if (!string.IsNullOrEmpty(izquierdo) || !string.IsNullOrEmpty(derecho))
            {
                paragraph.Add(new Chunk("\n" + nombreMusculo, _boldFont));
                paragraph.Add(new Chunk(string.Format("{0}{1}", izquierdo, derecho), _font));
            }

            return paragraph;
        }

        private IElement GetExploracionFisica()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("EXPLORACIÓN FÍSICA", _boldFont));

            string explorationInfo = string.Format("\nA la exploración extraoral, se observa cráneo {0}, suturas craneales {1}{2} cráneo {3}{4}. " +
                                                   "Rostro {5}{6}, cabello color {7}{8}, consistencia {9}, " +
                                                   "cantidad {10}{11}, carácter {12}{13}, implantación {14}, " +
                                                   "pabellones auriculares {15} de tamaño {16} con {17}{18}. Ojos {19}{20}, " +
                                                   "pupilas de tamaño {21} y forma {22}, reflejo fotomotor {23} a estímulos luminosos, " +
                                                   "reflejo consensual {24}, nariz {25} y {26}{27}, labios {28} {29}{30} {31}, " +
                                                   "de grosor {32} {33} y función {34}{35} perfil facial {36}. Cuello {37}{38}, " +
                                                   "movimientos de flexión {39}{40} de configuración {41} {42}{43}. " +
                                                   "Tráquea en posición {44}, ganglios linfáticos {45}, con movilidad {46}{47} {48} de consistencia {49}, " +
                                                   "de localización {50}. Pares craneales con {51}. \nArticulación temporomandibular {52}{53}, " +
                                                   "con {54}{55}, {56}{57}, cóndilo derecho {58}, cóndilo izquierdo {59}, " +
                                                   "lateralidad derecha con {60}, lateralidad izquierda con {61}, apertura bucal {62} de {63}.",
                                                    GetNote("Observa Craneo"),
                                                    GetNote("Sutura craneal"),
                                                    GetNote("Sutura craneal comentario", " "),
                                                    GetNote("Craneo"),
                                                    GetNote("Craneo comentario", " "),
                                                    GetNote("Rostro"),
                                                    GetNote("Rostro comentario", " a expensas de: "),
                                                    GetNote("Cabello color", "otros", true),
                                                    GetNote("Color cabello comentario", " "),
                                                    GetNote("Cabello consistencia"),
                                                    GetNote("Cabello cantidad", "otros", true),
                                                    GetNote("Cabello cantidad comentario", " "),
                                                    GetNote("Cabello caracter", "otros", true),
                                                    GetNote("Cabello caracter comentario", " "),
                                                    GetNote("Implantacion"),
                                                    GetNote("Pabellones auriculares"),
                                                    GetNote("Pabellones tamaño"),
                                                    GetNote("Pabellones secrecion"),
                                                    GetNote("Cabello caracter comentario", " "),
                                                    GetNote("Ojos"),
                                                    GetNote("Ojos comentario", " a expensas de: "),
                                                    GetNote("Pupilas tamaño"),
                                                    GetNote("Pupilas forma"),
                                                    GetNote("Pupilas reflejo"),
                                                    GetNote("Reflejo consensual"),
                                                    GetNote("Nariz posicion"),
                                                    GetNote("Nariz simetrica"),
                                                    GetNote("Nariz simetrica comentario", " a expensas de: "),
                                                    GetNote("Labios"),
                                                    GetNote("Labios simetricos"),
                                                    GetNote("Labios simetricos comentario", " a expensas de: "),
                                                    GetNote("Labios tamaño"),
                                                    GetNote("Labios grosor"),
                                                    GetNote("Labios grosor 2"),
                                                    GetNote("Labios funcion"),
                                                    GetNote("Labios funcion comentario", " "),
                                                    GetNote("Perfil facial"),
                                                    GetNote("Cuello"),
                                                    GetNote("Cuello comentario", " a expensas de: "),
                                                    GetNote("Cuello movimiento"),
                                                    GetNote("Cuello flexion", " a expensas de: "),
                                                    GetNote("Cuello configuracion"),
                                                    GetNote("Cuello lesiones"),
                                                    GetNote("Cuello lesiones comentario", " "),
                                                    GetNote("Traquea posicion"),
                                                    GetNote("Ganglios linfaticos"),
                                                    GetNote("Ganglios movilidad"),
                                                    GetNote("Ganglios movilidad comentario", " "),
                                                    GetNote("Ganglios movilidad 2"),
                                                    GetNote("Ganglios consistencia"),
                                                    GetNote("Ganglios localizacion"),
                                                    GetNote("Pares craneales"),
                                                    GetNote("Articulacion temporomandibular"),
                                                    GetNote("Articulacion temporomandibular comentario", " "),
                                                    GetNote("Articulacion temporomandibular articular"),
                                                    GetNote("Articulacion temporomandibular articular comentario", " "),
                                                    GetNote("Articulacion temporomandibular masticacion"),
                                                    GetNote("Articulacion temporomandibular masticacion comentario", " "),
                                                    GetNote("Condilo derecho"),
                                                    GetNote("Condilo izquierdo"),
                                                    GetNote("Lateralidad derecha"),
                                                    GetNote("Lateralidad izquierda"),
                                                    GetNote("Apertura bucal"),
                                                    GetNote("Apertura bucal de")
                                                    );

            paragraph.Add(new Chunk(explorationInfo, _font));

            return paragraph;
        }

        private IElement GetAntecedentesPadecimientoActual()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("PADECIMIENTO ACTUAL:", _boldFont));

            paragraph.Add(new Chunk("\n" + GetNote("Padecimiento actual"), _font));

            return paragraph;
        }

        private IElement GetAntecedentesPersonalesPatologicos()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("ANTECEDENTES PERSONALES PATOLÓGICOS:", _boldFont));

            paragraph.Add(new Chunk("\n" + GetNote("Antecedentes personales patológicos"), _font));

            return paragraph;
        }

        private IElement GetAntecedentesGinecoObstetricos(string gender)
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("ANTECEDENTES GINECO-OBSTÉTRICOS", _boldFont));

            if (gender == "femenino")
            {
                string womanInfo = string.Format("\nPrimera menarca {0}" +
                                                 "\nMenstruación con duración de {1}, " +
                                                 "frecuencia {2}, FUR (3) {4}",
                                                GetNote("Primera menarca"),
                                                GetNote("Duracion de menstruación"),
                                                GetNote("Frecuencia menstruacion"),
                                                GetNote("fecha de última regla"),
                                                GetNote("FUR")
                                                );

                paragraph.Add(new Chunk(womanInfo, _font));
            }

            string sexualLife = string.Format("\nVida sexual {0}, {1} embarazos, {2} gestaciones y {3} abortos." +
                                              "\nMétodos anticonceptivos {4}",
                                                GetNote("Vida sexual"),
                                                GetNote("Embarazos"),
                                                GetNote("Gestaciones"),
                                                GetNote("Abortos"),
                                                GetNote("Metodo anticonceptivo")
                                                );

            paragraph.Add(new Chunk(sexualLife, _font));

            string observations = string.Format("\nObservaciones: \n{0}",
                                                GetNote("Gineco-Obstétrico Obervaciones")
                                                );

            paragraph.Add(new Chunk(observations, _font));

            return paragraph;
        }

        private IElement GetAntecedentesPersonalesNoPatologicos()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("ANTECEDENTES PERSONALES NO PATOLÓGICOS", _boldFont));

            string patientInfo = string.Format("\nOriginario de {0} residente en {1} desde hace {2} años, " +
                                                "actualmente {3}, vive con su {4} escolaridad {5}, vivienda {6} {7}.",
                                                GetNote("Originario de"),
                                                GetNote("Residente en"),
                                                GetNote("Residente hace"),
                                                GetNote("Estado civil"),
                                                GetNote("Vive con"),
                                                GetNote("Escolaridad"),
                                                GetNote("Vivienda (1)"),
                                                GetNote("Vivienda (2)")
                                                );

            paragraph.Add(new Chunk(patientInfo, _font));

            paragraph.Add(new Chunk(GetAntecedentePersonalNoPatologico("\n-Alcoholismo: ", "APNP_Alcoholismo_SiNo", "APNP_Alcoholismo_Observacion", false), _font));
            paragraph.Add(new Chunk(GetAntecedentePersonalNoPatologico("\n-Tabaquismo: ", "APNP_Tabaquismo_SiNo", "APNP_Tabaquismo_Observacion", false), _font));
            paragraph.Add(new Chunk(GetAntecedentePersonalNoPatologico("\n-Toxicomanías: ", "APNP_Toxicomanias_SiNo", "APNP_Toxicomanias_Observacion", false), _font));
            paragraph.Add(new Chunk(GetAntecedentePersonalNoPatologico("\n-Hospitalizaciones o cirugías: ", "APNP_Cirugias_SiNo", "APNP_Cirugias_Observacion", false), _font));

            return paragraph;
        }

        private string GetAntecedentePersonalNoPatologico(string antecedente, string yesNoName, string parteDeName, bool heredofamiliar)
        {
            return Convert.ToBoolean(GetNote(yesNoName))
                        ? antecedente + GetNote(parteDeName)
                        : antecedente + "Negado";
        }

        private IElement GetAntecedentesHeredofamiliares()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("ANTECEDENTES HEREDOFAMILIARES", _boldFont));
            paragraph.Add(new Chunk(GetAntecedenteHeredofamiliar("\n-Hipertensión arterial:", "AH_HipertensionArterial_SiNo", "AH_HipertensionArterial_PorParteDe", "AH_HipertensionArterial_Finado"), _font));
            paragraph.Add(new Chunk(GetAntecedenteHeredofamiliar("\n-Diabetes Mellitus:", "AH_DiabetesMellitus_SiNo", "AH_DiabetesMellitus_PorParteDe", "AH_DiabetesMellitus_Finado"), _font));
            paragraph.Add(new Chunk(GetAntecedenteHeredofamiliar("\n-Cáncer:", "AH_Cancer_SiNo", "AH_Cancer_PorParteDe", "AH_Cancer_Finado"), _font));
            paragraph.Add(new Chunk(GetAntecedenteHeredofamiliar("\n-Discracias sanguíneas:", "AH_DiscranciasSanguineas_SiNo", "AH_DiscranciasSanguineas_PorParteDe", "AH_DiscranciasSanguineas_Finado"), _font));
            paragraph.Add(new Chunk(GetAntecedenteHeredofamiliar("\n-Enfermedades cardiacas:", "AH_Cardiacas_SiNo", "AH_Cardiacas_PorParteDe", "AH_Cardiacas_Finado"), _font));

            string otroPadecimiento = GetNote("AH_OtroPadecimiento_Padecimiento");
            if (!string.IsNullOrEmpty(otroPadecimiento))
            {
                paragraph.Add(new Chunk(GetAntecedenteHeredofamiliar("\n-" + otroPadecimiento + ": ", "AH_OtroPadecimiento_SiNo", "AH_OtroPadecimiento_PorParteDe", "AH_OtroPadecimiento_Finado"), _font));
            }

            return paragraph;
        }

        private string GetAntecedenteHeredofamiliar(string antecedente, string yesNoName, string parteDeName, string finadoName)
        {
            if (Convert.ToBoolean(GetNote(yesNoName)))
            {
                return string.Format("{0} Por parte de {1}{2}", 
                                    antecedente, 
                                    GetNote(parteDeName),
                                    (Convert.ToBoolean(GetNote(finadoName)) ? " (Finado)" : "")
                                    );
            }

            return antecedente + " No";
        }

        private IElement GetPatinetInfo(Model.Patient selectedPatient, out string gender)
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("NOMBRE DEL PACIENTE: ", _boldFont));
            paragraph.Add(new Chunk(selectedPatient.FullName, _font));
            gender = GetNote("Genero");

            string patientInfo = string.Format("\nPaciente {0} de {1} años de edad, acude a consulta por {2}",
                                                gender,
                                                GetNote("Edad"),
                                                GetNote("Motivo de la consulta")
                                                );

            paragraph.Add(new Chunk(patientInfo, _font));
            return paragraph;
        }

        private IElement GetTitle()
        {
            var paragraph = new iTextSharp.text.Paragraph(new Chunk("NOTA INICIAL DENTAL", _boldFont));
            paragraph.Alignment = Element.ALIGN_CENTER;
            return paragraph;
        }

        private string GetNote(string noteName, string beforeNote, bool hideWithBeforeNote = false)
        {
            string noteValue = GetNote(noteName);

            if (hideWithBeforeNote)
            {
                if (noteValue == beforeNote)
                {
                    return string.Empty;
                }

                return noteValue;
            }

            return string.IsNullOrEmpty(noteValue) ? string.Empty : (beforeNote + noteValue);
        }

        private string GetNote(string note)
        {
            var itemToRemove = _initialDentalNote.FirstOrDefault(r => r.Name == note);

            if (itemToRemove != null)
            {
                _initialDentalNote.Remove(itemToRemove);
                return itemToRemove.Value;
            }

            return string.Empty;
        }

        #endregion
    }
}