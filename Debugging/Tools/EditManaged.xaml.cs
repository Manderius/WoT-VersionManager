using Debugging.Common;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VersionManager.Persistence;
using VersionManager.Utils;
using VersionManagerUI.Data;

namespace Debugging.Tools
{
    /// <summary>
    /// Interaction logic for EditManaged.xaml
    /// </summary>
    public partial class EditManaged : Page
    {
        public ButtonEnabledClass ButtonEnabler = new ButtonEnabledClass();
        public ManagedVersionCollection ManagedVersions { get; set; }
        private string ManagedVersionsPath { get; set; }

        public EditManaged()
        {
            InitializeComponent();
            DataContext = this;
            btnAddVersion.DataContext = btnRemove.DataContext = btnSave.DataContext = ButtonEnabler;
        }

        private void btnAddVersion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You will now select directory containing your WoT game version.", "Select directory");
            string dir = Utils.SelectDirectory();
            if (dir == null)
                return;
            string version = Helpers.GetGameVersion(dir);
            MessageBox.Show("You will now select Version XML file for your selected WoT game version. If you don't have it, generate it first through \"Create version data file\"", "Select version XML");
            string xml = Utils.SelectXML(version.Replace(".", "_"));
            if (xml == null)
                return;

            ManagedVersions.Add(new ManagedGameVersion(dir, version, xml));
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            ManagedGameVersion version = lbVersions.SelectedItem as ManagedGameVersion;
            if (version != null)
                ManagedVersions.Remove(version);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("This will overwrite your previous Managed Versions file. Save?", "Confirm", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                SaveManagedVersions();
            }
        }
        private void btnBrowseManagedFile_Click(object sender, RoutedEventArgs e)
        {
            string path = Utils.SelectXML("ManagedVersions");
            if (path != null)
            {
                ManagedVersionsPath = path;
                LoadManagedVersions();
                ButtonEnabler.IsFileSelected = true;
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

        public class ButtonEnabledClass : INotifyPropertyChanged
        {
            private bool _isFileSelected = false;
            public bool IsFileSelected
            {
                get => _isFileSelected;
                set
                {
                    _isFileSelected = value;
                    OnPropertyChanged("IsFileSelected");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string info)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
