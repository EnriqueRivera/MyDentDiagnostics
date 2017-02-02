using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for ManageAreasWindow.xaml
    /// </summary>
    public partial class ManageAreasWindow : Window
    {
        #region Instance variables
        private Controllers.CustomViewModel<Model.Area> _areasViewModel;
        private bool _lastSearchViewAll = true;
        #endregion

        #region Constructors
        public ManageAreasWindow()
		{
			this.InitializeComponent();

            UpdateGrid();
		}
        #endregion

        #region Window event handlers
        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            _lastSearchViewAll = false;
            UpdateGrid();
		}

        private void btnViewAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _lastSearchViewAll = true;
            UpdateGrid();
        }

        private void btnViewDiagnostics_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Area selectedArea = dgAreas.SelectedItem == null ? null : dgAreas.SelectedItem as Model.Area;

            if (selectedArea == null)
                MessageBox.Show("Seleccione un área", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                new ManageDiagnosticsWindow(selectedArea).ShowDialog();
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new AddEditAreaModal(null).ShowDialog();
            UpdateGrid();
        }

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Area selectedArea = dgAreas.SelectedItem == null ? null : dgAreas.SelectedItem as Model.Area;

            if (selectedArea == null)
                MessageBox.Show("Seleccione un área", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                new AddEditAreaModal(selectedArea).ShowDialog();
                UpdateGrid();
            }
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Area selectedArea = dgAreas.SelectedItem == null ? null : dgAreas.SelectedItem as Model.Area;

            if (selectedArea == null)
                MessageBox.Show("Seleccione un área", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (MessageBox.Show
                                (string.Format("¿Está seguro(a) que desea eliminar el área '{0}'?",
                                        selectedArea.Name),
                                    "Advertencia",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes)
            {
                selectedArea.IsDeleted = true;

                if (Controllers.BusinessController.Instance.Update<Model.Area>(selectedArea))
                    UpdateGrid();
                else
                    MessageBox.Show("No se pudo eliminar el área", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            _areasViewModel = new Controllers.CustomViewModel<Model.Area>(a => a.IsDeleted == false && a.Name.Contains(searchTerm), "Name", "asc");
            dgAreas.DataContext = _areasViewModel;
        }

        private void UpdateGridAll()
        {
            _areasViewModel = new Controllers.CustomViewModel<Model.Area>(a => a.IsDeleted == false, "Name", "asc");
            dgAreas.DataContext = _areasViewModel;
        }
        #endregion
    }
}