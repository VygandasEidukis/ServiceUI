using EPS.Administration.APIAccess.Services;
using EPS.Administration.ServiceUI.ViewModel;
using Microsoft.Extensions.Configuration;
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
            Init();
        }

        private void Init()
        {
            BaseService.URI = ServicesManager.Configuration.GetSection("ServiceEndpoint").Value;
        }

        private async void LogInButton(object sender, System.Windows.RoutedEventArgs e)
        {
            var token = await Task.Run(Context.ManageLogIn);
            if (!string.IsNullOrEmpty(token))
            {
                MainWindow.Instance.Authenticate(token);
            }
        }

        private void PasswordBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Context.Password = PW_BOX.Password;
        }
    }
}