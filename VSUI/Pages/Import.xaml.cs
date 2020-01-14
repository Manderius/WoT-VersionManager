using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using VSUI.Services;
using static VSUI.Services.LocalVersionsService;

namespace VSUI.Pages
{
    /// <summary>
    /// Interakční logika pro Import.xaml
    /// </summary>
    public partial class Import : Page
    {
        private LocalVersionsService _versionService;
        public PBar ProgressBarData = new PBar();

        public Import(LocalVersionsService versionService)
        {
            InitializeComponent();
            _versionService = versionService;
            ProgressBar.DataContext = ProgressBarData;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ProgressBarData.progress += 10;
        }

        private void tbGameDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            string path = (sender as System.Windows.Controls.TextBox).Text;
            ImportResult result = _versionService.CanImport(path);
            if (result == ImportResult.ALREADY_EXISTS)
            {
                DisplayMessage("This game version is already imported.", true);
            }
            else if (result == ImportResult.INVALID_PATH)
            {
                DisplayMessage("This path is not a valid World of Tanks directory.", true);
            }
            else if (result == ImportResult.CAN_IMPORT)
            {
                DisplayMessage("This game version can be imported.");
            }
        }

        private void DisplayMessage(string message, bool error = false)
        {
            tbMessage.Text = message;
            tbMessage.Foreground = error ? Brushes.Red : Brushes.Green;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Browse clicked");
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbGameDir.Text = fbd.SelectedPath;
                }
            }
        }
    }

    public class PBar : INotifyPropertyChanged
    {
        private int _progress;
        public int progress
        {
            get { return _progress; }
            set
            {
                if (value != _progress)
                {
                    _progress = value;
                    OnPropertyChanged("progress");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
