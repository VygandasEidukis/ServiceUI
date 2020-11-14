using EPS.Administration.APIAccess.Models;

namespace EPS.Administration.ServiceUI.ViewModel
{
    public class LogInViewModel : BaseViewModel
    {
        public string Username { get; set; } = "Admin";
        public string Password { get; set; } = "Admin";

        public LogInViewModel()
        {

        }

        public void ManageLogIn()
        {
            var service = ServicesManager.SelfService;
            service.LogIn(new User(Username, Password));
        }
    }
}
