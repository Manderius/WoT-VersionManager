using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace VSUI.Pages
{
    /// <summary>
    /// Interakční logika pro Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public string DataDir { get; set; }
        private bool _canChangeDir;

        public Settings()
        {
            DataDir = Properties.Settings.Default.DataDirectory;
            DataContext = this;
            InitializeComponent();
        }

        private void btnBrowseDataDir_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbDataDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void tbDataDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            _canChangeDir = false;
            btnSave.IsEnabled = false;

            string path = (sender as System.Windows.Controls.TextBox).Text;

            if (path.Length == 0)
            {
                DisplayMessage("", true);
                return;
            }

            FileInfo fi;
            try
            {
                fi = new FileInfo(path);
            }
            catch (Exception)
            {
                DisplayMessage("Invalid path.", true);
                return;
            }

            if (fi.Directory is null)
            {
                DisplayMessage("Please select a valid directory. Don't select a drive.", true);
                return;
            }

            if (!Path.IsPathRooted(path))
            {
                DisplayMessage("Please select an absolute path (starting with drive letter).", true);
                return;
            }

            DriveInfo newDrive = new DriveInfo(fi.Directory.Root.FullName);

            if (!newDrive.IsReady)
            {
                DisplayMessage("This drive cannot be accessed", true);
                return;
            }

            // Less 15 GB left, but user can choose it
            if (newDrive.AvailableFreeSpace < 15L * 1024 * 1024 * 1024)
            {
                DisplayMessage("Warning: Less than 15 GB remaining on this drive.", true);
            }

            _canChangeDir = true;
            btnSave.IsEnabled = true;
        }

        private void DisplayMessage(string message, bool error = false)
        {
            tbMessage.Text = message;
            tbMessage.Foreground = error ? Brushes.Red : Brushes.Green;
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_canChangeDir)
            {
                return;
            }
            Properties.Settings.Default.DataDirectory = tbDataDir.Text;
            Properties.Settings.Default.Save();
        }
    }
}
