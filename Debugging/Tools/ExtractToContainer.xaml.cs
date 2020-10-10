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

        private void SelectGameXML(TextBox location)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Version Manager XML File|*.xml";
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                location.Text = dialog.FileName;
            }
        }

        private void ExtractGameDir(string containerPath, string versionFilePath, string gamePath)
        {
            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });
            RootDirectoryEntity deser = new RootDirectoryEntityIO().Deserialize(versionFilePath);
            DirectoryCache cache = DirectoryCache.FromDirectory(containerPath);
            ex.Extract(deser, gamePath, Helpers.EntityToPath(containerPath), cache, null);
        }

        private void btnBrowseContainer_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(txtContainer);
        }

        private void btnBrowseVersionFile_Click(object sender, RoutedEventArgs e)
        {
            SelectGameXML(txtVersionFile);
        }

        private void btnBrowseGameFolder_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(txtGameFolder);
        }

        private async void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            btnExtract.IsEnabled = false;
            btnExtract.Content = "Saving...";
            string container = txtContainer.Text;
            string xml = txtVersionFile.Text;
            string game = txtGameFolder.Text;
            await Task.Run(() => ExtractGameDir(container, xml, game));
            btnExtract.Content = "Save";
            btnExtract.IsEnabled = true;
        }
    }
}
