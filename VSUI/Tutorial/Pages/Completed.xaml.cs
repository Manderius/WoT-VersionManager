using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.Tutorial.Pages
{
    /// <summary>
    /// Interaction logic for Completed.xaml
    /// </summary>
    public partial class Completed : Page
    {
        private IPagination _pagination { get; set; }

        public Completed(IPagination pagination)
        {
            _pagination = pagination;
            InitializeComponent();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            _pagination.NextPage();
        }
    }
}
