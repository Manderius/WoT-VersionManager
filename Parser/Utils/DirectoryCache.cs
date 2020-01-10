using System.Collections.Generic;
using System.IO;

namespace VersionSwitcher_Server.Utils
{
    class DirectoryCache
    {
        HashSet<string> _existingPaths = new HashSet<string>();

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
                _existingPaths.Add(dir.FullName);
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
            if (!_existingPaths.Contains(dir))
            {
                Directory.CreateDirectory(dir);
                _existingPaths.Add(dir);
                return false;
            }

            return true;
        }
    }
}
