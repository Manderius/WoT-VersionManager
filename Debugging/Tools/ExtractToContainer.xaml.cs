using Debugging.Common;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Extraction;
using VersionManager.Filesystem;
using VersionManager.Persistence;
using VersionManager.Utils;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for ExtractToContainer.xaml
    /// </summary>
    public partial class ExtractToContainer : Page
    {
        private DirectoryCache _dirCache { get; set; }
        private string _dirCacheFile { get; set; }
        public ExtractToContainer()
        {
            InitializeComponent();
        }

        private void SelectDirectory(TextBox location)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    location.Text = dialog.FileName;
                }
            }
        }

        private void ExtractGameDir(string containerPath, string versionFilePath, string gamePath)
        {
            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });
            RootDirectoryEntity deser = new RootDirectoryEntityIO().Deserialize(versionFilePath);
            ex.Extract(deser, gamePath, Helpers.EntityToPath(containerPath), _dirCache, null);
        }

        private void btnBrowseContainer_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(txtContainer);               
        }

        private void btnBrowseVersionFile_Click(object sender, RoutedEventArgs e)
        {
            txtVersionFile.Text = Utils.SelectXML();
        }

        private void btnBrowseGameFolder_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(txtGameFolder);
        }

        private void btnBrowseDirCache_Click(object sender, RoutedEventArgs e)
        {
            _dirCacheFile = Utils.SelectXML("DirectoryCache");
            txtDirCacheFile.Text = _dirCacheFile;
        }

        private bool LoadDirCache()
        {
            if (_dirCacheFile == null || txtContainer.Text == null)
                return false;
            _dirCache = new DataContractXMLLoader().Deserialize<DirectoryCache>(_dirCacheFile);
            _dirCache.ContainerPath = txtContainer.Text;
            return true;
        }

        private void SaveDirCache()
        {
            new DataContractXMLLoader().Serialize(_dirCache, _dirCacheFile);
        }

        private async void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            _dirCacheFile = txtDirCacheFile.Text;
            if (!LoadDirCache())
                return;
            btnExtract.IsEnabled = false;
            btnExtract.Content = "Saving...";
            string container = txtContainer.Text;
            string xml = txtVersionFile.Text;
            string game = txtGameFolder.Text;
            await Task.Run(() => ExtractGameDir(container, xml, game));
            SaveDirCache();
            btnExtract.Content = "Save";
            btnExtract.IsEnabled = true;
        }

        
    }
}
