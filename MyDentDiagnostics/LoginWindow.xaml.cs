using System.Linq;
using System.Windows;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region Constructors
        public LoginWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Window event handlers
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            int userId;
            string userIdText = txtUserId.Text.Trim();
            string password = pbPassword.Password;

            if (int.TryParse(userIdText, out userId) == false || userId < 1)
            {
                MessageBox.Show("Introduzca un número de usuario válido", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Introduzca una contraseña", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MainWindow.UserLoggedIn = Controllers.BusinessController.Instance.FindBy<Model.User>(u => u.UserId == userId && u.Password == password).FirstOrDefault();

            if (MainWindow.UserLoggedIn == null)
                MessageBox.Show("Número de usuario y/o contraseña incorrectos", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                this.Close();
        }
        #endregion
    }
}
