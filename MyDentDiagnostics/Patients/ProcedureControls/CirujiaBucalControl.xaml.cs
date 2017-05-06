using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for CirujiaBucalControl.xaml
    /// </summary>
    public partial class CirujiaBucalControl : UserControl, IProcedureInfo
    {
        public CirujiaBucalControl()
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
