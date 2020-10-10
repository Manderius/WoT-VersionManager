using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace VersionManager.Utils
{
    [DataContract]
    public class DirectoryCache
    {
        [DataMember]
        private HashSet<string> ExistingPaths = new HashSet<string>();

        public DirectoryCache() { }

        public DirectoryCache(IEnumerable<string> source) {
            ExistingPaths = new HashSet<string>(source);
        }

        public static DirectoryCache FromDirectory(string path)
        {
            DirectoryCache cache = new DirectoryCache();
            cache.LoadExistingDirs(path);
            return cache;
        }

        private void LoadExistingDirs(string root)
        {
            foreach (DirectoryInfo dir in new DirectoryInfo(root).EnumerateDirectories())
            {
                ExistingPaths.Add(dir.FullName);
                LoadExistingDirs(dir.FullName);
            }
        }

        /// <summary>
        /// Caches directory as created for faster processing, creating if it doesn't exist
        /// It's faster than checking the filesystem with File.Exists
        /// </summary>
        /// <param name="dir">Absolute path to directory</param>
        /// <returns>True if directory already was in cache, false otherwise</returns>
        public bool CacheDirectory(string dir)
        {
            if (!ExistingPaths.Contains(dir))
            {
                Directory.CreateDirectory(dir);
                ExistingPaths.Add(dir);
                return false;
            }

            return true;
        }
    }
}
