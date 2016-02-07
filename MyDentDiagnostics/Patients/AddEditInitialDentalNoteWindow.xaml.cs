using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace MyDentDiagnostics
{
	/// <summary>
	/// Interaction logic for AddEditInitialDentalNoteWindow.xaml
	/// </summary>
	public partial class AddEditInitialDentalNoteWindow : Window
	{
        #region Instance variables
        private Model.Patient _patientToUpdate;
        private bool _isUpdateDentalInitialNote;
        #endregion

        #region Constructors
        public AddEditInitialDentalNoteWindow(Model.Patient patientToUpdate)
		{
			this.InitializeComponent();

            _patientToUpdate = patientToUpdate;
            _isUpdateDentalInitialNote = patientToUpdate != null;

            if (_isUpdateDentalInitialNote)
                PrepareWindowForUpdates();
		}
        #endregion

        #region Window event handlers
        private void CheckBox_CheckedUncheked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.Tag != null && !string.IsNullOrEmpty(checkbox.Tag.ToString()))
            {
                string[] tagAttributes = checkbox.Tag.ToString().Split('|');
                if (tagAttributes.Length > 1)
                {
                    string[] controls = tagAttributes[1].Split(',');
                    foreach (var item in controls)
                    {
                        var controlToDisable = GetControlByName(item);
                        if (controlToDisable != null)
                        {
                            controlToDisable.IsEnabled = checkbox.IsChecked.Value;

                            if (!controlToDisable.IsEnabled)
                            {
                                if (controlToDisable is TextBox)
                                    (controlToDisable as TextBox).Text = string.Empty;
                                else if (controlToDisable is CheckBox)
                                    (controlToDisable as CheckBox).IsChecked = false;
                            }
                        }
                    }
                }
            }
        }

        private void btnAddUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Window's logic
        private Control GetControlByName(string name)
        {
            return gridInitialDentalNote.Children.OfType<Control>()
                        .Where(t => t.Name == name)
                        .FirstOrDefault();
        }

        private bool AddUpdateInitialDentalNoteAttributeValue(string fieldName, string fieldValue)
        {
            Model.InitialDentalNote attribute = Controllers.BusinessController.Instance
                                                    .FindBy<Model.InitialDentalNote>(c => c.PatientId == _patientToUpdate.PatientId && c.Name == fieldName)
                                                    .FirstOrDefault();

            if (attribute == null)
            {
                Model.InitialDentalNote newAttribute = new Model.InitialDentalNote()
                {
                    PatientId = _patientToUpdate.PatientId,
                    Name = fieldName,
                    Value = fieldValue
                };

                return Controllers.BusinessController.Instance.Add<Model.InitialDentalNote>(newAttribute);
            }
            else
            {
                attribute.Value = fieldValue;
                return Controllers.BusinessController.Instance.Update<Model.InitialDentalNote>(attribute);
            }

        }

        private void PrepareWindowForUpdates()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}