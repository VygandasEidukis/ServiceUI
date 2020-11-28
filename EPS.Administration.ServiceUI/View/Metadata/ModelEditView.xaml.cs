using EPS.Administration.ServiceUI.ViewModel.Metadata;
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

namespace EPS.Administration.ServiceUI.View.Metadata
{
    /// <summary>
    /// Interaction logic for ModelEditView.xaml
    /// </summary>
    public partial class ModelEditView : UserControl
    {

        public ModelViewModel Context
        {
            get
            {
                return DataContext as ModelViewModel;
            }
        }

        public ModelEditView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ModelViewModel();
        }

        private async void Update(object sender, RoutedEventArgs e)
        {
            await Context.AddOrUpdateModels();
            UserControl_Loaded(null, null);
        }

        private void AddToList(object sender, RoutedEventArgs e)
        {
            Context.Models.Add(new Models.Device.DeviceModel());
        }
    }
}
