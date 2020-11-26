using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EPS.Administration.ServiceUI.ViewModel.Metadata
{
    public class LocationMetadataViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceLocation> Locations { get; set; }
        public List<DeviceLocation> AllLocations { get; set; }
        public DeviceLocation CurrentLocation { get; set; }

        public LocationMetadataViewModel()
        {
            Locations = new ObservableCollection<DeviceLocation>();
            AllLocations = new List<DeviceLocation>();
            GetClassifications();
        }

        private async void GetClassifications()
        {
            Locations = new ObservableCollection<DeviceLocation>();
            AllLocations = new List<DeviceLocation>();

            var service = ServicesManager.SelfService;
            try
            {
                var locationsResponse = await service.GetLocations(MainWindow.Instance.AuthenticationKey);

                if (locationsResponse == null || locationsResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(locationsResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
                }
                else
                {
                    AllLocations.AddRange(locationsResponse.Locations);
                    foreach (var classific in locationsResponse.Locations)
                    {
                        Locations.Add(classific);
                    }

                    if (Locations.Count > 0)
                    {
                        CurrentLocation = Locations[0];
                    }
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }

        internal void Filter(string text)
        {
            var filtered = AllLocations.Where(x => x.Name.ToLower().StartsWith(text.ToLower())).ToList();
            Locations = new ObservableCollection<DeviceLocation>();
            foreach (var item in filtered)
            {
                Locations.Add(item);
            }
        }
    }
}
