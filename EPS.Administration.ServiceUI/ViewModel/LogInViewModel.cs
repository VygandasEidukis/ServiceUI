using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.ServiceUI.View;
using EPS.Administration.ServiceUI.View.Device;
using EPS.Administration.ServiceUI.View.Menu;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel
{
    public class LogInViewModel : BaseViewModel
    {
        public string Username { get; set; } = "TEST";
        public string Password { get; set; } = "TEST";

        public LogInViewModel()
        {
        }

        public async Task ManageLogIn()
        {
            var service = ServicesManager.SelfService;
            try
            {
                var userCreds = await service.LogIn(new User(Username, Password));

                if (userCreds.Error == ErrorCode.OK)
                {
                    userCreds.Message = "Successfully logged in";
                    MainWindow.Instance.Authenticate(userCreds.Token);
                }
                MainWindow.Instance.AddNotification(userCreds);
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }
    }
}