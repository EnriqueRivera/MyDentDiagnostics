using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.IndicationsControls
{
    /// <summary>
    /// Interaction logic for ProfilaxisDentalIndicationsControl.xaml
    /// </summary>
    public partial class ProfilaxisDentalIndicationsControl : UserControl, IProcedureInfo
    {
        public ProfilaxisDentalIndicationsControl()
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
