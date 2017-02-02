using iTextSharp.text.pdf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Class variables
        public static Model.User UserLoggedIn { get; set; }
        #endregion

        #region Instance variables
        //Windows
        private ManageAreasWindow _manageAreasWindow;
        private ManagePatientsWindow _managePatientNotesWindow;

        private static BaseFont _baseFont = BaseFont.CreateFont("arialuni.ttf", BaseFont.CP1252, BaseFont.EMBEDDED, BaseFont.CACHED, MyDentDiagnostics.Properties.Resources.ARIALUNI, null);
        public static iTextSharp.text.Font Font = new iTextSharp.text.Font(_baseFont, 11, iTextSharp.text.Font.NORMAL);
        public static iTextSharp.text.Font BoldFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12);
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Logic for closing windows
        private void CloseAllWindows()
        {
            CloseWindow(_manageAreasWindow);
            CloseWindow(_managePatientNotesWindow);
        }

        private void CloseWindow(Window windowToClose)
        {
            if (windowToClose != null)
            {
                windowToClose.Close();
            }
        }
        #endregion

        #region Window event handlers
        void Window_Closed(object sender, EventArgs e)
        {
            if (sender is ManageAreasWindow)
            {
                _manageAreasWindow = null;
            }
            else if (sender is ManagePatientsWindow)
            {
                _managePatientNotesWindow = null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Controllers.BusinessController.Instance.CloseConnection();
        }

        private void btnManageAreas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsUserLoggedIn())
            {
                if (_manageAreasWindow == null)
                {
                    _manageAreasWindow = new ManageAreasWindow();
                    _manageAreasWindow.Closed += Window_Closed;
                }

                _manageAreasWindow.Show();
                _manageAreasWindow.WindowState = WindowState.Normal;
            }
        }

        private void btnManagePatientNotes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsUserLoggedIn())
            {
                if (_managePatientNotesWindow == null)
                {
                    _managePatientNotesWindow = new ManagePatientsWindow();
                    _managePatientNotesWindow.Closed += Window_Closed;
                }

                _managePatientNotesWindow.Show();
                _managePatientNotesWindow.WindowState = WindowState.Normal;
            }
        }

        private void btnChangePassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new ChangePasswordModal().ShowDialog();
        }
        #endregion

        #region Logic used in another window
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        #endregion

        #region Methods for multiple windows
        public static void SetImage(string imagePath, Image imageControl)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath) == false)
                {
                    imageControl.Source = new BitmapImage(new Uri(imagePath));
                    imageControl.ToolTip = imagePath;
                }
                else
                {
                    imageControl.Source = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen\n\nDetalle del error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Window's logic
        public bool IsUserLoggedIn()
        {
            if (UserLoggedIn == null)
            {
                new LoginWindow().ShowDialog();
                DisplayUserLoggedInOptions();
            }
            
            return UserLoggedIn != null;
        }

        private void DisplayUserLoggedInOptions()
        {
            if (UserLoggedIn == null)
            {
                lblLogin.Content = string.Empty;
                btnChangePassword.Visibility = Visibility.Hidden;
            }
            else
            {
                lblLogin.Content = string.Format("Bienvenido, {0} {1}",
                                                    UserLoggedIn.FirstName,
                                                    UserLoggedIn.LastName);
                btnChangePassword.Visibility = Visibility.Visible;
            }
        }
        #endregion
    }
}
