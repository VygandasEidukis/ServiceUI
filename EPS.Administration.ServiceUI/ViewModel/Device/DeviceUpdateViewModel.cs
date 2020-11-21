using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.ServiceUI.View.Menu;

namespace EPS.Administration.ServiceUI.ViewModel.Device
{
    public class DeviceUpdateViewModel : BaseViewModel
    {
        public Models.Device.Device Device { get; set; }
        public DeviceUpdateViewModel(string serialNumber)
        {
            GetAndAssignDevice(serialNumber);
        }

        public async void GetAndAssignDevice(string serialNumber)
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
                    MainWindow.Instance.AddNotification(baseResponse ?? new BaseResponse() { Error = ErrorCode.InternalError,  Message = "Failed to receive response from host." });
                    MainWindow.Instance.ChangeView(new MenuView());
                }
                else
                {
                    Device = baseResponse.RecievedDevice;
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
