using EPS.Administration.APIAccess.Services;
using EPS.Administration.Models.APICommunication;
using PropertyChanged;
using System.ComponentModel;

namespace EPS.Administration.ServiceUI.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public string Token { get => MainWindow.Instance.AuthenticationKey; }
        public ISelfService Service { get => ServicesManager.SelfService; }

        public void Notify(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                MainWindow.Instance.AddNotification(new BaseResponse { Message = text });
            }
        }

        public void Notify(ErrorCode errorCode, string message)
        {
            Notify(new BaseResponse { Error = errorCode, Message = message });
        }

        public void Notify(BaseResponse response)
        {
            if (response != null)
            {
                MainWindow.Instance.AddNotification(response);
            }
        }
    }
}