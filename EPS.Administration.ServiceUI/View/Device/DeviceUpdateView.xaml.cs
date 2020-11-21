using EPS.Administration.ServiceUI.ViewModel.Device;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EPS.Administration.ServiceUI.View.Device
{
    public partial class DeviceUpdateView : UserControl
    {
        public string SerialNumber { get; set; }
        public DeviceUpdateViewModel Context
        {
            get
            {
                return DataContext as DeviceUpdateViewModel;
            }
        }

        public DeviceUpdateView(string serialNumber)
        {
            InitializeComponent();
            SerialNumber = serialNumber;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new DeviceUpdateViewModel(SerialNumber);
        }
    }


}
