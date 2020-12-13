using EPS.Administration.ServiceUI.View.Device;
using EPS.Administration.ServiceUI.View.Metadata;
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

namespace EPS.Administration.ServiceUI.View.Menu
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        private bool _miniMenu { get; set; } = false;
        public Brush BackGroundBrush { get; private set; }
        public Brush ForegroundBrush { get; private set; }

        private static MenuView _menuView;

        public static MenuView Instance
        {
            get { return _menuView; }
            set { _menuView = value; }
        }


        public MenuView()
        {
            InitializeComponent();
            Instance = this;
        }

        public void ChangeView(object view)
        {
            BaseContent.Content = view;
        }

        private void Init()
        {
            BackGroundBrush = new SolidColorBrush(Color.FromRgb(173, 202, 247));
            ForegroundBrush = new SolidColorBrush(Color.FromRgb(169, 183, 196));
        }

        private void MenuSizing_Click(object sender, RoutedEventArgs e)
        {
            _miniMenu = !_miniMenu;

            if (_miniMenu)
            {
                mainMenuWindow.Width = 40;
                DevicesText.Visibility = Visibility.Collapsed;
                ImportExportText.Visibility = Visibility.Collapsed;
                ModelsText.Visibility = Visibility.Collapsed;
                ClassificationText.Visibility = Visibility.Collapsed;
                LocationsText.Visibility = Visibility.Collapsed;
                StatusText.Visibility = Visibility.Collapsed;
                StatisticsText.Visibility = Visibility.Collapsed;
            }
            else
            {
                mainMenuWindow.Width = 160;
                DevicesText.Visibility = Visibility.Visible;
                ImportExportText.Visibility = Visibility.Visible;
                ModelsText.Visibility = Visibility.Visible;
                ClassificationText.Visibility = Visibility.Visible;
                LocationsText.Visibility = Visibility.Visible;
                StatusText.Visibility = Visibility.Visible;
                StatisticsText.Visibility = Visibility.Visible;
            }
        }

        private void UpdateButtonBackgroud()
        {
            UndoMenuButtonColors();

            if (BaseContent.Content.GetType() == typeof(DeviceView))
            {
                DevicesMenuButton.Background = BackGroundBrush;
            }

            if (BaseContent.Content.GetType() == typeof(ImportExportView))
            {
                ImportExportMenuButton.Background = BackGroundBrush;
            }

            if (BaseContent.Content.GetType() == typeof(StatusEditView))
            {
                DeviceStatuses.Background = BackGroundBrush;
            }

            if (BaseContent.Content.GetType() == typeof(ClassificationEditView))
            {
                DeviceClassifications.Background = BackGroundBrush;
            }

            if (BaseContent.Content.GetType() == typeof(LocationEditView))
            {
                DeviceLocations.Background = BackGroundBrush;
            }

            if (BaseContent.Content.GetType() == typeof(ModelEditView))
            {
                DeviceModels.Background = BackGroundBrush;
            }

            if (BaseContent.Content.GetType() == typeof(StatisticsView))
            {
                StatisticsMenuButton.Background = BackGroundBrush;
            }
        }

        private void UndoMenuButtonColors()
        {
            DevicesMenuButton.Background = ForegroundBrush;
            ImportExportMenuButton.Background = ForegroundBrush;
            DeviceStatuses.Background = ForegroundBrush;
            DeviceClassifications.Background = ForegroundBrush;
            DeviceLocations.Background = ForegroundBrush;
            DeviceModels.Background = ForegroundBrush;
            StatisticsMenuButton.Background = ForegroundBrush;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new DeviceView();
            Init();
            UpdateButtonBackgroud();
        }

        private void DevicesMenuButton_Click(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new DeviceView();
            UpdateButtonBackgroud();
        }

        private void ImportExportMenuButton_Click(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new ImportExportView();
            UpdateButtonBackgroud();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.DataContext = new LogInView();
        }

        private void StatusButtonClick(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new StatusEditView();
            UpdateButtonBackgroud();
        }

        private void ClassificationButtonClick(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new ClassificationEditView();
            UpdateButtonBackgroud();
        }

        private void LocationsButtonClick(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new LocationEditView();
            UpdateButtonBackgroud();
        }

        private void ModelsClick(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new ModelEditView();
            UpdateButtonBackgroud();
        }

        private void ReportsMenu_Click(object sender, RoutedEventArgs e)
        {
            BaseContent.Content = new StatisticsView();
            UpdateButtonBackgroud();
        }
    }
}
