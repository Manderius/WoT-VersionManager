using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Stopwatch sw = Stopwatch.StartNew();
            //FileClassifier fc = new FileClassifier(@"E:\WoT\Container\");
            //fc.Classify(@"E:\WoT\Versions\World_of_Tanks - 0.7.2\", @"E:\WoT\VersionData\072.xml", "0.7.2");
            //GameFolderGenerator gen = new GameFolderGenerator();
            //gen.Generate(@"E:\WoT\VersionData\096.xml", @"E:\WoT\Container\", @"E:\WoT\Versions\Assembled\");
            //PathsBuilder pb = new PathsBuilder();
            //pb.CreatePaths(@"E:\WoT\095.xml", @"E:\WoT\Container\", @"E:\WoT\paths.xml");
            //FolderGenerator.Generate(@"E:\WoT\Container", 100000);
            //Dictionary<string, string> replayDetails = ReplayParser.Parse(@"D:\20180925_M5053-two-pen.wotreplay");
            //FolderCompare.Compare(@"E:\WoT\Versions\World_of_Tanks - 0.9.5\", @"E:\WoT\Versions\World_of_Tanks - 0.9.2\");
            //FolderCompare.Compare(@"E:\WoT\Versions\World_of_Tanks - 0.9.4", @"E:\WoT\Versions\World_of_Tanks - 0.9.5");
            Serialize();
            Deserialize();
            sw.Stop();
            MessageBox.Show(string.Format("Elapsed time: {0:hh\\:mm\\:ss}", sw.Elapsed));
            Environment.Exit(0);
        }

        public void Serialize()
        {
            RootDirectoryEntity root = new RootDirectoryEntity();
            root.RelativePath = "";
            root.Version = "1.2.3";
            PopulateDirectory(root);

            XmlSerializer serializer = new XmlSerializer(root.GetType());
            using (StreamWriter writer = new StreamWriter(@"E:\WoT\serial.xml"))
            {
                serializer.Serialize(writer, root);
            }

            
        }

        public void Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootDirectoryEntity));

            // Declare an object variable of the type to be deserialized.
            RootDirectoryEntity root;

            using (Stream reader = new FileStream(@"E:\WoT\serial.xml", FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                root = (RootDirectoryEntity)serializer.Deserialize(reader);
            }
            root.RelativePath = "";
            root.Deserialize();
        }

        private void PopulateDirectory(DirectoryEntity dir, int depth = 3)
        {
            Random ran = new Random();
            for (int i = 0; i < 3; i++)
            {
                if (ran.Next(10) % 2 == 0)
                {
                    DirectoryEntity de = new DirectoryEntity("Dir" + i);
                    dir.Add(de);
                    if (depth > 0 && ran.Next(10) % 2 == 0)
                    {
                        PopulateDirectory(de, depth - 1);
                    }
                }
                else
                {
                    FileEntity file = new FileEntity("File" + i);
                    file.Hash = "abcd";
                    dir.Add(file);
                }
            }
        }
    }
}
