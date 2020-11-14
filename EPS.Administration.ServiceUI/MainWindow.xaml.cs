using EPS.Administration.Models.APICommunication;
using EPS.Administration.ServiceUI.View;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private static int NOTIFICATION_DELAY = 3;

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private static MainWindow _mainWindow;
        public static MainWindow Window
        {
            get
            {
                return _mainWindow;
            }
            private set { _mainWindow = value; }
        }

        internal ObservableCollection<BaseResponse> _responseQueue { get; set; }
        private static Timer _timer { get; set; }

        public MainWindow()
        {
            Window = this;
            InitializeComponent();
            _responseQueue = new ObservableCollection<BaseResponse>();
            NotificationList.ItemsSource = _responseQueue;
        }

        public void AddNotification(BaseResponse response)
        {
            _responseQueue.Add(response);
            EnqueueTimer(true);
        }

        private void EnqueueTimer(object state)
        {
            if (_responseQueue != null && _responseQueue.Count > 0)
            {
                if (_timer == null)
                {
                    _timer = new Timer(new TimerCallback(EnqueueTimer), null, TimeSpan.FromSeconds(NOTIFICATION_DELAY), TimeSpan.FromSeconds(NOTIFICATION_DELAY));
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
                        _responseQueue.RemoveAt(0);
                    });
                }
            }
        }

        private static void Dequeue(object state)
        {
        }

        public MainWindow(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LogInView();
        }
    }
}
