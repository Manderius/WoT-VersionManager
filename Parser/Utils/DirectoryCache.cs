using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace VersionManager.Utils
{
    [DataContract(Namespace = "VersionManager.DirectoryCache")]
    public class DirectoryCache
    {
        [DataMember(Name = "ExistingPaths")]
        private HashSet<string> _existingPaths = new HashSet<string>();

        public string ContainerPath { get; set; }

        public DirectoryCache() { }

        public DirectoryCache(IEnumerable<string> source) {
            _existingPaths = new HashSet<string>(source);
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
            string relativePath = (dir.StartsWith(ContainerPath)) ? dir.Substring(ContainerPath.Length).TrimStart('\\') : dir;
            if (!_existingPaths.Contains(relativePath))
            {
                Directory.CreateDirectory(dir);
                _existingPaths.Add(relativePath);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete directory from cache if present
        /// </summary>
        /// <param name="dir">Path to directory</param>
        /// <returns>True if directory was in cache, false otherwise</returns>
        public bool DeleteDirectoryFromCache(string dir)
        {
            string relativePath = (dir.StartsWith(ContainerPath)) ? dir.Substring(ContainerPath.Length).TrimStart('\\') : dir;
            if (_existingPaths.Contains(relativePath))
            {
                _existingPaths.Remove(relativePath);
                return true;
            }
            return false;
        }
    }
}
