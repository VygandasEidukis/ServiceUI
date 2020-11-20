using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.APICommunication.Filter;
using System.IO;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public class SelfService : ISelfService
    {
        // POST Requests
        private const string POST_AUTHENTICATION = "api/User/authenticate";
        private const string POST_GETDEVICES = "api/Devices/GetFiltered";
        private const string POST_UPLOADEXTENDERDATA = "api/Files/uploadExtenderData";

        // GET Requests

        public async Task<GetDevicesResponse> GetDevices(string token, DeviceFilter filter)
        {
            var devices = await RequestHandler.ProcessPostRequest<GetDevicesResponse, DeviceFilter>(POST_GETDEVICES, filter, token);
            return devices;       
        }

        public async Task<LogInResponse> LogIn(User user)
        {
            return await RequestHandler.ProcessPostRequest<LogInResponse, User>(POST_AUTHENTICATION, user);
        }

        public async Task<BaseResponse> ImportExtenderDataFromExcel(string token, string path)
        {
            var file = File.Open(path, FileMode.Open);


            return await RequestHandler.ProcessPostFileRequest<BaseResponse, FileStream>(POST_UPLOADEXTENDERDATA, file, file.Name, token);
        }
    }
}