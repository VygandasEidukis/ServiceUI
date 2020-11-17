using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Device
{
    public class DeviceViewModel : BaseViewModel
    {
        private readonly int PAGE_SIZE = 25;

        public ObservableCollection<Models.Device.Device> Devices { get; set; }

        public DeviceViewModel()
        {
             Task.Run(() => GetDevices(1));
        }

        public async Task GetDevices(int page)
        {
            var service = ServicesManager.SelfService;
            try
            {
                var deviceResponse = await service.GetDevices(page * (PAGE_SIZE - 1), PAGE_SIZE, MainWindow.Instance.AuthenticationKey);

                if (deviceResponse == null || deviceResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(deviceResponse);
                }
                else
                {
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
