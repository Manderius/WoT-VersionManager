using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Controls;
using VersionManagerUI.Data;
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

        public Overview(ManagedVersionsService versionService, GameDirectoryService gameDirService)
        {
            InitializeComponent();
            _versionService = versionService;
            _gameDirService = gameDirService;
            frmOverviewDetails.Navigate(new OverviewEmpty());

            ManagedVersionCollection versions = _versionService.GetManagedVersions();
            lbGameVersions.ItemsSource = versions;
            lbGameVersions.Items.SortDescriptions.Clear();
            lbGameVersions.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Version", System.ComponentModel.ListSortDirection.Descending));
            lbGameVersions.SelectedIndex = 0;

            versions.CollectionChanged += ContentCollectionChanged;
        }

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                lbGameVersions.SelectedIndex = 0;
            }
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
            ProgressWindow pw = new ProgressWindow("Deleting version", "Deleting selected World of Tanks. Do not close the application.");
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
            await Task.Run(() => _gameDirService.RebuildGameDirectory(version, progress));
            await winTask;
        }
    }
}
