using System;
using System.IO;
using System.IO.Compression;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Utils;

namespace VersionSwitcher_Server.Extraction
{
    class PackageExtractor : Extractor
    {
        public override bool CanExtract(BaseEntity entity)
        {
            return entity is PackageEntity;
        }

        public override void Extract(string root, BaseEntity entity, Func<BaseEntity, string> entityToDir, DirectoryCache cache)
        {
            PackageEntity package = entity as PackageEntity;
            using (ZipArchive arch = ZipFile.OpenRead(Path.Combine(root, entity.RelativePath)))
            {
                foreach (var entry in arch.Entries)
                {
                    BaseEntity ent = package.GetEntityFromRelativePath(entry.FullName, false);
                    if (ent is null)
                    {
                        throw new InvalidOperationException("Couldn't find entity in structure: " + entry.FullName + " of package: " + entity.RelativePath);
                    }
                    if (ent is FileEntity)
                    {
                        string dir = entityToDir(ent);
                        if (!cache.CacheDirectory(dir))
                        {
                            entry.ExtractToFile(Path.Combine(dir, ent.Name), false);
                        }
                    }
                }
            }
        }
    }
}
