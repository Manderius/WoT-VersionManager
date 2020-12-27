using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shell;
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
        private TaskbarItemInfo Taskbar { get; set; }

        public Import(ImportService importService)
        {
            InitializeComponent();
            _importService = importService;
            ProgressBar.DataContext = ProgressBarData;
            bannerAlreadyImported.Visibility = bannerCanImport.Visibility = bannerInvalidDirectory.Visibility = Visibility.Hidden;
            Taskbar = new TaskbarItemInfo();
        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            string dir = tbGameDir.Text;
            ImportStatus result = _importService.CanImport(dir);
            if (result == ImportStatus.ALREADY_EXISTS)
            {
                MessageWindow updateWindow = new MessageWindow("Update", "This version is already imported. Are you sure you want to overwrite it?\nUse this feature only if you want to save a newer build of the same version.", MessageWindowButtons.YesNo);
                updateWindow.ShowDialog();
                if (updateWindow.Result != MessageWindowResult.Yes)
                    return;
            }

            btnImport.IsEnabled = false;
            tbGameDir.IsEnabled = false;
            btnBrowse.IsEnabled = false;
            chbImportMods.IsEnabled = false;
            btnImportText.Text = "Importing...";
            Taskbar.ProgressState = TaskbarItemProgressState.Normal;
            Taskbar.ProgressValue = 0;
            Progress<int> progress = new Progress<int>(percent =>
            {
                ProgressBarData.Progress = percent;
                Taskbar.ProgressValue = percent / 100.0;
            });
            
            bool importMods = chbImportMods.IsChecked.GetValueOrDefault(false);
            await Task.Run(() => _importService.Import(dir, importMods, progress));
            tbGameDir.IsEnabled = true;
            btnBrowse.IsEnabled = true;
            chbImportMods.IsEnabled = true;
            btnImportText.Text = "Import";
            new MessageWindow("Finished", "Import successfully finished!\nYou can now play replays from this version through the Replays tab.\nAfter you make sure everything works, you can delete the original game directory.", MessageWindowButtons.OK).ShowDialog();
            OnPathChanged();
            Taskbar.ProgressState = TaskbarItemProgressState.None;
        }

        private void OnPathChanged()
        {
            ProgressBarData.Progress = 0;
            string path = tbGameDir.Text;
            ImportStatus result = _importService.CanImport(path);
            ShowBannerWithResult(result);
            btnImport.IsEnabled = false;
            switch (result)
            {
                case ImportStatus.CAN_IMPORT:
                    btnImportText.Text = "Import";
                    btnImport.IsEnabled = true;
                    break;
                case ImportStatus.ALREADY_EXISTS:
                    btnImportText.Text = "Update";
                    btnImport.IsEnabled = true;
                    break;
                case ImportStatus.INVALID_PATH:
                    btnImportText.Text = "Import";
                    btnImport.IsEnabled = false;
                    break;
            }
        }

        private void tbGameDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnPathChanged();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).TaskbarItemInfo = Taskbar;
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
