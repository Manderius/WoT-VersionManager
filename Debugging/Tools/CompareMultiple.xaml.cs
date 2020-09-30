using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Filesystem;
using VersionManager.Persistence;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for CompareMultiple.xaml
    /// </summary>
    public partial class CompareMultiple : Page
    {
        public ObservableCollection<RootDirectoryEntity> _items;
        public CompareMultiple()
        {
            InitializeComponent();
            _items = new ObservableCollection<RootDirectoryEntity>();
            lbVersions.ItemsSource = _items;
        }

        private void btnAddVersion_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Version Manager XML File|*.xml";
            dialog.Multiselect = true;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                foreach (string file in dialog.FileNames)
                {
                    RootDirectoryEntity ent = new RootDirectoryEntityIO().Deserialize(file);
                    _items.Add(ent);
                }
            }
        }

        private void btnRemoveVersion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Removing version " + (lbVersions.SelectedItem as RootDirectoryEntity).Version);
            _items.Remove(lbVersions.SelectedItem as RootDirectoryEntity);
        }

        private async void btnCompute_Click(object sender, RoutedEventArgs e)
        {
            btnCompare.IsEnabled = false;
            btnCompare.Content = "Computing...";

            string message = await Task.Run(() => GetTotalSpace());
            MessageBox.Show(message);

            btnCompare.Content = "Compute";
            btnCompare.IsEnabled = true;
        }

        private string GetTotalSpace()
        {
            HashSet<FileEntity> uniqueFiles = new HashSet<FileEntity>();
            List<FileEntity> files = new List<FileEntity>();

            foreach (RootDirectoryEntity ent in _items)
            {
                uniqueFiles.UnionWith(ent.GetAllFileEntities(true).OfType<FileEntity>());
                files.AddRange(ent.GetAllFileEntities(true).OfType<FileEntity>());
            }

            double totalSize = files.Select(f => f.Size).Sum() / (1024 * 1024);
            double uniqueSize = uniqueFiles.Select(f => f.Size).Sum() / (1024 * 1024);
            string message = string.Format("Result of using Version Manager with {0} WoT versions:\n\nTotal files: {1:N0}\n" +
                "Total size: {2:0.##} GB\nFiles with Version Manager: {3:N0}\nSize with Version Manager: {4:0.##} GB\n\nYou save: {5:0.##} GB ({6:0} %)",
                _items.Count, files.Count, totalSize / 1024, uniqueFiles.Count, uniqueSize / 1024, (totalSize - uniqueSize) / 1024, 100 - (uniqueSize * 100 / totalSize));
            return message;
        }
    }
}
