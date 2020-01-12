using System;
using System.IO;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Utils;

namespace VersionSwitcher_Server.Extraction
{
    class FileExtractor : Extractor
    {
        public override bool CanExtract(BaseEntity entity)
        {
            return entity is FileEntity;
        }

        public override void Extract(string root, BaseEntity entity, Func<BaseEntity, string> entityToDir, DirectoryCache cache)
        {
            string dir = entityToDir(entity);
            if (!cache.CacheDirectory(dir)) {
                string path = Path.Combine(dir, entity.Name);
                File.Copy(Path.Combine(root, entity.RelativePath), path, false);
            }
        }
    }
}
