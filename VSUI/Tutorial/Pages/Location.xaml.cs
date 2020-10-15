using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.Tutorial.Pages
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Page
    {
        private IPagination _pagination { get; set; }

        public Location(IPagination pagination)
        {
            _pagination = pagination;
            InitializeComponent();
            spNotEnoughSpace.Visibility = Visibility.Collapsed;
            GetFreeSpace();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _pagination.PreviousPage();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            _pagination.NextPage();
        }

        private void GetFreeSpace()
        {
            string root = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
            DriveInfo dInfo = new DriveInfo(root);
            long freeSpaceGB = dInfo.AvailableFreeSpace / (1024 * 1024 * 1024);
            long totalSpaceGB = dInfo.TotalSize / (1024 * 1024 * 1024);
            tbDriveName.Text = root;
            tbDriveCapacity.Text = string.Format("{0} GB free of {1} GB", freeSpaceGB, totalSpaceGB);
            pbDriveCapacity.Value = 100 * (1 - 1.0 * freeSpaceGB / totalSpaceGB);
            if (freeSpaceGB < 50)
            {
                btnYes.Visibility = Visibility.Hidden;
                spNotEnoughSpace.Visibility = Visibility.Visible;
                btnNo.Content = "Close";
            }
        }
    }
}
