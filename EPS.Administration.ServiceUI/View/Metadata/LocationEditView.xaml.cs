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
    /// Interaction logic for LocationEditView.xaml
    /// </summary>
    public partial class LocationEditView : UserControl
    {
        public LocationMetadataViewModel Context
        {
            get
            {
                return DataContext as LocationMetadataViewModel;
            }
        }

        public LocationEditView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LocationMetadataViewModel();
        }

        private void FilterLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            Context.Filter(FilterLocation.Text);
        }
    }
}
