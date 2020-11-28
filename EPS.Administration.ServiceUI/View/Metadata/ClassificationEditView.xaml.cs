using EPS.Administration.ServiceUI.ViewModel.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace EPS.Administration.ServiceUI.View.Metadata
{
    /// <summary>
    /// Interaction logic for ClassificationEditView.xaml
    /// </summary>
    public partial class ClassificationEditView : UserControl
    {
        public ClassificationMetadataViewModel Context
        {
            get
            {
                return DataContext as ClassificationMetadataViewModel;
            }
        }

        public ClassificationEditView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ClassificationMetadataViewModel();
        }

        private async void Update(object sender, RoutedEventArgs e)
        {
            await Context.AddOrUpdateClassification();
            UserControl_Loaded(null, null);
        }

        private void AddToList(object sender, RoutedEventArgs e)
        {
            Context.Classifications.Add(new Models.Device.Classification());
        }
    }
}
