using EPS.Administration.ServiceUI.ViewModel.Device;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EPS.Administration.ServiceUI.View.Device
{
    /// <summary>
    /// Interaction logic for DeviceView.xaml
    /// </summary>
    public partial class DeviceView : UserControl
    {
        public DeviceViewModel Context
        {
            get { return DataContext as DeviceViewModel; }
        }

        public DeviceView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new DeviceViewModel(); 
            CurrentPage.Text = "1";
        }

        private async void Filter(object sender, System.Windows.RoutedEventArgs e)
        {
            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(10);
                await App.Current.Dispatcher.Invoke(async () => await Context.GetDevices(0));
            });
            CurrentPage.Text = "1";
        }

        private async void NextPage(object sender, System.Windows.RoutedEventArgs e)
        {
            var page = Context.Filter.Page + 1;
            if (page >= Context.Filter.AllPages)
            {
                page = Context.Filter.AllPages -1;
            }

            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(10);
                await App.Current.Dispatcher.Invoke(async () => await Context.GetDevices(page++));
            });
            CurrentPage.Text = (page).ToString();
        }

        private async void PreviousPage(object sender, System.Windows.RoutedEventArgs e)
        {
            var page = Context.Filter.Page - 1;
            CurrentPage.Text = (page+1).ToString();

            if (page < 0)
            {
                page = 0;
                CurrentPage.Text = (1).ToString();
            }

            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(10);
                await App.Current.Dispatcher.Invoke(async () => await Context.GetDevices(page--));
            });
        }

        private async void LastPage(object sender, System.Windows.RoutedEventArgs e)
        {
            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(10);
                await App.Current.Dispatcher.Invoke(async () => await Context.GetDevices(Context.Filter.AllPages-1));
            });
            CurrentPage.Text = (Context.Filter.AllPages).ToString();
        }

        private async void FirstPage(object sender, System.Windows.RoutedEventArgs e)
        {
            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(10);
                await App.Current.Dispatcher.Invoke(async () => await Context.GetDevices(0));
            });
            CurrentPage.Text = (1).ToString();
        }
    }
}
