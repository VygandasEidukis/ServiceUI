using EPS.Administration.Models.Device;
using EPS.Administration.ServiceUI.View.Device.DeviceUpdateForms;
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
            Load_DetailedInfo();
            DataContext = new DeviceUpdateViewModel(SerialNumber);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Load_DetailedInfo();
        }

        public void Load_DetailedInfo()
        {
            DeviceUpdateContent.Content = new IdentificationDataView();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeviceUpdateContent.Content = new DetailedInfoView();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DeviceUpdateContent.Content = new EventsDataView();
        }

        private void AddOrUpdateButton(object sender, RoutedEventArgs e)
        {
            if (Context.IsEdit)
            {
                Context.AddOrUpdate();
            }else
            {
                Context.AddOrUpdateAll();
            }
        }
    }


}
