using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VersionSwitcher_Server
{
    public partial class Form1 : Form
    {
        string replay = "";
        string version = "";
        readonly string XMLLocation = @"E:\WoT\VersionData";
        readonly string ContainerLocation = @"E:\WoT\Container";
        string DestDir = @"E:\WoT\Versions\Assembled";

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            tDestDir.Text = DestDir;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // Okay
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            DestDir = tDestDir.Text;

            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (fileList[0].EndsWith(".wotreplay")) {
                replay = fileList[0];
                tReplayFile.Text = replay;
                Dictionary<string, string> details = ReplayParser.Parse(replay);
                lReplayVersion.Text = details["version"];
                version = details["version"];
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lStatus.Text = "Generating WoT folder...";
            button2.Enabled = false;
            Application.DoEvents();
            string shortVer = version.Replace(".", "");
            string versionXMLPath = Path.Combine(XMLLocation, shortVer + ".xml");

            if (!File.Exists(versionXMLPath))
            {
                MessageBox.Show("Info for " + version + " doesn't exist.");
                return;
            }

            new GameFolderGenerator().Generate(versionXMLPath, ContainerLocation, DestDir);
            lStatus.Text = "Playing...";
            Application.DoEvents();

            Process p = new Process();
            p.StartInfo.FileName = Path.Combine(DestDir, "WorldOfTanks.exe");
            p.StartInfo.Arguments = replay;
            p.Start();

            button2.Enabled = true;
        }
    }
}
