using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using VersionManagerUI.Services;
using static VersionManagerUI.Services.ImportService;

namespace VersionManagerUI.Pages
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Import : Page
    {
        private ManagedVersionsService _versionService;
        private ImportService _importService;
        public PBar ProgressBarData = new PBar();

        public Import(ManagedVersionsService versionService, ImportService importService)
        {
            InitializeComponent();
            _versionService = versionService;
            _importService = importService;
            ProgressBar.DataContext = ProgressBarData;
            bannerAlreadyImported.Visibility = bannerCanImport.Visibility = bannerInvalidDirectory.Visibility = Visibility.Hidden;
        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            btnImport.IsEnabled = false;
            tbGameDir.IsEnabled = false;
            btnBrowse.IsEnabled = false;
            btnImportText.Text = "Importing...";
            Progress<int> progress = new Progress<int>(percent =>
            {
                ProgressBarData.progress = percent;
            });
            string dir = tbGameDir.Text;
            await Task.Run(() => _importService.Import(dir, progress));
            tbGameDir.IsEnabled = true;
            btnBrowse.IsEnabled = true;
            btnImportText.Text = "Done";
        }

        private void tbGameDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProgressBarData.progress = 0;
            btnImportText.Text = "Import";
            string path = tbGameDir.Text;
            ImportStatus result = _importService.CanImport(path);
            ShowBannerWithResult(result);
            btnImport.IsEnabled = result == ImportStatus.CAN_IMPORT;
        }

        private void ShowBannerWithResult(ImportStatus result)
        {
            bannerAlreadyImported.Visibility = bannerCanImport.Visibility = bannerInvalidDirectory.Visibility = Visibility.Hidden;
            switch (result)
            {
                case ImportStatus.ALREADY_EXISTS:
                    bannerAlreadyImported.Visibility = Visibility.Visible;
                    break;
                case ImportStatus.INVALID_PATH:
                    bannerInvalidDirectory.Visibility = Visibility.Visible;
                    break;
                case ImportStatus.CAN_IMPORT:
                    bannerCanImport.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
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
