using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for DetratrajeCuretajeControl.xaml
    /// </summary>
    public partial class DetratrajeCuretajeControl : UserControl, IProcedureInfo
    {
        public DetratrajeCuretajeControl()
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
