using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using VersionManagerUI.Data;
using VersionManagerUI.MessageWindows;
using VersionManagerUI.ProgressWindows;
using VersionManagerUI.Services;

namespace VersionManagerUI.Pages
{

    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {

        private ManagedVersionsService _versionService;
        private GameDirectoryService _gameDirService;
        private ManagedVersionCollection _versions;

        public Overview(ManagedVersionsService versionService, GameDirectoryService gameDirService)
        {
            InitializeComponent();
            _versionService = versionService;
            _gameDirService = gameDirService;
            frmOverviewDetails.Navigate(new OverviewEmpty());

            _versions = _versionService.GetManagedVersions();
            _versions.CollectionChanged += ContentCollectionChanged;
            ShowVersions();
        }

        private void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if ((sender as ManagedVersionCollection).Count == 0)
                {
                    frmOverviewDetails.Navigate(new OverviewEmpty());
                    //return;
                }
            }
            ShowVersions();
        }

        private void ShowVersions()
        {
            lbGameVersions.ItemsSource = _versions.OrderByDescending(v => v.LocalVersion);
            lbGameVersions.SelectedIndex = 0;
        }

        private void lbGameVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                frmOverviewDetails.Navigate(new OverviewDetails(e.AddedItems[0] as ManagedGameVersion, this));
            }
        }

        public async void DeleteVersion(ManagedGameVersion version)
        {
            ProgressWindow pw = new ProgressWindow("Deleting version", "Deleting selected World of Tanks version. Do not close the application.");
            pw.CloseOnComplete = true;
            var winTask = pw.ShowDialogAsync();

            int previous = 0;
            Progress<int> progress = new Progress<int>(prog =>
            {
                if (prog != previous)
                    pw.SetProgress(prog);
                previous = prog;
            });
            await Task.Run(() => _gameDirService.DeleteGameDirectory(version, progress));
            await winTask;
        }

        public async void RebuildDirectory(ManagedGameVersion version)
        {
            ProgressWindow pw = new ProgressWindow("Verify integrity", "Rebuilding World of Tanks directory. Do not close the application.");
            pw.CloseOnComplete = true;
            var winTask = pw.ShowDialogAsync();

            int previous = 0;
            Progress<int> progress = new Progress<int>(prog =>
            {
                if (prog != previous)
                    pw.SetProgress(prog);
                previous = prog;
            });
            try
            {
                await Task.Run(() => _gameDirService.RebuildGameDirectory(version, progress));
            }
            catch (FileNotFoundException ex)
            {
                new MessageWindow("Error", "Corrupted files detected. Please delete this version and re-import it.", MessageWindowButtons.OK).ShowDialog();
                (progress as IProgress<int>).Report(100);
            } 
            await winTask;
        }
    }
}
