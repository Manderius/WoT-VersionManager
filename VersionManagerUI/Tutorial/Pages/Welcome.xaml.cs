using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.Tutorial.Pages
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Page
    {
        private IPagination _pagination { get; set; }

        public Welcome(IPagination pagination)
        {
            _pagination = pagination;
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _pagination.NextPage();
        }
    }
}
