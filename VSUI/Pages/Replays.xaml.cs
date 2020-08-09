using System.Windows.Controls;
using System.Windows.Forms;
using VersionManager.Replay;
using VSUI.Services;

namespace VSUI.Pages
{
    /// <summary>
    /// Interakční logika pro Replay.xaml
    /// </summary>
    public partial class Replays : Page
    {
        private LocalVersionsService _localVersionsService;
        public Replays(LocalVersionsService localVersionsService)
        {
            _localVersionsService = localVersionsService;
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void btnBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Replay files (*.wotreplay)|*.wotreplay";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Replay replay = ReplayParser.Parse(openFileDialog.FileName);
                    frmReplayDetails.Navigate(new ReplayDetails(replay, openFileDialog.FileName, _localVersionsService));
                    btnPlay.IsEnabled = true;
                }
            }

        }
    }
}
