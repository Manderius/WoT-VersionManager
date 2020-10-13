using Debugging.Common;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Filesystem;
using VersionManager.GameRemover;
using VersionManager.Persistence;
using VersionManager.Utils;
using VersionManagerUI.Data;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for DeleteVersion.xaml
    /// </summary>
    public partial class DeleteVersion : Page
    {

        public ManagedVersionCollection ManagedVersions { get; set; }
        private string ManagedVersionsPath { get; set; }
        private DirectoryCache _dirCache { get; set; }
        private string _dirCacheFile { get; set; }
        private string _containerPath { get; set; }

        public DeleteVersion()
        {
            InitializeComponent();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveGameFiles();
        }

        private async void RemoveGameFiles()
        {
            string btnContent = btnRemove.Content as string;
            ManagedGameVersion selected = lbVersions.SelectedItem as ManagedGameVersion;
            if (selected == null)
                return;

            _containerPath = txtContainer.Text;
            if (_containerPath == null || !Directory.Exists(_containerPath))
            {
                MessageBox.Show("Please select container directory.", "Error");
                return;
            }

            if (!LoadDirCache())
                return;

            MessageBoxResult result = MessageBox.Show("This process will remove ALL files from the selected version and delete the game directory. Proceed?", "Warning", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            btnRemove.IsEnabled = false;
            btnRemove.Content = "Removing...";

            if (Directory.Exists(selected.Path))
                await Task.Run(() => Directory.Delete(selected.Path, true));

            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(selected.GameXML);
            List<RootDirectoryEntity> otherVersions = new List<RootDirectoryEntity>();
            foreach (var mgv in ManagedVersions)
            {
                RootDirectoryEntity data = new RootDirectoryEntityIO().Deserialize(mgv.GameXML);
                otherVersions.Add(data);
            }

            await Task.Run(() => GameFilesRemover.RemoveFiles(root, otherVersions, _containerPath, Helpers.EntityToPath(_containerPath), _dirCache, null));
            ManagedVersions.Remove(selected);
            SaveManagedVersions();
            SaveDirCache();

            btnRemove.IsEnabled = true;
            btnRemove.Content = btnContent;
        }

        private void btnBrowseContainer_Click(object sender, RoutedEventArgs e)
        {
            txtContainer.Text = Utils.SelectDirectory("Select container directory");
        }

        private void btnSelectMVFile_Click(object sender, RoutedEventArgs e)
        {
            string path = Utils.SelectXML("ManagedVersions");
            if (path != null)
            {
                ManagedVersionsPath = path;
                LoadManagedVersions();
            }
        }

        private void LoadManagedVersions()
        {
            DataContractXMLLoader dds = new DataContractXMLLoader();
            ManagedVersions = dds.Deserialize<ManagedVersionCollection>(ManagedVersionsPath);
            lbVersions.ItemsSource = ManagedVersions;
        }

        private void SaveManagedVersions()
        {
            DataContractXMLLoader dds = new DataContractXMLLoader();
            dds.Serialize(ManagedVersions, ManagedVersionsPath);
        }

        private bool LoadDirCache()
        {
            MessageBox.Show("Select your DirectoryCache.xml file. It should be in \"WoT Version Manager/Data\" directory.");
            string cacheFile = Utils.SelectXML("DirectoryCache");
            if (cacheFile == null)
                return false;

            _dirCacheFile = cacheFile;
            _dirCache = new DataContractXMLLoader().Deserialize<DirectoryCache>(cacheFile);
            _dirCache.ContainerPath = _containerPath;
            return true;
        }

        private void SaveDirCache()
        {
            new DataContractXMLLoader().Serialize(_dirCache, _dirCacheFile);
        }
    }
}
