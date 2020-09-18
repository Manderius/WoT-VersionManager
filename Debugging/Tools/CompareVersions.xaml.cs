using System.Windows;
using System.Windows.Controls;
using VersionManager.Filesystem;
using Microsoft.Win32;
using VersionManager.Persistence;
using VersionManager.Utils;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for CompareVersions.xaml
    /// </summary>
    public partial class CompareVersions : Page
    {
        public CompareVersions()
        {
            InitializeComponent();
        }

        private void btnBrowseFirst_Click(object sender, RoutedEventArgs e)
        {
            SelectGameXML(txtFirstVersion);
        }
        private void btnBrowseSecond_Click(object sender, RoutedEventArgs e)
        {
            SelectGameXML(txtSecondVersion);
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

        private async void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            btnCompare.IsEnabled = false;
            btnCompare.Content = "Comparing...";
            string first = txtFirstVersion.Text;
            string second = txtSecondVersion.Text;
            string message = await Task.Run(() => Compare(first, second));
            MessageBox.Show(message);
            btnCompare.Content = "Compare";
            btnCompare.IsEnabled = true;
        }

        private string Compare(string path1, string path2)
        {
            string formatVersion(string version, IEnumerable<FileEntity> files, IEnumerable<FileEntity> unique) =>
                string.Format("Version: {0}\nNumber of files: {1:N0}\nSize: {2:N0} MB\nUnique files: {3:N0} ({4:N0} MB)",
                version, files.Count(), files.Select(f => f.Size).Sum() / (1024 * 1024), unique.Count(), unique.Select(f => f.Size).Sum() / (1024 * 1024));
            RootDirectoryEntity first = new XMLStructureLoader().Deserialize(path1);
            RootDirectoryEntity second = new XMLStructureLoader().Deserialize(path2);

            IEnumerable<FileEntity> firstFiles = first.GetAllFileEntities(true).OfType<FileEntity>();
            IEnumerable<FileEntity> secondFiles = second.GetAllFileEntities(true).OfType<FileEntity>();

            IEnumerable<FileEntity> firstUnique = firstFiles.Except(secondFiles);
            IEnumerable<FileEntity> secondUnique = secondFiles.Except(firstFiles);

            string message = string.Format("{0}\n\n{1}", formatVersion(first.Version, firstFiles, firstUnique), formatVersion(second.Version, secondFiles, secondUnique));
            return message;
        }

    }
}
