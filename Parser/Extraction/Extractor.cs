using System;
using VersionManager.Filesystem;
using VersionManager.Utils;

namespace VersionManager.Extraction
{
    abstract class Extractor
    {
        public abstract bool CanExtract(BaseEntity entity);
        public abstract void Extract(string root, BaseEntity entity, Func<BaseEntity, string> entityToDir, DirectoryCache cache);
    }
}
