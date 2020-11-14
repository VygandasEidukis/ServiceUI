using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public class SelfService : ISelfService
    {
        private const string POST_AUTHENTICATION = "api/User/authenticate";

        public async Task<LogInResponse> LogIn(User user)
        {
            return await RequestHandler.ProcessPostRequest<LogInResponse, User>(POST_AUTHENTICATION, user);
        }
    }
}