using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using EPS.Administration.ServiceUI.View.Menu;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EPS.Administration.ServiceUI.ViewModel.Device
{
    public class DeviceUpdateViewModel : BaseViewModel
    {
        public Models.Device.Device Device { get; set; }
        public ObservableCollection<DeviceEvent> DeviceEvents{ get; set; }
        //Metadata
        public ObservableCollection<Classification> Classifications { get; set; }
        public ObservableCollection<DeviceLocation> Locations { get; set; }
        public ObservableCollection<DeviceModel> Models { get; set; }
        public ObservableCollection<DetailedStatus> Statuses { get; set; }

        public DeviceUpdateViewModel(string serialNumber)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                Device = new Models.Device.Device();
                DeviceEvents = new ObservableCollection<DeviceEvent>();
            }
            else
            {
                GetAndAssignDevice(serialNumber);
            }
            GetDeviceMetadata();
        }

        private async void GetDeviceMetadata()
        {
            var service = ServicesManager.SelfService;
            try
            {
                DeviceMetadataResponse metadataResponse = await service.GetDeviceMetadata(MainWindow.Instance.AuthenticationKey);

                if (metadataResponse == null || metadataResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(metadataResponse);
                }
                else
                {
                    Classifications = new ObservableCollection<Classification>();
                    metadataResponse.Classifications.ForEach(x => Classifications.Add(x));
                    Locations = new ObservableCollection<DeviceLocation>();
                    metadataResponse.Locations.ForEach(x => Locations.Add(x));
                    Models = new ObservableCollection<DeviceModel>();
                    metadataResponse.Models.ForEach(x => Models.Add(x));
                    Statuses = new ObservableCollection<DetailedStatus>();
                    metadataResponse.Statuses.ForEach(x => Statuses.Add(x));
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }

        private async void GetAndAssignDevice(string serialNumber)
        {

            if (string.IsNullOrEmpty(serialNumber))
            {
                MainWindow.Instance.AddNotification(new BaseResponse() { Error = ErrorCode.InternalError, Message = "Did not receive serial number" });
                return;
            }

            var service = ServicesManager.SelfService;
            try
            {
                DeviceResponse baseResponse = await service.GetDevice(MainWindow.Instance.AuthenticationKey, serialNumber);

                if (baseResponse == null || baseResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(baseResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
                    MainWindow.Instance.ChangeView(new MenuView());
                }
                else
                {
                    Device = baseResponse.RecievedDevice;
                    DeviceEvents = new ObservableCollection<DeviceEvent>();
                    if (Device.DeviceEvents != null)
                    {
                        Device.DeviceEvents.ForEach(x => DeviceEvents.Add(x));
                    }
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }

        public async void AddOrUpdate()
        {
            Device.DeviceEvents = new List<DeviceEvent>();
            foreach (var evnt in DeviceEvents)
            {
                Device.DeviceEvents.Add(evnt);
            }

            var service = ServicesManager.SelfService;
            try
            {
                BaseResponse baseResponse = await service.UpdateDevice(MainWindow.Instance.AuthenticationKey, Device);

                if (baseResponse == null || baseResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(baseResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = $"Failed to update device. {Device.SerialNumber}" });
                    MainWindow.Instance.ChangeView(new MenuView());
                }
                else
                {
                    MainWindow.Instance.AddNotification(new BaseResponse() { Error = ErrorCode.OK, Message = $"Device {Device.SerialNumber} was updated successfully." });
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
            finally
            {
                MainWindow.Instance.ChangeView(new MenuView());
            }
        }
    }
}
