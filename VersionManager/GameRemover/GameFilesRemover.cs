using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VersionManager.Filesystem;
using VersionManager.Utils;

namespace VersionManager.GameRemover
{
    public class GameFilesRemover
    {
        /// <summary>
        /// Deletes target version's files from container if they're not used by other versions.
        /// </summary>
        /// <param name="versionToRemove"></param>
        /// <param name="allVersions"></param>
        /// <param name="fileToPath"></param>
        /// <param name="cache"></param>
        /// <param name="progress"></param>
        public static void RemoveFiles(RootDirectoryEntity versionToRemove, List<RootDirectoryEntity> allVersions, Func<BaseEntity, string> fileToPath, DirectoryCache cache, IProgress<int> progress)
        {
            RemoveFiles(new HashSet<FileEntity>(versionToRemove.GetAllFileEntities(true).OfType<FileEntity>()),
                allVersions.Except(new List<RootDirectoryEntity> { versionToRemove }).ToList(),
                fileToPath,
                cache,
                progress);
        }

        /// <summary>
        /// Deletes files from container if they're not used by other versions.
        /// </summary>
        /// <param name="filesToRemove"></param>
        /// <param name="otherVersions"></param>
        /// <param name="fileToPath"></param>
        /// <param name="cache"></param>
        /// <param name="progress"></param>
        public static void RemoveFiles(HashSet<FileEntity> filesToRemove, List<RootDirectoryEntity> otherVersions, Func<BaseEntity, string> fileToPath, DirectoryCache cache, IProgress<int> progress)
        {
            filesToRemove = new HashSet<FileEntity>(filesToRemove);
            HashSet<FileEntity> otherVersionsFiles = new HashSet<FileEntity>();
            otherVersions.ForEach(version => otherVersionsFiles.UnionWith(version.GetAllFileEntities(true).OfType<FileEntity>()));
            filesToRemove.ExceptWith(otherVersionsFiles);
            RemoveFilesInner(filesToRemove, fileToPath, cache, progress);
        }

        private static void RemoveFilesInner(HashSet<FileEntity> files, Func<BaseEntity, string> fileToPath, DirectoryCache cache, IProgress<int> progress)
        {
            int total = files.Count;
            float done = 0;

            foreach (FileEntity fe in files)
            {
                string path = fileToPath(fe);
                if (cache.DeleteDirectoryFromCache(path))
                {
                    try { Directory.Delete(path, true); } catch (Exception ex) { }

                    progress?.Report((int)(100 * ++done / total));
                }
            }
        }
    }
}
