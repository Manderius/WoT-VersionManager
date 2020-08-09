using System.Windows.Controls;
using VSUI.Data;
using VSUI.Services;

namespace VSUI.Pages
{

    /// <summary>
    /// Interakční logika pro Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {

        private LocalVersionsService _versionService;

        public Overview(LocalVersionsService versionService)
        {
            InitializeComponent();
            _versionService = versionService;
            
            lbGameVersions.ItemsSource = _versionService.GetLocalVersions();
            lbGameVersions.SelectedIndex = 0;
        }

        private void lbGameVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frmOverviewDetails.Navigate(new OverviewDetails(e.AddedItems[0] as LocalGameVersion));
        }
    }

    

}
