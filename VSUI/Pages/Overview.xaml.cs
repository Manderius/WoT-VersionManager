using System.Windows.Controls;
using VersionManagerUI.Data;
using VersionManagerUI.Services;

namespace VersionManagerUI.Pages
{

    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {

        private ManagedVersionsService _versionService;

        public Overview(ManagedVersionsService versionService)
        {
            InitializeComponent();
            _versionService = versionService;
            frmOverviewDetails.Navigate(new OverviewEmpty());

            lbGameVersions.ItemsSource = _versionService.GetManagedVersions();
            lbGameVersions.Items.SortDescriptions.Clear();
            lbGameVersions.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("LocalVersion.Version", System.ComponentModel.ListSortDirection.Descending));
            lbGameVersions.SelectedIndex = 0;
        }

        private void lbGameVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frmOverviewDetails.Navigate(new OverviewDetails((e.AddedItems[0] as ManagedGameVersion).LocalVersion));
        }
    }

    

}
