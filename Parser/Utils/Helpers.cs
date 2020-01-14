using System.IO;

namespace VersionManager.Utils
{
    class Helpers
    {
        public static string GetFileDirectory(string container, string hash)
        {
            string topDir = hash.Substring(0, 3);
            string dir = hash.Substring(3);
            string fullPath = Path.Combine(container, topDir, dir);
            return fullPath;
        }
    }
}
