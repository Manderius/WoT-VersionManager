using System.Windows.Controls;
using VSUI.Data;
using VSUI.Services;

namespace VSUI.Pages
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
            
            lbGameVersions.ItemsSource = _versionService.GetManagedVersions();
            lbGameVersions.SelectedIndex = 0;
        }

        private void lbGameVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frmOverviewDetails.Navigate(new OverviewDetails(e.AddedItems[0] as LocalGameVersion));
        }
    }

    

}
