using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionSwitcher_Server.Extraction
{
    class ExtractionCache
    {
        HashSet<string> _existingPaths = new HashSet<string>();

        public static ExtractionCache FromDirectory(string path)
        {
            ExtractionCache cache = new ExtractionCache();
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
