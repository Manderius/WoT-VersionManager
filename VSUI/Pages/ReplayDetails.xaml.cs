using System.Windows.Controls;
using VersionManager.Replay;
using VSUI.Services;

namespace VSUI.Pages
{
    /// <summary>
    /// Interakční logika pro ReplayDetails.xaml
    /// </summary>
    public partial class ReplayDetails : Page
    {
        public Replay ReplayData { get; set; }
        public string Path { get; set; }
        private LocalVersionsService _localVersionService;

        public ReplayDetails(Replay replay, string path, LocalVersionsService localVersionsService)
        {
            ReplayData = replay;
            Path = path;
            DataContext = this;
            _localVersionService = localVersionsService;
            InitializeComponent();

            if (ReplayData.Version != "Unknown")
            {
                versionPick.Visibility = System.Windows.Visibility.Hidden;  
            }


        }
    }
}
