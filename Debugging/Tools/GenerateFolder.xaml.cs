using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Filesystem;
using VersionManager.GameGenerator;
using VersionManager.Persistence;
using VersionManager.Utils;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for GenerateFolder.xaml
    /// </summary>
    public partial class GenerateFolder : Page
    {
        public GenerateFolder()
        {
            InitializeComponent();
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

        private void GenerateGameFolder(string containerPath, string versionFilePath, string outputFolderPath)
        {
            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(versionFilePath);
            string gameFolder = Path.Combine(outputFolderPath, "World of Tanks " + root.Version);
            string entityToPath(BaseEntity entity) => Helpers.GetFileDirectory(containerPath, (entity as FileEntity).Hash);
            GameDirGenerator.Generate(root, gameFolder, containerPath, entityToPath);
        }

        private void btnBrowseContainer_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(txtContainer);
        }

        private void btnBrowseVersionFile_Click(object sender, RoutedEventArgs e)
        {
            SelectGameXML(txtVersionFile);
        }

        private void btnBrowseOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(txtOutputFolder);
        }

        private async void btnCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            btnCreateFolder.IsEnabled = false;
            btnCreateFolder.Content = "Creating...";
            string container = txtContainer.Text;
            string xml = txtVersionFile.Text;
            string output = txtOutputFolder.Text;
            await Task.Run(() => GenerateGameFolder(container, xml, output));
            btnCreateFolder.Content = "Create";
            btnCreateFolder.IsEnabled = true;
        }

    }
}
