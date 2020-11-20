using EPS.Administration.ServiceUI.View.Device;
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

        public MenuView()
        {
            InitializeComponent();
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
            }
            else
            {
                mainMenuWindow.Width = 160;
                DevicesText.Visibility = Visibility.Visible;
                ImportExportText.Visibility = Visibility.Visible;
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
        }

        private void UndoMenuButtonColors()
        {
            DevicesMenuButton.Background = ForegroundBrush;
            ImportExportMenuButton.Background = ForegroundBrush;
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
    }
}
