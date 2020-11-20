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

namespace EPS.Administration.ServiceUI.View.Device
{
    /// <summary>
    /// Interaction logic for ImportExportView.xaml
    /// </summary>
    public partial class ImportExportView : UserControl
    {
        public DeviceImportViewModel Context
        {
            get
            {
                return DataContext as DeviceImportViewModel;
            }
        }

        public ImportExportView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new DeviceImportViewModel();
        }

        private void UploadFile(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.RestoreDirectory = true;
            ofd.Title = "Choose device excel sheet";
            ofd.DefaultExt = "xlsx";
            ofd.Filter = "xlsx files (*.xlsx)|*.xlsx";
            ofd.Multiselect = false;

            var fileOpened = ofd.ShowDialog();
            if (fileOpened.HasValue && fileOpened.Value)
            {
                Context.UploadFile(ofd.FileName);
            }
        }

    }
}
