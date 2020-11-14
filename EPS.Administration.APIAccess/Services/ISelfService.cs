using EPS.Administration.Models.Account;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public interface ISelfService
    {
        Task<string> LogIn(User user);
    }
}