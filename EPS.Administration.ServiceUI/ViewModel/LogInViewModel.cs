using EPS.Administration.Models.Account;

namespace EPS.Administration.ServiceUI.ViewModel
{
    public class LogInViewModel : BaseViewModel
    {
        public string Username { get; set; } = "TEST";
        public string Password { get; set; } = "TEST";

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
