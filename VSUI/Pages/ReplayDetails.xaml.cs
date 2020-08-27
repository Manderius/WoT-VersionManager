using System.IO;
using System.Windows.Controls;
using VersionManager.Replay;

namespace VSUI.Pages
{
    /// <summary>
    /// Interaction logic for ReplayDetails.xaml
    /// </summary>
    public partial class ReplayDetails : Page
    {
        public Replay ReplayData { get; private set; }
        public string FileName { get; private set; }

        public ReplayDetails(Replay replay)
        {
            ReplayData = replay;
            DataContext = this;
            FileName = Path.GetFileNameWithoutExtension(ReplayData.Path);
            InitializeComponent();
        }
    }
}
