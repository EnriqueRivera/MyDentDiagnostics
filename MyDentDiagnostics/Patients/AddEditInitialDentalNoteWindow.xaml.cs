using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Controllers;
using System.Diagnostics;

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

            FillTreatments();
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

        private void cbDiagnostics_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FillDiagnosticInfo((cbDiagnostics.SelectedItem as Controllers.ComboBoxItem).Value as Model.Diagnostic);
        }

        private void btnRemoveDiagnostic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var items = lstDiagnostics.SelectedItems.OfType<string>().ToList();
            foreach (var item in items)
            {
                lstDiagnostics.Items.Remove(item);
            }
        }

        private void btnAddDiagnostic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedDiagnostic = (cbDiagnostics.SelectedItem as Controllers.ComboBoxItem).Value as Model.Diagnostic;

            if (selectedDiagnostic != null && !lstDiagnostics.Items.Contains(selectedDiagnostic.Name))
            {
                lstDiagnostics.Items.Add(selectedDiagnostic.Name);
            }
        }

        private void diagnosticImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Image imageControl = sender as Image;

                if (imageControl.ToolTip != null && string.IsNullOrEmpty(imageControl.ToolTip.ToString()) == false)
                    Process.Start(imageControl.ToolTip.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la imagen\n\nDetalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Después de guardar una nota inicial dental no será posible editarla o eliminarla."
                                + "\n¿Seguro(a) que desea guardar esta nota inicial dental?",
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                SaveInitialDentalNote();
            }
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Window's logic
        private void SaveInitialDentalNote()
        {
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Indique el nombre del paciente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_isUpdateDentalInitialNote)
            {
                _patientToUpdate = new Model.Patient()
                {
                    FullName = txtFullName.Text.Trim(),
                    IsDCM = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    UserId = MainWindow.UserLoggedIn.UserId
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
                else if (item is ListBox)
                {
                    ListBox listBox = item as ListBox;
                    string fieldName = listBox.Tag.ToString();
                    string fieldValue = string.Join("|", listBox.Items.OfType<string>().ToList());

                    result &= AddUpdateInitialDentalNoteAttributeValue(fieldName, fieldValue);
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
            var controls = gridInitialDentalNote.Children.OfType<ComboBox>()
                            .Where(t => t.Tag != null && !string.IsNullOrEmpty(t.Tag.ToString()))
                            .ToList();

            foreach (var item in controls)
            {
                item.Items.Insert(0, string.Empty);
                item.SelectedIndex = 0;
            }
        }

        private void FillTreatments()
        {
            List<Model.Diagnostic> diagnostics = BusinessController.Instance.FindBy<Model.Diagnostic>(t => t.IsDeleted == false)
                                                .OrderBy(t => t.Name)
                                                .ToList();

            cbDiagnostics.Items.Add(new Controllers.ComboBoxItem() { Text = string.Empty, Value = null });

            foreach (Model.Diagnostic diagnostic in diagnostics)
            {
                cbDiagnostics.Items.Add(new Controllers.ComboBoxItem() { Text = diagnostic.Name, Value = diagnostic });
            }
        }

        private void FillDiagnosticInfo(Model.Diagnostic diagnostic)
        {
            if (diagnostic == null)
            {
                gridDiagnostic.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                gridDiagnostic.Visibility = System.Windows.Visibility.Visible;

                txtDiagnosticName.Content = diagnostic.Name;
                txtDiagnosticDescription.Text = diagnostic.Description;
                MainWindow.SetImage(diagnostic.PicturePath1, diagnosticImage1);
                MainWindow.SetImage(diagnostic.PicturePath2, diagnosticImage2);
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
                else if (item is ListBox)
                {
                    ListBox listBox = item as ListBox;
                    string fieldName = listBox.Tag.ToString();
                    Model.InitialDentalNote attribute = GetInitialDentalNoteAttributeValue(fieldName);

                    if (attribute != null)
                    {
                        var items = attribute.Value.Split('|');
                        for (int i = 0; i < items.Length; i++)
                        {
                            listBox.Items.Add(items[i]);
                        }
                    }
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