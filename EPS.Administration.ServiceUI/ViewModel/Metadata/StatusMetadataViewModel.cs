using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Metadata
{
    public class StatusMetadataViewModel : BaseViewModel
    {
        public ObservableCollection<DetailedStatus> Statuses { get; set; }
        public DetailedStatus CurrentStatus { get; set; }

        public StatusMetadataViewModel()
        {
            Statuses = new ObservableCollection<DetailedStatus>();
            GetClassifications();
        }

        private async void GetClassifications()
        {
            Statuses = new ObservableCollection<DetailedStatus>();

            var service = ServicesManager.SelfService;
            try
            {
                var statusResponse = await service.GetStatuses(MainWindow.Instance.AuthenticationKey);

                if (statusResponse == null || statusResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(statusResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
                }
                else
                {
                    foreach (var stat in statusResponse.Statuses)
                    {
                        Statuses.Add(stat);
                    }

                    if (Statuses.Count > 0)
                    {
                        CurrentStatus = Statuses[0];
                    }
                }
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }

        internal async Task AddOrUpdateClassification()
        {
            var service = ServicesManager.SelfService;
            try
            {
                if (CurrentStatus == null || string.IsNullOrEmpty(CurrentStatus.Status))
                {
                    MainWindow.Instance.AddNotification(new BaseResponse()
                    {
                        Error = ErrorCode.ValidationError,
                        Message = "Status cannot be empty."
                    });
                    return;
                }

                var statusResponse = await service.UpdateStatus(MainWindow.Instance.AuthenticationKey, CurrentStatus);
                MainWindow.Instance.AddNotification(statusResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }
    }
}
