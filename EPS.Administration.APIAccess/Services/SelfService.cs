using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public class SelfService : ISelfService
    {
        // POST Requests
        private const string POST_AUTHENTICATION = "api/User/authenticate";

        // GET Requests
        private const string GET_GETDEVICES = "api/Devices?from={0}&count={1}";

        public async Task<GetDevicesResponse> GetDevices(int from, int count, string token)
        {
            string request = string.Format(GET_GETDEVICES, from, count);

            return await RequestHandler.ProcessGetRequest<GetDevicesResponse>(request, token);
        }

        public async Task<LogInResponse> LogIn(User user)
        {
            return await RequestHandler.ProcessPostRequest<LogInResponse, User>(POST_AUTHENTICATION, user);
        }
    }
}