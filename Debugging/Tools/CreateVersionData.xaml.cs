using Debugging.Common;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Filesystem;
using VersionManager.Hashing;
using VersionManager.Parsing;
using VersionManager.Persistence;
using VersionManager.Utils;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for CreateVersionData.xaml
    /// </summary>
    public partial class CreateVersionData : Page
    {
        public CreateVersionData()
        {
            InitializeComponent();
        }

        private async void btnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo wot = new DirectoryInfo(txtGameFolder.Text);
            string output = txtOutputFile.Text;
            RootDirectoryEntity root = new RootDirectoryEntity(Helpers.GetGameVersion(txtGameFolder.Text));
            HashProvider sha1 = new SHA1HashProvider();
            btnCreateFile.IsEnabled = false;
            btnCreateFile.Content = "Creating...";
            await Task.Run(() => GameDirectoryParser.Parse(wot, root, wot.FullName.Length, sha1, IgnoreList.FromEnumerable(File.ReadAllLines("ignored.txt")), null));
            await Task.Run(() => new RootDirectoryEntityIO().Serialize(root, output));
            btnCreateFile.Content = "Create";
            btnCreateFile.IsEnabled = true;
        }

        private void btnBrowseOutputFile_Click(object sender, RoutedEventArgs e)
        {
            string version = "version";
            try
            {
                version = Helpers.GetGameVersion(txtGameFolder.Text).Replace(".", "_");
            }
            catch (Exception) { }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Version Manager XML File|*.xml";
            dialog.FileName = version;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                txtOutputFile.Text = dialog.FileName;
            }
        }

        private void btnBrowseWoTFolder_Click(object sender, RoutedEventArgs e)
        {
            txtGameFolder.Text = Utils.SelectDirectory();
        }
    }
}
