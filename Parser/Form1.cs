using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyReplay_Player
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FileClassifier fc = new FileClassifier();
            //fc.Classify(@"E:\WoT\Versions\World_of_Tanks - 0.9.6\", @"E:\WoT\Container\", @"E:\WoT\VersionData\096.xml", "0.9.6");
            GameFolderGenerator gen = new GameFolderGenerator();
            gen.Generate(@"E:\WoT\VersionData\096.xml", @"E:\WoT\Container\", @"E:\WoT\Versions\Assembled\");
            //PathsBuilder pb = new PathsBuilder();
            //pb.CreatePaths(@"E:\WoT\095.xml", @"E:\WoT\Container\", @"E:\WoT\paths.xml");
            //FolderGenerator.Generate(@"E:\WoT\Container", 100000);
            //Dictionary<string, string> replayDetails = ReplayParser.Parse(@"D:\20180925_M5053-two-pen.wotreplay");
            //FolderCompare.Compare(@"E:\WoT\Versions\World_of_Tanks - 0.9.5\", @"E:\WoT\Versions\World_of_Tanks - 0.9.2\");
            //FolderCompare.Compare(@"E:\WoT\Versions\World_of_Tanks - 1.2.0\", @"C:\Program Files (x86)\World_of_Tanks\");
        }
    }
}
