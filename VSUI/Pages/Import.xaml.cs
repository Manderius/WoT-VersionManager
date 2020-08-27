using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using VSUI.Services;
using static VSUI.Services.LocalVersionsService;

namespace VSUI.Pages
{
    /// <summary>
    /// Interaction logic for Import.xaml
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
            bannerAlreadyImported.Visibility = bannerCanImport.Visibility = bannerInvalidDirectory.Visibility = Visibility.Hidden;

        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ProgressBarData.progress += 10;
        }

        private void tbGameDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            string path = (sender as System.Windows.Controls.TextBox).Text;
            ImportStatus result = _versionService.CanImport(path);
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
