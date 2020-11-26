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
    /// Interaction logic for StatusEditView.xaml
    /// </summary>
    public partial class StatusEditView : UserControl
    {
        public StatusMetadataViewModel Context
        {
            get
            {
                return DataContext as StatusMetadataViewModel;
            }
        }

        public StatusEditView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new StatusMetadataViewModel();
        }
    }
}
