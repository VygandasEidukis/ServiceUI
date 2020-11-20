using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.APICommunication.Filter;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public interface ISelfService
    {
        Task<LogInResponse> LogIn(User user);
        Task<GetDevicesResponse> GetDevices(string token, DeviceFilter filter);
        Task<BaseResponse> ImportExtenderDataFromExcel(string token, string path);
    }
}