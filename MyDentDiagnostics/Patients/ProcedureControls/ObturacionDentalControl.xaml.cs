using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for ObturacionDentalControl.xaml
    /// </summary>
    public partial class ObturacionDentalControl : UserControl, IProcedureInfo
    {
        public ObturacionDentalControl()
        {
            InitializeComponent();
            Utils.AddEmptyItemToComboBoxes(gdContent);
        }

        public string GetProcedureContent()
        {
            return Utils.GetGridControlsContent(gdContent);
        }
    }
}
