using Controllers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDentDiagnostics.Patients.ProcedureControls
{
    /// <summary>
    /// Interaction logic for BiopsiaTejidosBlandosControl.xaml
    /// </summary>
    public partial class BiopsiaTejidosBlandosControl : UserControl, IProcedureInfo
    {
        public BiopsiaTejidosBlandosControl()
        {
            InitializeComponent();
            AddEmptyItemToComboBoxes();
        }

        private void AddEmptyItemToComboBoxes()
        {
            var controls = gdContent.Children.OfType<ComboBox>().ToList();

            foreach (var item in controls)
            {
                item.Items.Insert(0, string.Empty);
                item.SelectedIndex = 0;
            }
        }

        public string GetProcedureContent()
        {
            StringBuilder content = new StringBuilder();
            foreach (var item in gdContent.Children)
            {
                if (item is Label)
                    content.Append((item as Label).Content);
                else if (item is TextBox)
                    content.Append((item as TextBox).Text);
                else if (item is ComboBox)
                    content.Append((item as ComboBox).SelectedValue.ToString());
            }
            return content.ToString();
        }
    }
}
