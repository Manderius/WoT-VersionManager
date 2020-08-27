using System.Windows.Controls;

namespace VSUI.Pages
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Page
    {
        public Download()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string link = ((sender as System.Windows.Documents.Hyperlink).Inlines.FirstInline as System.Windows.Documents.Run).Text;
            System.Diagnostics.Process.Start(link);
        }
    }
}
