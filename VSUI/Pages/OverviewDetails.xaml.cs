using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using VersionManagerUI.Data;

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
    }
}
