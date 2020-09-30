using System;
using System.IO;
using VersionManager.Filesystem;
using VersionManager.Utils;

namespace VersionManager.Extraction
{
    public class FileExtractor : Extractor
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
