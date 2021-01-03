using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.Models.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Metadata
{
    public class ModelViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceModel> Models { get; set; }
        public DeviceModel CurrentModel { get; set; }

        public ModelViewModel()
        {
            Models = new ObservableCollection<DeviceModel>();
            GetModels();
        }

        internal async Task AddOrUpdateModels()
        {
            var service = ServicesManager.SelfService;
            try
            {
                if (CurrentModel == null || string.IsNullOrEmpty(CurrentModel.Name))
                {
                    MainWindow.Instance.AddNotification(new BaseResponse()
                    {
                        Error = ErrorCode.ValidationError,
                        Message = "Model cannot be empty."
                    });
                    return;
                }

                if (Models.Count(x => x.Name == CurrentModel.Name) > 1)
                {
                    MainWindow.Instance.AddNotification(new BaseResponse()
                    {
                        Error = ErrorCode.ValidationError,
                        Message = "Model name already exists."
                    });
                    return;
                }

                var locationsResponse = await service.UpdateModel(Token, CurrentModel);
                MainWindow.Instance.AddNotification(locationsResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
            }
            catch (ServiceException ex)
            {
                //TODO: HIGH Add logging
                MainWindow.Instance.AddNotification(ex);
            }
        }

        private async void GetModels()
        {
            Models = new ObservableCollection<DeviceModel>();

            var service = ServicesManager.SelfService;
            try
            {
                GetModelResponse modelResponse = await service.GetModels(Token);

                if (modelResponse == null || modelResponse.Error != ErrorCode.OK)
                {
                    MainWindow.Instance.AddNotification(modelResponse ?? new BaseResponse() { Error = ErrorCode.InternalError, Message = "Failed to receive response from host." });
                }
                else
                {
                    foreach (var mdl in modelResponse.Models)
                    {
                        Models.Add(mdl);
                    }

                    if (Models.Count > 0)
                    {
                        CurrentModel = Models[0];
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
