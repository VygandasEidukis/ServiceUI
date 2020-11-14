using EPS.Administration.APIAccess.Models;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public class SelfService : ISelfService
    {
        public async Task<string> LogIn(Models.User user)
        {
            Administration.Models.Account.User recievedUser = await RequestHandler.ProcessPostRequest<Administration.Models.Account.User, User>("api/User/authenticate", user);
            return recievedUser.Token;
        }
    }
}