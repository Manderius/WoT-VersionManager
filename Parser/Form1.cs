using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Hashing;
using VersionSwitcher_Server.Persistence;
using VersionSwitcher_Server.Extraction;

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

            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });

            //DirectoryInfo wot = new DirectoryInfo(@"E:\WoT\Versions\World_of_Tanks - 0.9.4\");
            //RootDirectoryEntity root = new RootDirectoryEntity();
            //HashProvider sha1 = new SHA1HashProvider();
            //new FileClassifier("", sha1, new SortedSet<string>()).Parse(wot, root, wot.FullName.Length, true);
            ////Console.WriteLine("Total files: {0}", TotalFiles(root));
            //Hashing.Hashing.ComputeHashes(root, sha1);
            //new XMLStructureLoader().Serialize(root, @"E:\WoT\serial.xml");

            RootDirectoryEntity deser = new XMLStructureLoader().Deserialize(@"E:\WoT\serial-hash.xml");
            ExtractionCache cache = ExtractionCache.FromDirectory(@"E:\WoT\Container3");
            Func<BaseEntity, string> entityToPath = (entity) => FileClassifier.GetFileDirectory(@"E:\WoT\Container3", (entity as FileEntity).Hash);
            ex.Extract(deser, @"E:\WoT\Versions\World_of_Tanks - 0.9.4\", entityToPath, cache);
            sw.Stop();
            MessageBox.Show(string.Format("Elapsed time: {0:hh\\:mm\\:ss}", sw.Elapsed));
            Environment.Exit(0);
        }

        public int TotalFiles(DirectoryEntity dir)
        {
            int total = dir.Contents.OfType<FileEntity>().Count();
            return total + dir.Contents.OfType<DirectoryEntity>().Select(d => TotalFiles(d)).Sum();
        }
    }
}
