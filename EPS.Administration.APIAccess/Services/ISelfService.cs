using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public interface ISelfService
    {
        Task<LogInResponse> LogIn(User user);
        Task<GetDevicesResponse> GetDevices(int from, int count, string token);
    }
}