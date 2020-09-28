using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Filesystem;
using VersionManager.GameRemover;
using VersionManager.Persistence;
using VersionManager.Utils;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for DeleteVersion.xaml
    /// </summary>
    public partial class DeleteVersion : Page
    {

        public ObservableCollection<RootDirectoryEntity> _items;

        public DeleteVersion()
        {
            InitializeComponent();
            _items = new ObservableCollection<RootDirectoryEntity>();
            lbVersions.ItemsSource = _items;
        }

        private string SelectDirectory(string title = "Select a directory")
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog(title))
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }
            return null;
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
                    if (!_items.Contains(ent))
                    {
                        _items.Add(ent);
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveGameFiles();
        }

        private async void RemoveGameFiles()
        {
            btnRemove.IsEnabled = false;
            btnRemove.Content = "Removing...";

            RootDirectoryEntity selected = lbVersions.SelectedItem as RootDirectoryEntity;
            if (selected == null)
                return;

            string container = txtContainer.Text;
            if (container == null || !Directory.Exists(container))
            {
                MessageBox.Show("Please select container directory.", "Error");
                return;
            }

            MessageBoxResult result = MessageBox.Show("This process will remove ALL files from the selected version and delete the game directory. Proceed?", "Warning", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            MessageBox.Show("Please select the selected game's directory, if it exists.", "Info");
            string gameDir = GetGameDirectory(selected.Version);
            if (gameDir != null)
            {
                await Task.Run(() => Directory.Delete(gameDir, true));
            }

            string entityToPath(BaseEntity entity) => Helpers.GetFileDirectory(container, (entity as FileEntity).Hash);
            await Task.Run(() => GameFilesRemover.RemoveFiles(selected, _items.ToList(), container, entityToPath));
            _items.Remove(selected);

            btnRemove.IsEnabled = true;
            btnRemove.Content = "Remove version";
        }

        private string GetGameDirectory(string selectedVersion)
        {

            string dir = SelectDirectory();
            if (Directory.Exists(dir))
            {
                string version = Helpers.GetGameVersion(dir);
                if (version != selectedVersion)
                {
                    MessageBox.Show(string.Format("You wanted to delete version {0}, but selected folder with {1}. Please try again or cancel the folder picker dialog.", selectedVersion, version), "Error");
                    return GetGameDirectory(selectedVersion);
                }

                return dir;
            }
            else
            {
                MessageBox.Show("You didn't select a valid directory. Version files will be marked for removal now. You will have to remove the game directory manually later.", "Info");
            }
            return null;
        }

        private void btnBrowseContainer_Click(object sender, RoutedEventArgs e)
        {
            txtContainer.Text = SelectDirectory("Select container directory");
        }
    }
}
