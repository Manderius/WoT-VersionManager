using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using VersionManager.Filesystem;
using VersionManager.Parsing;
using VersionManager.Utils;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for VersionInfo.xaml
    /// </summary>
    public partial class VersionInfo : Page
    {
        public VersionInfo()
        {
            InitializeComponent();
        }

        private void btnBrowseWoTFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtFolder.Text = dialog.FileName;
            }
        }

        private async void btnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            btnShowInfo.IsEnabled = false;
            string gameDir = txtFolder.Text;
            DirectoryInfo gameDirInfo = new DirectoryInfo(gameDir);
            string version = Helpers.GetGameVersion(gameDir);
            RootDirectoryEntity root = new RootDirectoryEntity(version);
            await Task.Run(() => GameDirectoryParser.Parse(gameDirInfo, root, gameDirInfo.FullName.Length, false, null, new IgnoreList()));
            long files = Helpers.TotalFiles(root);
            long size = Helpers.TotalSize(root) / (1024 * 1024);
            string result = string.Format("Directory: {0}\nVersion: {1}\nTotal files: {2:N0}\nTotal size: {3:N0} MB", gameDir, version, files, size);

            MessageBox.Show(result);
            btnShowInfo.IsEnabled = true;
        }
    }
}
