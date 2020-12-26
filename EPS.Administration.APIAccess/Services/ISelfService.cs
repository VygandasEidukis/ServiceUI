using EPS.Administration.Models.Account;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.APICommunication.Filter;
using EPS.Administration.Models.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    public interface ISelfService
    {
        Task<LogInResponse> LogIn(User user);
        Task<GetDevicesResponse> GetDevices(string token, DeviceFilter filter);
        Task<BaseResponse> ImportExtenderDataFromExcel(string token, string path);
        Task<DeviceResponse> GetDevice(string token, string serialNumber);
        Task<DeviceMetadataResponse> GetDeviceMetadata(string token);
        Task<BaseResponse> UpdateDevice(string authenticationKey, Device device);
        Task<GetStatusResponse> GetStatuses(string token);
        Task<GetLocationResponse> GetLocations(string token);
        Task<GetModelResponse> GetModels(string token);
        Task<GetClassificationResponse> GetClassifications(string token);
        Task<BaseResponse> UpdateStatus(string token, DetailedStatus status);
        Task<BaseResponse> UpdateLocation(string token, DeviceLocation location);
        Task<BaseResponse> UpdateClassification(string token, Classification classification);
        Task<BaseResponse> UpdateModel(string token, DeviceModel model);
        Task<FileUploadResponse> UploadDocument(string token, string filePath);
        Task<List<Device>> GetReportDevices(string token, DeviceMetadataResponseWithDates metadata);
    }
}