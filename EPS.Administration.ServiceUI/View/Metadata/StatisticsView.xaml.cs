using EPS.Administration.ServiceUI.Model.Metadata;
using EPS.Administration.ServiceUI.ViewModel.Statistics;
using FastExcel;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EPS.Administration.ServiceUI.View.Metadata
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public ReportsViewModel Context
        {
            get
            {
                return DataContext as ReportsViewModel;
            }
        }

        public StatisticsView()
        {
            InitializeComponent();
        }

        private void DatePickerTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //datePicker.IsDropDownOpen = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ReportsViewModel();
            DateTill.DisplayDateEnd = DateTime.Now;
            DateTill.DisplayDateStart = DateTime.Parse("01/01/2000");
            DateFrom.DisplayDateEnd = DateTime.Now;
            DateFrom.DisplayDateStart = DateTime.Parse("01/01/2000");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Groups == null) return;

            foreach (var group in Groups.Items)
            {
                (group as ClassificationCheckBox).IsChecked = true;
            }
            CollectionViewSource.GetDefaultView(Context.Classifications).Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Groups == null) return;

            foreach (var group in Groups.Items)
            {
                (group as ClassificationCheckBox).IsChecked = false;
            }
            CollectionViewSource.GetDefaultView(Context.Classifications).Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Models == null) return;

            foreach (var group in Models.Items)
            {
                (group as ModelCheckBox).IsChecked = true;
            }
            CollectionViewSource.GetDefaultView(Context.Models).Refresh();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Models == null) return;

            foreach (var group in Models.Items)
            {
                (group as ModelCheckBox).IsChecked = false;
            }
            CollectionViewSource.GetDefaultView(Context.Models).Refresh();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Locations == null) return;

            foreach (var group in Locations.Items)
            {
                (group as LocationCheckbox).IsChecked = true;
            }
            CollectionViewSource.GetDefaultView(Context.Locations).Refresh();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (Locations == null) return;

            foreach (var group in Locations.Items)
            {
                (group as LocationCheckbox).IsChecked = false;
            }
            CollectionViewSource.GetDefaultView(Context.Locations).Refresh();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (Status == null) return;

            foreach (var group in Status.Items)
            {
                (group as StatusCheckBox).IsChecked = true;
            }
            CollectionViewSource.GetDefaultView(Context.Statuses).Refresh();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (Status == null) return;

            foreach (var group in Status.Items)
            {
                (group as StatusCheckBox).IsChecked = false;
            }
            CollectionViewSource.GetDefaultView(Context.Statuses).Refresh();
        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            var devices = await Context.GenerateReport();
            if (devices != null && devices.Any())
            {
                Ookii.Dialogs.Wpf.VistaFolderBrowserDialog ofd = new VistaFolderBrowserDialog();
                ofd.Description = "Please select folder where you wish to save your excel file";
                ofd.UseDescriptionForTitle = true;
                if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                    MessageBox.Show("Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
                if ((bool)ofd.ShowDialog())
                {
                    var path = ofd.SelectedPath;

                    if (Directory.Exists(path))
                    {
                        GenerateReportExcel(path, devices);
                    }
                    else
                    {
                        MessageBox.Show("Directory does not exist");
                    }
                }
            }
        }

        private void GenerateReportExcel(string path, List<Models.Device.Device> devices)
        {
            var name = $"DataReport-{DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss")}.xlsx";
            path = Path.Combine(path, name);
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template.xlsx");

            var fi = new FileInfo(path);
            var tfi = new FileInfo(templatePath);


            Worksheet worksheet = new Worksheet();
            List<Cell> celss = new List<Cell>()
            {
                new Cell(1, "Serial number"),
                new Cell(2, "Model"),
                new Cell(3, "Group"),
                new Cell(4, "Current status"),
                new Cell(5, "Events"),
                new Cell(6, "Last event"),
                new Cell(7, "Current client"),
            };

            List<Row> rows = new List<Row>()
            {
                new Row(1, celss)
            };

            for (int i = 0; i < devices.Count; i++)
            {
                var cells = new List<Cell>()
                {
                    new Cell(1, devices[i].SerialNumber),
                    new Cell(2, devices[i].Model),
                    new Cell(3, devices[i].Classification),
                    new Cell(4, devices[i].LastStatus),
                    new Cell(5, devices[i].DeviceEventsCount),
                    new Cell(6, devices[i].DeviceEvents.OrderByDescending(x => x.Date).First().Date.ToShortDateString()),
                    new Cell(7, devices[i].DeviceEvents.OrderByDescending(x => x.Date).First().Location),
                };
                rows.Add(new Row(i + 2, cells));
            }


            worksheet.Rows = rows.ToArray();
            using (FastExcel.FastExcel fastExcel = new FastExcel.FastExcel(tfi, fi))
            {
                fastExcel.Write(worksheet, "Lapas1");
            }

            File.Open(path, FileMode.Open);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateFrom == null || DateTill == null)
            {
                return;
            }

            DateFrom.IsEnabled = DateRanges.SelectedIndex == 0;
            DateTill.IsEnabled = DateRanges.SelectedIndex == 0;

            if (DateRanges.SelectedIndex != 0)
            {
                DateTill.SelectedDate = DateTime.Now;
                DateTill.SelectedDate = DateTime.Now;
            }

            switch (DateRanges.SelectedIndex)
            {
                case 1:
                    DateFrom.SelectedDate = DateTime.Now.AddDays(-1);
                    DateFrom.DisplayDate = DateTime.Now.AddDays(-1);
                    break;
                case 2:
                    DateFrom.SelectedDate = DateTime.Now.AddDays(-7);
                    DateFrom.DisplayDate = DateTime.Now.AddDays(-7);
                    break;
                case 3:
                    DateFrom.SelectedDate = DateTime.Now.AddMonths(-1);
                    DateFrom.DisplayDate = DateTime.Now.AddMonths(-1);
                    break;
                case 4:
                    DateFrom.SelectedDate = DateTime.Now.AddMonths(-3);
                    DateFrom.DisplayDate = DateTime.Now.AddMonths(-3);
                    break;
                case 5:
                    DateFrom.SelectedDate = DateTime.Now.AddMonths(-6);
                    DateFrom.DisplayDate = DateTime.Now.AddMonths(-6);
                    break;
                case 6:
                    DateFrom.SelectedDate = DateTime.Now.AddYears(-1);
                    DateFrom.DisplayDate = DateTime.Now.AddYears(-1);
                    break;
                default:
                    DateFrom.SelectedDate = DateTime.Now.AddYears(-1);
                    DateFrom.DisplayDate = DateTime.Now.AddYears(-1);
                    break;

            }
        }

        private void DateTill_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateTill.SelectedDate > DateTime.Now)
            {
                DateTill.SelectedDate = DateTime.Now;
            }

            if (DateTill.SelectedDate <= DateFrom.SelectedDate)
            {
                DateFrom.SelectedDate = DateTill.SelectedDate.Value.AddDays(-1);
            }
        }

        private void DateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateFrom.SelectedDate >= DateTime.Now)
            {
                DateFrom.SelectedDate = DateTime.Now.AddDays(-1);
            }

            if (DateTill.SelectedDate.HasValue && DateFrom.SelectedDate.HasValue && DateTill.SelectedDate.Value <= DateFrom.SelectedDate.Value)
            {
                DateFrom.SelectedDate = DateTill.SelectedDate.Value.AddDays(-1);
            }
        }
    }
}
