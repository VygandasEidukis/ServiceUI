using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.ServiceUI.View.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Administration.ServiceUI.ViewModel.Device
{
    public class DeviceImportViewModel
    {
        public async Task UploadFile(string path)
        {
            if (Path.GetExtension(path) == ".xlsx")
            {
                var service = ServicesManager.SelfService;

                try
                {
                    BaseResponse baseResponse = await service.ImportExtenderDataFromExcel(MainWindow.Instance.AuthenticationKey, path);

                    if (baseResponse == null || baseResponse.Error != ErrorCode.OK)
                    {
                        MainWindow.Instance.AddNotification(baseResponse);
                    }
                    else
                    {
                        MainWindow.Instance.ChangeView(new MenuView());
                    }
                }
                catch (ServiceException ex)
                {
                    //TODO: HIGH Add logging
                    MainWindow.Instance.AddNotification(ex);
                }
            }
            else
            {
                MainWindow.Instance.AddNotification(new BaseResponse()
                {
                    Error = ErrorCode.InternalError,
                    Message = $"File '{path ?? "<EMPTY>"}' doesn't exist or isn't an .xlsx file"
                });
            }
        }
    }
}
