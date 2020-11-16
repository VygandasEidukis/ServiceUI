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
        private bool _miniMenu { get; set; } = true;
        public MenuView()
        {
            InitializeComponent();
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
                mainMenuWindow.Width = MenuColumn.ActualWidth;
                DevicesText.Visibility = Visibility.Visible;
                ImportExportText.Visibility = Visibility.Visible;
            }
        }
    }
}
