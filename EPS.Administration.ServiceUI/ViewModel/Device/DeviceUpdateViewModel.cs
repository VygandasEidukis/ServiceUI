using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.APIAccess.Services;
using EPS.Administration.Models.APICommunication;
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
        public ObservableCollection<DeviceEvent> DeviceEvents{ get; set; }

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
                await AddOrUpdate();
            }

            MainWindow.Instance.ChangeView(new MenuView());
        }

        public async Task AddOrUpdate()
        {
            var doc = await UploadDocument();
            if (doc != null)
            {
                Device.Document = doc;
            }

            Device.DeviceEvents = new List<DeviceEvent>();
            foreach (var evnt in DeviceEvents)
            {
                Device.DeviceEvents.Add(evnt);
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
