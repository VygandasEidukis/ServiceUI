﻿using EPS.Administration.ServiceUI.ViewModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EPS.Administration.ServiceUI.View
{
    /// <summary>
    /// Interaction logic for LogInView.xaml
    /// </summary>
    public partial class LogInView : UserControl
    {
        private LogInViewModel Context
        {
            get
            {
                return DataContext as LogInViewModel;
            }
        }

        public LogInView()
        {
            InitializeComponent();
            DataContext = new LogInViewModel();
        }

        private async void LogInButton(object sender, System.Windows.RoutedEventArgs e)
        {
            var token = await Task.Run(Context.ManageLogIn);
            MainWindow.Instance.Authenticate(token);
        }
    }
}