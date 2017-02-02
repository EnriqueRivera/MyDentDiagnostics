using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for ChangePasswordModal.xaml
    /// </summary>
    public partial class ChangePasswordModal : Window
    {
        #region Constructors
        public ChangePasswordModal()
        {
            InitializeComponent();
        }
        #endregion

        #region Window event handlers
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = pbOldPassword.Password;
            string newPassword = pbNewPassword.Password;
            string repeatNewPassword = pbRepeatNewPassword.Password;

            if (string.IsNullOrEmpty(oldPassword))
            {
                MessageBox.Show("Introduzca la contraseña actual", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Introduzca la nueva contraseña", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(repeatNewPassword))
            {
                MessageBox.Show("Repita la nueva contraseña", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (oldPassword != MainWindow.UserLoggedIn.Password)
            {
                MessageBox.Show("La contraseña actual no es válida", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newPassword != repeatNewPassword)
            {
                MessageBox.Show("Las contraseñas nuevas no coinciden", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MainWindow.UserLoggedIn.Password = newPassword;

            if (Controllers.BusinessController.Instance.Update<Model.User>(MainWindow.UserLoggedIn))
            {
                MessageBox.Show("La contraseña ha sido cambiada", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al tratar de cambiar la contraseña", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
