using EPS.Administration.Models.Account;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public class SelfService : ISelfService
    {
        private const string POST_AUTHENTICATION = "api/User/authenticate";

        public async Task<string> LogIn(User user)
        {
            User recievedUser = await RequestHandler.ProcessPostRequest<User, User>(POST_AUTHENTICATION, user);
            return recievedUser.Token;
        }
    }
}