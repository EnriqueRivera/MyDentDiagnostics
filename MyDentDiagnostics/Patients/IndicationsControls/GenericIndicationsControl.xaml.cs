using Controllers;
using System.Windows.Controls;

namespace MyDentDiagnostics.Patients.IndicationsControls
{
    /// <summary>
    /// Interaction logic for GenericIndicationsControl.xaml
    /// </summary>
    public partial class GenericIndicationsControl : UserControl, IProcedureInfo
    {
        public GenericIndicationsControl()
        {
            InitializeComponent();
        }

        public string GetProcedureContent()
        {
            return Utils.GetGridControlsContent(gdContent);
        }
    }
}
