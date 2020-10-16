using System.Diagnostics;
using System.Windows.Controls;

namespace VersionManagerUI.Pages
{
    public partial class Help : Page
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
