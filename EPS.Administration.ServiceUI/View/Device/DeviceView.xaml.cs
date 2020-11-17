using EPS.Administration.ServiceUI.ViewModel.Device;
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
        }
    }
}
