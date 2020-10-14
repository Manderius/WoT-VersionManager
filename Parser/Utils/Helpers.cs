using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using VersionManager.Filesystem;
using VersionManager.Hashing;
using VersionManager.Parsing;

namespace VersionManager.Utils
{
    public class Helpers
    {
        public static Func<BaseEntity, string> EntityToPath(string container) {
            return entity => GetFileDirectory(container, (entity as FileEntity).Hash);
        }

        public static string GetFileDirectory(string container, string hash)
        {
            string topDir = hash.Substring(0, 3);
            string dir = hash.Substring(3);
            string fullPath = Path.Combine(container, topDir, dir);
            return fullPath;
        }

        public static int TotalFiles(DirectoryEntity dir)
        {
            int total = dir.Contents.OfType<FileEntity>().Count();
            return total + dir.Contents.OfType<DirectoryEntity>().Select(d => TotalFiles(d)).Sum();
        }

        public static long TotalSize(DirectoryEntity dir)
        {
            return dir.GetAllFileEntities(true).Cast<FileEntity>().Aggregate(0L, (x, y) => x + y.Size);
        }

        public static string GetGameVersion(string gameDir)
        {
            string versionXml = Path.Combine(gameDir, "version.xml");
            if (!Directory.Exists(gameDir) || !File.Exists(versionXml))
                return null;

            XElement xml = XElement.Parse(File.ReadAllText(versionXml));
            string versionText = xml.Element("version").Value.Trim();

            Regex regex = new Regex(@"v\.([0-9\.]+)\s");
            Match match = regex.Match(versionText);

            return match.Groups[1].Value;
        }

        public static RootDirectoryEntity CreateRootEntityFromDirectory(string path, bool withHash = true, IProgress<int> progress = null)
        {
            DirectoryInfo wot = new DirectoryInfo(path);
            RootDirectoryEntity root = new RootDirectoryEntity(Helpers.GetGameVersion(path));
            HashProvider hp = withHash ? new SHA1HashProvider() : null;
            GameDirectoryParser.Parse(wot, root, wot.FullName.Length, hp, IgnoreList.FromEnumerable(File.ReadAllLines("ignored.txt")), progress);
            return root;
        }
    }
}
