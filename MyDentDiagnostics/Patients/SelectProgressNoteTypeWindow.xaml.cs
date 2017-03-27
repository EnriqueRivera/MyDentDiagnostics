using Controllers;
using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for SelectProgressNoteTypeWindow.xaml
    /// </summary>
    public partial class SelectProgressNoteTypeWindow : Window
    {
        #region Instance variables
        private Model.Patient _patient;
        #endregion

        #region Constructors
        public SelectProgressNoteTypeWindow(Model.Patient patient)
        {
            InitializeComponent();

            _patient = patient;
            FillProgressNoteTypes();
        }
        #endregion

        #region Window event handlers
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            Window windowToOpen = null;
            ComboBoxItem selectedItem = cbNoteType.SelectedItem as ComboBoxItem;
            ProgressNoteType selectedType = (ProgressNoteType)selectedItem.Value;
            string selectedTypeText = selectedItem.Text;
            
            switch (selectedType)
            {
                case ProgressNoteType.GENERAL:
                    windowToOpen = new AddGeneralProgressNoteModal(_patient, selectedTypeText);
                    break;
                //TODO: Add more notes that are not postoperatioras
                default:
                    windowToOpen = new AddSpecificProgressNoteModal(_patient, selectedTypeText, selectedType);
                    break;
            }

            if (windowToOpen != null)
            {
                this.Hide();
                windowToOpen.ShowDialog();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Window's logic
        private void FillProgressNoteTypes()
        {
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "General", Value = ProgressNoteType.GENERAL });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Biopsia de tejidos blandos", Value = ProgressNoteType.BIOPSIA_DE_TEJIDOS_BLANDOS });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Cirugía bucal", Value = ProgressNoteType.CIRUGIA_BUCAL });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Detartraje y curetaje", Value = ProgressNoteType.DETARTRAJE_Y_CURETAJE });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Obturación dental", Value = ProgressNoteType.OBTURACION_DENTAL });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Odontectomía", Value = ProgressNoteType.ODONTECTOMIA });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Profilaxis dental", Value = ProgressNoteType.PROFILAXIS_DENTAL });
            cbNoteType.Items.Add(new Controllers.ComboBoxItem() { Text = "Prótesis parcial fija", Value = ProgressNoteType.PROTESIS_PARCIAL_FIJA });

            cbNoteType.SelectedIndex = 0;
        }
        #endregion
    }
}
