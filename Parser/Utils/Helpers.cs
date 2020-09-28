using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using VersionManager.Filesystem;

namespace VersionManager.Utils
{
    public class Helpers
    {
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
    }
}
