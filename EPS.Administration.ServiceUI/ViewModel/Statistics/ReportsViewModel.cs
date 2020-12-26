using AutoMapper;
using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using EPS.Administration.ServiceUI.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Statistics
{
    public class ReportsViewModel : BaseViewModel
    {
        public ObservableCollection<ClassificationCheckBox> Classifications { get; set; }
        public ObservableCollection<LocationCheckbox> Locations { get; set; }
        public ObservableCollection<ModelCheckBox> Models { get; set; }
        public ObservableCollection<StatusCheckBox> Statuses { get; set; }
        public DateTime DateFrom { get; set; } = DateTime.Now.AddDays(-1);
        public DateTime DateTill { get; set; } = DateTime.Now.AddDays(1);

        public ReportsViewModel()
        {
            Task.Run(async () => await GetDeviceMetadata());
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
                    Classifications = new ObservableCollection<ClassificationCheckBox>();
                    metadataResponse.Classifications.ForEach(x => Classifications.Add(new ClassificationCheckBox() { Classification = x, IsChecked = true }));
                    Locations = new ObservableCollection<LocationCheckbox>();
                    metadataResponse.Locations.ForEach(x => Locations.Add(new LocationCheckbox() { Location = x, IsChecked = true }));
                    Models = new ObservableCollection<ModelCheckBox>();
                    metadataResponse.Models.ForEach(x => Models.Add( new ModelCheckBox() { Model = x, IsChecked = true }));
                    Statuses = new ObservableCollection<StatusCheckBox>();
                    metadataResponse.Statuses.ForEach(x => Statuses.Add( new StatusCheckBox() { Status = x, IsChecked = true }));
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                Notify(ex.Message);
            }
        }

        internal async Task<List<Models.Device.Device>> GenerateReport()
        {
            var mdr = new DeviceMetadataResponseWithDates();
            mdr.Metadatas.Classifications = Classifications.Where(x => x.IsChecked).Select(x => x.Classification).ToList();
            mdr.Metadatas.Locations = Locations.Where(x => x.IsChecked).Select(x => x.Location).ToList();
            mdr.Metadatas.Models = Models.Where(x => x.IsChecked).Select(x => x.Model).ToList();
            mdr.Metadatas.Statuses = Statuses.Where(x => x.IsChecked).Select(x => x.Status).ToList();
            mdr.DateFrom = DateFrom;
            mdr.DateTill = DateTill;

            try
            {
                var resp = await Service.GetReportDevices(Token, mdr);

                if (resp == null || resp.Count() == 0)
                {
                    Notify("Failed to generate report, no devices were found by specified filter");
                    return null;
                }
                else
                {
                    Notify($"Received '{resp.Count()}' items");
                    return resp;
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                Notify(ex.Message);
            }
            return null;
        }
    }
}
