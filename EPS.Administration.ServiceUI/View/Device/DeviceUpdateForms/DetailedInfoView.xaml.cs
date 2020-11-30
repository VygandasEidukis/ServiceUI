using EPS.Administration.ServiceUI.ViewModel.Device;
using Microsoft.Win32;
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

namespace EPS.Administration.ServiceUI.View.Device.DeviceUpdateForms
{
    /// <summary>
    /// Interaction logic for DetailedInfoView.xaml
    /// </summary>
    public partial class DetailedInfoView : UserControl
    {
        public DeviceUpdateViewModel Context
        {
            get
            {
                return DataContext as DeviceUpdateViewModel;
            }
        }

        public DetailedInfoView()
        {
            InitializeComponent();
        }

        private void SelectDocument(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.RestoreDirectory = true;
            ofd.Title = "Choose document for the device";
            ofd.DefaultExt = "pdf";
            ofd.Filter = "pdf files (*.pdf)|*.pdf";
            ofd.Multiselect = false;

            var fileOpened = ofd.ShowDialog();
            if (fileOpened.HasValue && fileOpened.Value)
            {
                SelectedItem.Text = ofd.SafeFileName;
                Context.AddDocument(ofd.FileName, ofd.SafeFileName);
            }
        }
    }
}
