using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for ManageDiagnosticsWindow.xaml
    /// </summary>
    public partial class ManageDiagnosticsWindow : Window
	{
        #region Instance variables
        private Controllers.CustomViewModel<Model.Diagnostic> _diagnosticsViewModel;
        private Model.Area _selectedArea;
        private bool _lastSearchViewAll = true;
        #endregion        
        
        #region Constructors
        public ManageDiagnosticsWindow(Model.Area selectedArea)
        {
            this.InitializeComponent();

            _selectedArea = selectedArea;
            lblAreaName.ToolTip = lblAreaName.Content = selectedArea.Name;
            UpdateGrid();
        }
        #endregion

        #region Window event handlers
        private void btnViewAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _lastSearchViewAll = true;
            UpdateGrid();
        }

        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _lastSearchViewAll = false;
            UpdateGrid();
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new AddEditDiagnosticModal(null, _selectedArea).ShowDialog();
            UpdateGrid();
        }

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Diagnostic selectedDiagnostic = dgDiagnostics.SelectedItem == null ? null : dgDiagnostics.SelectedItem as Model.Diagnostic;

            if (selectedDiagnostic == null)
                MessageBox.Show("Seleccione un diagnóstico", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                new AddEditDiagnosticModal(selectedDiagnostic, _selectedArea).ShowDialog();
                UpdateGrid();
            }
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Diagnostic selectedDiagnostic = dgDiagnostics.SelectedItem == null ? null : dgDiagnostics.SelectedItem as Model.Diagnostic;

            if (selectedDiagnostic == null)
                MessageBox.Show("Seleccione un diagnóstico", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (MessageBox.Show
                                (string.Format("¿Está seguro(a) que desea eliminar el diagnóstico '{0}'?",
                                        selectedDiagnostic.Name),
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                selectedDiagnostic.IsDeleted = true;

                if (Controllers.BusinessController.Instance.Update<Model.Diagnostic>(selectedDiagnostic))
                    UpdateGrid();
                else
                    MessageBox.Show("No se pudo eliminar el diagnóstico", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Window's logic
        private void UpdateGrid()
        {
            if (_lastSearchViewAll)
                UpdateGridAll();
            else
                UpdateGridFiltered();
        }

        private void UpdateGridFiltered()
        {
            string searchTerm = txtSearchTerm.Text;

            _diagnosticsViewModel = new Controllers.CustomViewModel<Model.Diagnostic>(d => d.IsDeleted == false && d.AreaId == _selectedArea.AreaId && d.Name.Contains(searchTerm), "Name", "asc");
            dgDiagnostics.DataContext = _diagnosticsViewModel;
        }

        private void UpdateGridAll()
        {
            _diagnosticsViewModel = new Controllers.CustomViewModel<Model.Diagnostic>(d => d.IsDeleted == false && d.AreaId == _selectedArea.AreaId, "Name", "asc");
            dgDiagnostics.DataContext = _diagnosticsViewModel;
        }
        #endregion
    }
}