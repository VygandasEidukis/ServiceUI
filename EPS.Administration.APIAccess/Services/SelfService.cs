using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.APICommunication.Filter;
using EPS.Administration.Models.Device;
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
        private const string POST_UPDATEDEVICE = "api/Devices/AddOrUpdate";

        // GET Requests
        private const string GET_GETDEVICE = "api/Devices?serialNumber={0}";
        private const string GET_GETDEVICEMETADATA = "api/Devices/GetMetadata";

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

        public async Task<DeviceResponse> GetDevice(string token, string serialNumber)
        {
            var link = string.Format(GET_GETDEVICE, serialNumber);
            return await RequestHandler.ProcessGetRequest<DeviceResponse>(link, token);
        }

        public async Task<DeviceMetadataResponse> GetDeviceMetadata(string token)
        {
            return await RequestHandler.ProcessGetRequest<DeviceMetadataResponse>(GET_GETDEVICEMETADATA, token);
        }

        public async Task<BaseResponse> UpdateDevice(string token, Device device)
        {
            return await RequestHandler.ProcessPostRequest<BaseResponse, Device>(POST_UPDATEDEVICE, device, token);
        }
    }
}