using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for ProtesisParcialFijaControl.xaml
    /// </summary>
    public partial class ProtesisParcialFijaControl : UserControl, IProcedureInfo
    {
        public ProtesisParcialFijaControl()
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
