using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.Tutorial.Pages
{
    /// <summary>
    /// Interaction logic for HowItWorks.xaml
    /// </summary>
    public partial class HowItWorks : Page
    {
        private IPagination _pagination { get; set; }

        public HowItWorks(IPagination pagination)
        {
            _pagination = pagination;
            InitializeComponent();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _pagination.PreviousPage();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _pagination.NextPage();
        }
    }
}
