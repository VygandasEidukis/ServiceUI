using EPS.Administration.ServiceUI.Model;
using EPS.Administration.ServiceUI.ViewModel.Device;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EPS.Administration.ServiceUI.View.Device.DeviceUpdateForms
{
    /// <summary>
    /// Interaction logic for IdentificationDataView.xaml
    /// </summary>
    public partial class IdentificationDataView : UserControl
    {
        public DeviceUpdateViewModel Context
        {
            get
            {
                return DataContext as DeviceUpdateViewModel;
            }
        }

        public IdentificationDataView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Context.IsEdit)
            {
                AddDeviceButton.Visibility = Visibility.Collapsed;
                MulitpleDeviceCreation.Visibility = Visibility.Collapsed;
                SerialNumberBox.IsEnabled = false;
            }
        }

        private void AddDevice_Button(object sender, RoutedEventArgs e)
        {
            Context.MultiDevices.Add(new Models.Device.Device());
            Context.SerialNumbers.Add(new SerialNumberModel());
            Context.CurrentDevice = Context.MultiDevices.Last();
            Context.SerialNumber = Context.SerialNumbers.Last();
        }

        private void SerialNumber_Changed(object sender, TextChangedEventArgs e)
        {
            if (MultiDevicesList.SelectedIndex >= 0)
            {
                if (Context != null)
                {
                    Context.MultiDevices[MultiDevicesList.SelectedIndex].SerialNumber = SerialNumberBox.Text;
                }
            }
        }

        private void MultiDevicesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MultiDevicesList.SelectedIndex >= 0)
            {
                Context.CurrentDevice = Context.MultiDevices[MultiDevicesList.SelectedIndex];
            }
        }
    }
}
