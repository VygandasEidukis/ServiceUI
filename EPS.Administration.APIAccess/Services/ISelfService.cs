using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public interface ISelfService
    {
        Task<LogInResponse> LogIn(User user);
    }
}