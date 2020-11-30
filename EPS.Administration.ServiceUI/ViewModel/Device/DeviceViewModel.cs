using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.APICommunication.Filter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EPS.Administration.ServiceUI.ViewModel.Device
{
    public class DeviceViewModel : BaseViewModel
    {
        public DeviceFilter Filter { get; set; }
        public ObservableCollection<Models.Device.Device> Devices { get; set; }

        public DeviceViewModel()
        {
            Filter = new DeviceFilter();
            Task.Run(() => GetDevices(0));
        }

        public async Task GetDevices(int page)
        {
            var service = ServicesManager.SelfService;
            Filter.Page = page;

            try
            {
                GetDevicesResponse deviceResponse = await service.GetDevices(Token, Filter);

                if (deviceResponse == null || deviceResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(deviceResponse);
                }
                else
                {
                    Filter.AllPages = deviceResponse.Pages;
                    Devices = new ObservableCollection<Models.Device.Device>();
                    foreach (var device in deviceResponse.Devices)
                    {
                        Devices.Add(device);
                    }
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }
    }
}
