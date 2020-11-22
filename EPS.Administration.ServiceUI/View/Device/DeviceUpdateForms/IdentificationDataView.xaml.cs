using EPS.Administration.ServiceUI.ViewModel.Device;
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
            if (Context.Device == null)
            {
                return;
            }

           // Model.SelectedItem = Context.Device.Model;
        }
    }
}
