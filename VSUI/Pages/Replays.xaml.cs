using System.Windows.Controls;

namespace VSUI.Pages
{
    /// <summary>
    /// Interakční logika pro Replay.xaml
    /// </summary>
    public partial class Replays : Page
    {
        public Replays()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnPlay.IsEnabled = false;
        }
    }
}
