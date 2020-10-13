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
        public static void RemoveFiles(RootDirectoryEntity versionToRemove, List<RootDirectoryEntity> versions, string container, Func<BaseEntity, string> fileToPath, DirectoryCache cache, IProgress<int> progress)
        {
            HashSet<FileEntity> versionFiles = new HashSet<FileEntity>(versionToRemove.GetAllFileEntities(true).OfType<FileEntity>());
            List<RootDirectoryEntity> otherVersions = versions.ToList();
            otherVersions.Remove(versionToRemove);
            HashSet<FileEntity> otherFiles = new HashSet<FileEntity>();
            otherVersions.ForEach(version => otherFiles.UnionWith(version.GetAllFileEntities(true).OfType<FileEntity>()));
            versionFiles.ExceptWith(otherFiles);
            int total = versionFiles.Count;
            float done = 0;

            foreach (FileEntity fe in versionFiles)
            {
                string path = fileToPath(fe);
                if (cache.DeleteDirectoryFromCache(path))
                {
                    try { Directory.Delete(path, true);  } catch (Exception ex) { }
                    
                    progress?.Report((int) (100 * ++done / total));
                }
            }
        }
    }
}
