using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VersionManager.Extraction;
using VersionManager.Filesystem;
using VersionManager.GameGenerator;
using VersionManager.Hashing;
using VersionManager.Parsing;
using VersionManager.Persistence;
using VersionManager.Utils;

namespace VersionManager
{
    public class Main
    {
        public static void Run()
        {
            Stopwatch sw = Stopwatch.StartNew();

            //string dir = @"E:\WoT\Versions\World_of_Tanks - 0.9.4\";
            //DirectoryInfo wot = new DirectoryInfo(dir);
            //RootDirectoryEntity root = new RootDirectoryEntity(Helpers.GetGameVersion(dir));
            //HashProvider sha1 = new SHA1HashProvider();
            //GameDirectoryParser.Parse(wot, root, wot.FullName.Length, true, sha1, new IgnoreList());
            //Console.WriteLine("Total files: {0}", Helpers.TotalFiles(root));
            //Console.WriteLine("Total size: {0} MB", Helpers.TotalSize(root) / (1024 * 1024));
            //new XMLStructureLoader().Serialize(root, @"E:\WoT\serial-094-size.xml");

            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });
            RootDirectoryEntity deser = new XMLStructureLoader().Deserialize(@"E:\WoT\serial-094-size.xml");
            //DirectoryCache cache = DirectoryCache.FromDirectory(@"E:\WoT\Container3");
            string entityToPath(BaseEntity entity) => Helpers.GetFileDirectory(@"E:\WoT\Container3", (entity as FileEntity).Hash);
            //ex.Extract(deser, @"E:\WoT\Versions\World_of_Tanks - 0.9.4\", entityToPath, cache);
            GameDirGenerator.Generate(deser, @"E:\WoT\Versions\Assembled\WoT 0.9.4", @"E:\WoT\Container3", entityToPath);
            sw.Stop();
            MessageBox.Show(string.Format("Elapsed time: {0:hh\\:mm\\:ss}", sw.Elapsed));
            Environment.Exit(0);
        }
    }
}
