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

            AddEmptyItemToComboBoxes();

            _patientToUpdate = patientToUpdate;
            _isUpdateDentalInitialNote = patientToUpdate != null;

            if (_isUpdateDentalInitialNote)
                PrepareWindowForUpdates();
		}
        #endregion

        #region Window event handlers
        /// <summary>
        /// Tag = {0}|{1},{2},...
        /// {0} Attribute name
        /// {1}, {2},... Controls to enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckedUncheked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.Tag != null && !string.IsNullOrEmpty(checkbox.Tag.ToString()))
            {
                string[] tagAttributes = checkbox.Tag.ToString().Split('|');
                if (tagAttributes.Length > 1)
                {
                    string[] controls = tagAttributes[1].Split(',');
                    EnableDisableControls(controls, checkbox.IsChecked.Value);
                }
            }
        }

        /// <summary>
        /// Tag = {0}|{1},{2},...
        /// {0} Attribute name
        /// {1}, {2},... Controls to enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.Tag != null && !string.IsNullOrEmpty(comboBox.Tag.ToString()))
            {
                string[] tagAttributes = comboBox.Tag.ToString().Split('|');
                if (tagAttributes.Length > 2)
                {
                    int selectedIndex = comboBox.SelectedIndex;
                    int[] indexes = tagAttributes[1].Split(',').Select(i => Convert.ToInt32(i) + 1).ToArray();
                    string[] controls = tagAttributes[2].Split(',');
                    EnableDisableControls(controls, indexes.Contains(selectedIndex));
                }
            }
        }

        private void btnAddUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_isUpdateDentalInitialNote)
            {
                _patientToUpdate = new Model.Patient()
                {
                    FullName = txtFullName.Text.Trim(),
                    IsDCM = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                if (!Controllers.BusinessController.Instance.Add<Model.Patient>(_patientToUpdate))
                {
                    MessageBox.Show("Error al crear el paciente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (UpdateInitialDentalNote())
                this.Close();
            else
                MessageBox.Show("Error al guardar la nota inicial dental del paciente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Window's logic
        private void EnableDisableControls(string[] controls, bool enable)
        {
            foreach (var item in controls)
            {
                var controlToDisable = GetControlByName(item);
                if (controlToDisable != null)
                {
                    controlToDisable.IsEnabled = enable;

                    if (!controlToDisable.IsEnabled)
                    {
                        if (controlToDisable is TextBox)
                            (controlToDisable as TextBox).Text = string.Empty;
                        else if (controlToDisable is CheckBox)
                            (controlToDisable as CheckBox).IsChecked = false;
                        else if (controlToDisable is ComboBox)
                            (controlToDisable as ComboBox).SelectedIndex = 0;
                    }
                }
            }
        }

        private bool UpdateInitialDentalNote()
        {
            bool result = true;
            var controls = gridInitialDentalNote.Children.OfType<Control>()
                                .Where(t => t.Tag != null && !string.IsNullOrEmpty(t.Tag.ToString()))
                                .ToList();

            foreach (var item in controls)
            {
                if (item is TextBox)
                {
                    TextBox textField = item as TextBox;
                    string fieldName = textField.Tag.ToString();
                    string fieldValue = textField.Text.Trim();

                    result &= AddUpdateInitialDentalNoteAttributeValue(fieldName, fieldValue);
                }
                else if (item is CheckBox)
                {
                    CheckBox checkBox = item as CheckBox;
                    string fieldName = checkBox.Tag.ToString().Split('|')[0];
                    string fieldValue = checkBox.IsChecked.Value.ToString();

                    result &= AddUpdateInitialDentalNoteAttributeValue(fieldName, fieldValue);
                }
                else if (item is ComboBox)
                {
                    ComboBox comboBox = item as ComboBox;
                    string fieldName = comboBox.Tag.ToString().Split('|')[0];
                    string fieldValue = comboBox.SelectedValue.ToString();

                    result &= AddUpdateInitialDentalNoteAttributeValue(fieldName, fieldValue);
                }
                else if (item is RadioButton)
                {
                    RadioButton radioButton = item as RadioButton;
                    string groupName = radioButton.GroupName;
                    string tagName = radioButton.Tag.ToString();

                    if (radioButton.IsChecked.Value)
                        result &= AddUpdateInitialDentalNoteAttributeValue(groupName, tagName);
                }
            }

            return result;
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

        private Control GetControlByName(string name)
        {
            return gridInitialDentalNote.Children.OfType<Control>()
                        .Where(t => t.Name == name)
                        .FirstOrDefault();
        }

        private void AddEmptyItemToComboBoxes()
        {
            var controls = gridInitialDentalNote.Children.OfType<ComboBox>().ToList();

            foreach (var item in controls)
            {
                item.Items.Insert(0, string.Empty);
                item.SelectedIndex = 0;
            }
        }

        private void PrepareWindowForUpdates()
        {
            this.Title = "Actualizar Nota Inicial Dental";
            btnAddUpdate.Content = "Actualizar";

            txtFullName.Text = _patientToUpdate.FullName;
            LoadInitialDentalNoteInfo();
        }

        private void LoadInitialDentalNoteInfo()
        {
            var controls = gridInitialDentalNote.Children.OfType<Control>()
                                .Where(t => t.Tag != null && !string.IsNullOrEmpty(t.Tag.ToString()))
                                .ToList();

            foreach (var item in controls)
            {
                if (item is TextBox)
                {
                    TextBox textField = item as TextBox;
                    string fieldName = textField.Tag.ToString();
                    Model.InitialDentalNote attribute = GetInitialDentalNoteAttributeValue(fieldName);

                    textField.Text = attribute == null ? string.Empty : attribute.Value;
                }
                else if (item is CheckBox)
                {
                    CheckBox checkBox = item as CheckBox;
                    string fieldName = checkBox.Tag.ToString().Split('|')[0];
                    Model.InitialDentalNote attribute = GetInitialDentalNoteAttributeValue(fieldName);
                    bool isChecked;
                    bool.TryParse(attribute == null ? string.Empty : attribute.Value, out isChecked);

                    checkBox.IsChecked = isChecked;
                }
                else if (item is ComboBox)
                {
                    ComboBox comboBox = item as ComboBox;
                    string fieldName = comboBox.Tag.ToString().Split('|')[0];
                    Model.InitialDentalNote attribute = GetInitialDentalNoteAttributeValue(fieldName);

                    comboBox.SelectedValue = attribute == null ? string.Empty : attribute.Value;
                }
                else if (item is RadioButton)
                {
                    RadioButton radioButton = item as RadioButton;
                    string groupName = radioButton.GroupName;
                    string tagName = radioButton.Tag.ToString();
                    Model.InitialDentalNote attribute = GetInitialDentalNoteAttributeValue(groupName);

                    if (attribute != null)
                        radioButton.IsChecked = attribute.Value == tagName;
                    
                }
            }
        }

        private Model.InitialDentalNote GetInitialDentalNoteAttributeValue(string fieldName)
        {
            return Controllers.BusinessController.Instance.FindBy<Model.InitialDentalNote>(c => c.PatientId == _patientToUpdate.PatientId && c.Name == fieldName).FirstOrDefault();
        }
        #endregion
    }
}