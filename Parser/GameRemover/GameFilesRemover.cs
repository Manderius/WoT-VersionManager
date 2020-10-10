using System;
using System.Collections.Generic;
using System.Linq;
using VersionManager.Filesystem;

namespace VersionManager.GameRemover
{
    public class GameFilesRemover
    {
        public static void RemoveFiles(RootDirectoryEntity versionToRemove, List<RootDirectoryEntity> versions, string container, Func<BaseEntity, string> fileToPath)
        {
            HashSet<FileEntity> versionFiles = new HashSet<FileEntity>(versionToRemove.GetAllFileEntities(true).OfType<FileEntity>());
            List<RootDirectoryEntity> otherVersions = versions.ToList();
            otherVersions.Remove(versionToRemove);
            HashSet<FileEntity> otherFiles = new HashSet<FileEntity>();
            otherVersions.ForEach(version => otherFiles.UnionWith(version.GetAllFileEntities(true).OfType<FileEntity>()));
            versionFiles.ExceptWith(otherFiles);

            foreach (FileEntity fe in versionFiles)
            {

            }
        }
    }
}
