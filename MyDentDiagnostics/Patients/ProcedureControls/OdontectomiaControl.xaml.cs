using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for OdontectomiaControl.xaml
    /// </summary>
    public partial class OdontectomiaControl : UserControl, IProcedureInfo
    {
        public OdontectomiaControl()
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
