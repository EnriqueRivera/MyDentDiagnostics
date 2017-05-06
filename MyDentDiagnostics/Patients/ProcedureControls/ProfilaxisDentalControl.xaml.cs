using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for ProfilaxisDentalControl.xaml
    /// </summary>
    public partial class ProfilaxisDentalControl : UserControl, IProcedureInfo
    {
        public ProfilaxisDentalControl()
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
