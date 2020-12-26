using EPS.Administration.APIAccess.Models.Exceptions;
using EPS.Administration.Models.APICommunication;
using EPS.Administration.ServiceUI.View;
using EPS.Administration.ServiceUI.View.Menu;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows;

namespace EPS.Administration.ServiceUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string AuthenticationKey { get; private set; }
        private static int NOTIFICATION_DELAY = 5;
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        private static MainWindow _mainWindow;

        public static MainWindow Instance
        {
            get
            {
                return _mainWindow;
            }
            set { _mainWindow = value; }
        }

        internal ObservableCollection<BaseResponse> _responseQueue { get; set; }
        private static Timer _timer { get; set; }

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            _responseQueue = new ObservableCollection<BaseResponse>();
            NotificationList.ItemsSource = _responseQueue;
        }

        public void Authenticate(string token)
        {
            AuthenticationKey = token;
            ChangeView(new MenuView());
        }

        public void AddNotification(BaseResponse response)
        {
            App.Current.Dispatcher.Invoke(() => _responseQueue.Add(response));
            App.Current.Dispatcher.Invoke(() => EnqueueTimer(true));
        }

        public void AddNotification(ServiceException response)
        {
            var r = new BaseResponse()
            {
                Error = ErrorCode.ServiceError,
                Message = " | " + response.Message
            };

            AddNotification(r);
        }

        private void EnqueueTimer(object state)
        {
            if (_responseQueue != null && _responseQueue.Count > 0)
            {
                if (_timer == null)
                {
                    _timer = new Timer(new TimerCallback(EnqueueTimer), null, _responseQueue.Count <= 1 ? TimeSpan.FromSeconds(NOTIFICATION_DELAY) : TimeSpan.FromSeconds(0), _responseQueue.Count <= 1 ? TimeSpan.FromSeconds(NOTIFICATION_DELAY) : TimeSpan.FromSeconds(2));
                }
                else
                {
                    if (state != null)
                    {
                        return;
                    }

                    if (_responseQueue.Count == 0)
                    {
                        _timer.Dispose();
                        return;
                    }

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if(_responseQueue.Count > 0)
                        {
                            _responseQueue.RemoveAt(0);
                        }
                    });
                }
            }
        }

        public void ChangeView(object view)
        {
            DataContext = view;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LogInView();
        }
    }
}