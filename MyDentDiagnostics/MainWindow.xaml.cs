﻿using System;
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

namespace MyDentDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Instance variables
        //Windows
        private ManageAreasWindow _manageAreasWindow;
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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Controllers.BusinessController.Instance.CloseConnection();
        }

        private void btnManageAreas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_manageAreasWindow == null)
            {
                _manageAreasWindow = new ManageAreasWindow();
                _manageAreasWindow.Closed += Window_Closed;
            }

            _manageAreasWindow.Show();
            _manageAreasWindow.WindowState = WindowState.Normal;
        }
        #endregion
    }
}
