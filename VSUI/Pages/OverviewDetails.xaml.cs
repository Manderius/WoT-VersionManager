using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using VersionManagerUI.Data;
using VersionManagerUI.MessageWindows;

namespace VersionManagerUI.Pages
{
    /// <summary>
    /// Interaction logic for OverviewDetails.xaml
    /// </summary>
    public partial class OverviewDetails : Page
    {
        public LocalGameVersion GameDetails { get; set; }

        public OverviewDetails(LocalGameVersion versionItem)
        {
            GameDetails = versionItem;
            InitializeComponent();
            DataContext = this;
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", GameDetails.Path);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Format("This action will delete World of Tanks {0} from your computer. Other versions will not be affected. This process takes about 20 minutes.\nAre you sure you want to delete this game version?", GameDetails.Version);
            MessageWindow confirm = new MessageWindow("Confirm delete", message, MessageWindowButtons.YesNo);
            confirm.ShowDialog();
            if (confirm.Result != MessageWindowResult.Yes)
                return;

        }

        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Format("This action will recreate World of Tanks {0} directory. Use this if you have issues with the imported version. If you have mods installed for this version, create a backup of them first, they will get removed! This process takes about 15 minutes.\nProceed?", GameDetails.Version);
            MessageWindow confirm = new MessageWindow("Confirm action", message, MessageWindowButtons.YesNo);
            confirm.ShowDialog();
            if (confirm.Result != MessageWindowResult.Yes)
                return;
        }
    }
}
