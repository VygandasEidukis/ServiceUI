﻿using EPS.Administration.ServiceUI.View.Menu;
using EPS.Administration.ServiceUI.ViewModel.Device;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new DeviceViewModel(); 
            CurrentPage.Text = "1";
            startDate.SelectedDate = null;
            await Context.GetDevices(0);

            if (Context.Devices != null)
            {
                CollectionViewSource.GetDefaultView(Context.Devices).Refresh();
            }
        }

        private async void Filter(object sender, System.Windows.RoutedEventArgs e)
        {
            await ChangePageAsync(0);
            CurrentPage.Text = "1";
        }

        private async void NextPage(object sender, System.Windows.RoutedEventArgs e)
        {
            var page = Context.Filter.Page + 1;
            if (page >= Context.Filter.AllPages)
            {
                page = Context.Filter.AllPages -1;
            }

            await ChangePageAsync(page);
            CurrentPage.Text = (page + 1).ToString();
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

            await ChangePageAsync(page);
        }

        private async void LastPage(object sender, System.Windows.RoutedEventArgs e)
        {
            await ChangePageAsync(Context.Filter.AllPages - 1);
            CurrentPage.Text = (Context.Filter.AllPages).ToString();
        }

        private async void FirstPage(object sender, System.Windows.RoutedEventArgs e)
        {
            await ChangePageAsync(0);
            CurrentPage.Text = (1).ToString();
        }

        private async Task ChangePageAsync(int page)
        {
            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(10);
                await App.Current.Dispatcher.Invoke(async () => await Context.GetDevices(page));
            });
        }

        private void DoubleClickTerminalItem(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = TerminalList.SelectedItem as Models.Device.Device;

            if (item == null || string.IsNullOrEmpty(item.SerialNumber))
            {
                return;
            }

            MenuView.Instance.ChangeView(new DeviceUpdateView(item.SerialNumber));
        }

        private void AddPromotionButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuView.Instance.ChangeView(new DeviceUpdateView(null));
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Filter(null, null);
            }
        }
    }
}
