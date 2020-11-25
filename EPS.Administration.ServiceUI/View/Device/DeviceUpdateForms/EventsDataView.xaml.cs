using EPS.Administration.Models.Device;
using EPS.Administration.ServiceUI.ViewModel.Device;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace EPS.Administration.ServiceUI.View.Device.DeviceUpdateForms
{
    /// <summary>
    /// Interaction logic for EventsDataView.xaml
    /// </summary>
    public partial class EventsDataView : UserControl
    {
        public DeviceUpdateViewModel Context
        {
            get
            {
                return DataContext as DeviceUpdateViewModel;
            }
        }

        public EventsDataView()
        {
            InitializeComponent();
        }

        private void AddNewEvent(object sender, RoutedEventArgs e)
        {
            if (Context.DeviceEvents == null)
            {
                Context.DeviceEvents = new ObservableCollection<DeviceEvent>();
            }

            Context.DeviceEvents.Add(new DeviceEvent());
        }


        private void RemoveEventClick(object sender, RoutedEventArgs e)
        {
            if (EventsList.SelectedIndex >= 0)
            {
                Context.DeviceEvents.RemoveAt(EventsList.SelectedIndex);
            }
        }
    }
}
