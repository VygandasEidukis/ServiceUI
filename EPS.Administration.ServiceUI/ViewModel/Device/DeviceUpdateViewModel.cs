using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.APIAccess.Services;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.APICommunication.Filter;
using EPS.Administration.Models.Device;
using EPS.Administration.ServiceUI.Model;
using EPS.Administration.ServiceUI.View.Menu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Device
{
    public class DeviceUpdateViewModel : BaseViewModel
    {
        //MULTI DEVICES
        public Models.Device.Device CurrentDevice { get; set; }
        public ObservableCollection<Models.Device.Device> MultiDevices { get; set; }
        public ObservableCollection<SerialNumberModel> SerialNumbers { get; set; }
        public SerialNumberModel SerialNumber { get; set; }

        public Models.Device.Device Device { get; set; }
        public ObservableCollection<DeviceEvent> DeviceEvents { get; set; }
        public DeviceEvent CurrentEvent { get; set; }

        //Metadata
        public ObservableCollection<Classification> Classifications { get; set; }
        public ObservableCollection<DeviceLocation> Locations { get; set; }
        public ObservableCollection<DeviceModel> Models { get; set; }
        public ObservableCollection<DetailedStatus> Statuses { get; set; }
        public FileDefinition Document { get; set; }
        public bool IsEdit { get; set; }

        public DeviceUpdateViewModel(string serialNumber)
        {
            SerialNumbers = new ObservableCollection<SerialNumberModel>();
            Init(serialNumber);
        }

        private async void Init(string serialNumber)
        {
            CurrentDevice = new Models.Device.Device();
            MultiDevices = new ObservableCollection<Models.Device.Device>();

            IsEdit = !string.IsNullOrEmpty(serialNumber);
            if (IsEdit)
            {
                await GetAndAssignDevice(serialNumber);
                CurrentDevice = Device;
            }
            else
            {
                Device = new Models.Device.Device();
                DeviceEvents = new ObservableCollection<DeviceEvent>();
            }
            await GetDeviceMetadata();
        }

        private async Task GetDeviceMetadata()
        {
            try
            {
                DeviceMetadataResponse metadataResponse = await Service.GetDeviceMetadata(Token);

                if (metadataResponse == null || metadataResponse.Error != ErrorCode.OK)
                {
                    Notify(metadataResponse);
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
                Notify(ex.Message);
            }
        }

        internal void AddDocument(string filePath, string fileName)
        {
            Document = new FileDefinition()
            {
                FileName = fileName,
                Path = filePath
            };

            if(!File.Exists(Document.Path))
            {
                Notify($"'{ Document.Path ?? "EMPTY" }' does not exist");
            }
        }

        private async Task GetAndAssignDevice(string serialNumber)
        {

            if (string.IsNullOrEmpty(serialNumber))
            {
                Notify(ErrorCode.InternalError, "Did not receive serial number");
                return;
            }

            try
            {
                DeviceResponse baseResponse = await Service.GetDevice(Token, serialNumber);

                if (baseResponse == null || baseResponse.Error != ErrorCode.OK)
                {
                    Notify(baseResponse);
                    MainWindow.Instance.ChangeView(new MenuView());
                }
                else
                {
                    Device = baseResponse.RecievedDevice;
                    Document = Device.Document;
                    SerialNumber = new SerialNumberModel();
                    SerialNumber.SerialNumber = Device.SerialNumber;
                    Device.DeviceEvents.OrderByDescending(x => x.Date);
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
                Notify(ex.Message);
            }
        }

        public async void AddOrUpdateAll()
        {
            foreach (var device in MultiDevices)
            {
                Device.SerialNumber = device.SerialNumber;
                Device.Model = device.Model;
                Device.OwnedBy = device.OwnedBy;
                Device.AcquisitionDate = device.AcquisitionDate;
                bool success = await AddOrUpdate();

                if (!success)
                {
                    return;
                }
            }

            if (MultiDevices == null || MultiDevices.Count == 0)
            {
                Notify("No devices registered in 'Individual device info' tab.");
                return;
            }

            MainWindow.Instance.ChangeView(new MenuView());
        }

        public async Task<bool> AddOrUpdate()
        {
            var document = await UploadDocument();
            if (document != null)
            {
                Device.Document = document;
            }

            Device.DeviceEvents = new List<DeviceEvent>();
            foreach (var evnt in DeviceEvents)
            {
                Device.DeviceEvents.Add(evnt);
            }

            if (!await VaildateChageAsync(Device))
            {
                return false;
            }

            try
            {
                BaseResponse baseResponse = await Service.UpdateDevice(Token, Device);

                if (baseResponse == null || baseResponse.Error != ErrorCode.OK)
                {
                    Notify(baseResponse);
                }
                else
                {
                    Notify(ErrorCode.OK, $"Device {Device.SerialNumber} was updated successfully.");
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                Notify(ex.Message);
            }
            finally
            {
                if (IsEdit)
                {
                    MainWindow.Instance.ChangeView(new MenuView());
                }
            }
            return true;
        }

        private async Task<bool> VaildateChageAsync(Models.Device.Device device)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(device.SerialNumber))
            {
                Notify(ErrorCode.ValidationError, $"SerialNumber is not set in 'Individual device info' tab.");
                valid = false;
            }

            if (device.Model == null)
            {
                Notify(ErrorCode.ValidationError, $"Model is not set in 'Individual device info' tab.");
                valid = false;
            }

            if (device.OwnedBy == null)
            {
                Notify(ErrorCode.ValidationError, $"Owner of the device is not set in 'Individual device info' tab.");
                valid = false;
            }

            if (device.AcquisitionDate == DateTime.MinValue)
            {
                Notify(ErrorCode.ValidationError, $"Acquisition date of the device is not set in 'Individual device info' tab.");
                valid = false;
            }

            if (device.AcquisitionDate > DateTime.Now)
            {
                Notify(ErrorCode.ValidationError, $"Acquisition date of the device cannot be set to future, in 'Individual device info' tab.");
                valid = false;
            }

            if (MultiDevices.Count == 0 && !IsEdit)
            {
                Notify(ErrorCode.ValidationError, $"List of the device individual info cannot be empty , in 'Individual device info' tab.");
                valid = false;
            }

            if (device.Status == null)
            {
                Notify(ErrorCode.ValidationError, $"Acquisition status field cannot be empty, in 'Common data' tab.");
                valid = false;
            }

            if (device.InitialLocation == null)
            {
                Notify(ErrorCode.ValidationError, $"Device owner field cannot be empty, in 'Common data' tab.");
                valid = false;
            }

            if (device.SfDate == DateTime.MinValue)
            {
                Notify(ErrorCode.ValidationError, $"Invoice date of the device is not set in 'Common data' tab.");
                valid = false;
            }

            if (device.SfDate > DateTime.Now)
            {
                Notify(ErrorCode.ValidationError, $"Invoice date of the device cannot be set to future, in 'Common data' tab.");
                valid = false;
            }

            if (device.DeviceEvents == null || device.DeviceEventsCount == 0)
            {
                Notify(ErrorCode.ValidationError, $"Device event cannot be empty, in 'Device events' tab.");
                valid = false;
            }
            else
            {
                for (var i = 0; i < device.DeviceEventsCount; i++)
                {
                    if (device.DeviceEvents[i].Status == null)
                    {
                        Notify(ErrorCode.ValidationError, $"Device event {i + 1} 'Event status' field cannot be empty, in 'Device events' tab.");
                        valid = false;
                    }

                    if (device.DeviceEvents[i].Location == null)
                    {
                        Notify(ErrorCode.ValidationError, $"Device event {i + 1} 'Current client' field cannot be empty, in 'Device events' tab.");
                        valid = false;
                    }

                    if (device.DeviceEvents[i].Date == DateTime.MinValue)
                    {
                        Notify(ErrorCode.ValidationError, $"Device event {i + 1} 'Date of event' field cannot be empty, in 'Device events' tab.");
                        valid = false;
                    }

                    if (device.DeviceEvents[i].Date > DateTime.Now)
                    {
                        Notify(ErrorCode.ValidationError, $"Device event {i + 1} 'Date of event' field cannot be set in the future, in 'Device events' tab.");
                        valid = false;
                    }
                }
            }

            if (await AnyDevicesWithSerialNumber(device.SerialNumber) || MultiDevices.Count(x => x.SerialNumber == device.SerialNumber) > 1)
            {
                Notify($"Serial number '{device.SerialNumber}' is not unique");
            }

            return valid;
        }

        public async Task<bool> AnyDevicesWithSerialNumber(string serialNumber)
        {
            var service = ServicesManager.SelfService;
            var Filter = new DeviceFilter();
            Filter.SerialNumberFilter = serialNumber;

            try
            {
                GetDevicesResponse deviceResponse = await service.GetDevices(Token, Filter);

                if (deviceResponse == null || deviceResponse.Error != ErrorCode.OK)
                {
                    Notify(deviceResponse);
                    return true;
                }
                else if (deviceResponse.Devices != null && deviceResponse.Devices.Any())
                {
                    return true;
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                Notify(ex.Message);
            }

            return false;
        }

        private async Task<FileDefinition> UploadDocument()
        {
            if (Document == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(Document.FileName))
            {
                return null;
            }

            if (Device.Document != null)
            {
                if (Device.Document.StoredFileName == Document.StoredFileName)
                {
                    return null;
                }
            }

            var fileUploadResponse = await Service.UploadDocument(Token, Document.Path);
            if (fileUploadResponse.Error == ErrorCode.OK)
            {
                return fileUploadResponse.UploadedFileInfo;
            }
            else
            {
                Notify(fileUploadResponse);
                return null;
            }
        }
    }
}
