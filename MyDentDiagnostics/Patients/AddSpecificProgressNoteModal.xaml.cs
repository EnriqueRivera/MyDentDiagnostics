using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Controllers;
using Model;
using MyDentDiagnostics.Patients;
using MyDentDiagnostics.Patients.ProcedureControls;

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
        #endregion

        #region Constructors
        public AddSpecificProgressNoteModal(Patient patient, string noteTypeText, ProgressNoteType noteType)
        {
            InitializeComponent();

            this._patient = patient;
            this._noteTypeText = noteTypeText;
            this._noteType = noteType;

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
            string indications = txtIndications.Text.Trim();
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
                Type = _noteTypeText
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
                    break;
                case ProgressNoteType.DETARTRAJE_Y_CURETAJE:
                    break;
                case ProgressNoteType.OBTURACION_DENTAL:
                    break;
                case ProgressNoteType.ODONTECTOMIA:
                    break;
                case ProgressNoteType.PROFILAXIS_DENTAL:
                    break;
                case ProgressNoteType.PROTESIS_PARCIAL_FIJA:
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
            }
        }
        #endregion
    }
}
