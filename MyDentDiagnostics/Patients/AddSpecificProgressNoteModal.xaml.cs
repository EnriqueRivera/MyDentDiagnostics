using System;
using System.Collections.Generic;
using System.Windows;
using Controllers;
using Model;
using MyDentDiagnostics.Patients.ProcedureControls;
using MyDentDiagnostics.Patients.IndicationsControls;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for AddSpecificProgressNoteModal.xaml
    /// </summary>
    public partial class AddSpecificProgressNoteModal : Window
    {
        #region Instance variables
        private ProgressNoteType _noteType;
        private string _noteTypeText;
        private Patient _patient;
        private IProcedureInfo _procedureControl = null;
        private IProcedureInfo _indicationsControl = new GenericIndicationsControl();
        #endregion

        #region Constructors
        public AddSpecificProgressNoteModal(Patient patient, string noteTypeText, ProgressNoteType noteType)
        {
            InitializeComponent();

            _patient = patient;
            _noteTypeText = noteTypeText;
            _noteType = noteType;

            AddProcedureTemplate();
            SetTitle();
        }
        #endregion

        #region Window event handlers
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Después de guardar una nota de evolución no será posible editarla o eliminarla."
                                + "\n¿Seguro(a) que desea guardar esta nota de evolución?",
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                SaveNote();
            }
        }

        #endregion

        #region Window's logic
        private void SaveNote()
        {
            string procedure = _procedureControl.GetProcedureContent();
            string indications = _indicationsControl.GetProcedureContent();
            string medicaments = txtMedicaments.Text.Trim();
            string foundOut = txtFoundOut.Text.Trim();
            string pronostics = txtPronostics.Text.Trim();
            string diagnostic = txtDiagnostic.Text.Trim();
            string nextTreatment = txtNextTreatment.Text.Trim();

            Model.ProgressNote progressNoteToAdd = new Model.ProgressNote()
            {
                CreatedDate = DateTime.Now,
                PatientId = _patient.PatientId,
                IsDeleted = false,
                UserId = MainWindow.UserLoggedIn.UserId,
                Type = _noteTypeText,
                TypeEnum = _noteType.ToString()
            };

            if (BusinessController.Instance.Add(progressNoteToAdd))
            {
                Dictionary<string, string> noteDetials = new Dictionary<string, string>();
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_PROCEDIMIENTO.ToString(), procedure);
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_INDICACIONES.ToString(), indications);
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_MEDICAMENTO.ToString(), medicaments);
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_HALLAZGOS.ToString(), foundOut);
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_PRONOSTICOS.ToString(), pronostics);
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_DIAGNOSTICO.ToString(), diagnostic);
                noteDetials.Add(Controllers.ProgressNoteDetail.ESPECIFICA_TRATAMIENTO_PROX_CITA.ToString(), nextTreatment);

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

        private void SetTitle()
        {
            string title = "Nota postoperatoria " + _noteTypeText;
            this.Title = title;
            lblTitle.Content = title;
        }

        private void AddProcedureTemplate()
        {
            switch (_noteType)
            {
                case ProgressNoteType.BIOPSIA_DE_TEJIDOS_BLANDOS:
                    _procedureControl = new BiopsiaTejidosBlandosControl();
                    break;
                case ProgressNoteType.CIRUGIA_BUCAL:
                    _procedureControl = new CirujiaBucalControl();
                    break;
                case ProgressNoteType.DETARTRAJE_Y_CURETAJE:
                    _procedureControl = new DetratrajeCuretajeControl();
                    break;
                case ProgressNoteType.OBTURACION_DENTAL:
                    _procedureControl = new ObturacionDentalControl();
                    break;
                case ProgressNoteType.ODONTECTOMIA:
                    _procedureControl = new OdontectomiaControl();
                    break;
                case ProgressNoteType.PROFILAXIS_DENTAL:
                    _procedureControl = new ProfilaxisDentalControl();
                    _indicationsControl = new ProfilaxisDentalIndicationsControl();
                    break;
                case ProgressNoteType.PROTESIS_PARCIAL_FIJA:
                    _procedureControl = new ProtesisParcialFijaControl();
                    break;
                default:
                    break;
            }

            if (_procedureControl == null)
            {
                MessageBox.Show("Error seleccionando la plantilla para el procedimiento realizado ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            else
            {
                grdProcedure.Children.Add(_procedureControl as UIElement);
                grdIndications.Children.Add(_indicationsControl as UIElement);
            }
        }
        #endregion
    }
}
