using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System.Collections.ObjectModel;

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

        private async void GetClassifications()
        {
            Classifications = new ObservableCollection<Classification>();

            var service = ServicesManager.SelfService;
            try
            {
                GetClassificationResponse classificationResponse = await service.GetClassifications(MainWindow.Instance.AuthenticationKey);

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
