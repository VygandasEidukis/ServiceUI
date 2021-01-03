using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Metadata
{
    public class ClassificationMetadataViewModel : BaseViewModel
    {
        public ObservableCollection<Classification> Classifications { get; set; }
        public Classification CurrentClassification { get; set; }

        public ClassificationMetadataViewModel()
        {
            Classifications = new ObservableCollection<Classification>();
            GetClassifications();
        }

        internal async Task AddOrUpdateClassification()
        {
            var service = ServicesManager.SelfService;
            try
            {
                if (CurrentClassification == null || string.IsNullOrEmpty(CurrentClassification.Code) || string.IsNullOrEmpty(CurrentClassification.Model))
                {
                    MainWindow.Instance.AddNotification(new BaseResponse()
                    {
                        Error = ErrorCode.ValidationError,
                        Message = "Code and Model cannot be empty."
                    });
                    return;
                }

                if (Classifications.Count(x => x.Model == CurrentClassification.Model) > 1)
                {
                    MainWindow.Instance.AddNotification(new BaseResponse()
                    {
                        Error = ErrorCode.ValidationError,
                        Message = "Internal model already exists."
                    });
                    return;
                }

                var locationsResponse = await service.UpdateClassification(Token, CurrentClassification);
                MainWindow.Instance.AddNotification(locationsResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }

        private async void GetClassifications()
        {
            Classifications = new ObservableCollection<Classification>();

            var service = ServicesManager.SelfService;
            try
            {
                GetClassificationResponse classificationResponse = await service.GetClassifications(Token);

                if (classificationResponse == null || classificationResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(classificationResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
                }
                else
                {
                    foreach (var classific in classificationResponse.Classifications)
                    {
                        Classifications.Add(classific);
                    }

                    if (Classifications.Count > 0)
                    {
                        CurrentClassification = Classifications[0];
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
