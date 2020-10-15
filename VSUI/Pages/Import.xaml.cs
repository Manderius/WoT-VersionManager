using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using VersionManagerUI.MessageWindows;
using VersionManagerUI.Services;
using static VersionManagerUI.Services.ImportService;

namespace VersionManagerUI.Pages
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Import : Page
    {
        private ImportService _importService;
        public PBar ProgressBarData = new PBar();

        public Import(ImportService importService)
        {
            InitializeComponent();
            _importService = importService;
            ProgressBar.DataContext = ProgressBarData;
            bannerAlreadyImported.Visibility = bannerCanImport.Visibility = bannerInvalidDirectory.Visibility = Visibility.Hidden;
        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            btnImport.IsEnabled = false;
            tbGameDir.IsEnabled = false;
            btnBrowse.IsEnabled = false;
            chbImportMods.IsEnabled = false;
            btnImportText.Text = "Importing...";
            Progress<int> progress = new Progress<int>(percent =>
            {
                ProgressBarData.Progress = percent;
            });
            string dir = tbGameDir.Text;
            bool importMods = chbImportMods.IsChecked.GetValueOrDefault(false);
            await Task.Run(() => _importService.Import(dir, importMods, progress));
            tbGameDir.IsEnabled = true;
            btnBrowse.IsEnabled = true;
            chbImportMods.IsEnabled = true;
            btnImportText.Text = "Import";
            new MessageWindow("Finished", "Import successfully finished!\nYou can now play replays from this version through the Replays tab.\nAfter you make sure everything works, you can delete the original game directory.", MessageWindowButtons.OK).ShowDialog();
        }

        private void tbGameDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProgressBarData.Progress = 0;
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
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    tbGameDir.Text = dialog.FileName;
                }
            }
        }
    }

    public class PBar : INotifyPropertyChanged
    {
        private int _progress;
        public int Progress
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
