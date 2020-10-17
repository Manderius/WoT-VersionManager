using System;
using System.Collections.Generic;
using VersionManager.Filesystem;
using VersionManager.Utils;

namespace VersionManager.Extraction
{
    public class ExtractionManager
    {
        IList<Extractor> _extractors;
        public ExtractionManager(IList<Extractor> extractors)
        {
            _extractors = extractors;
        }

        public void Extract(DirectoryEntity entity, string root, Func<BaseEntity, string> fileToPath, DirectoryCache cache, IProgress<int> progress)
        {
            int totalFiles = entity.GetAllFileEntities(true).Count;
            int processed = 0;
            Progress<int> sumProgress = new Progress<int>(prog =>
            {
                int percent = ++processed * 100 / totalFiles;
                progress?.Report(percent);
            });

            ExtractInner(entity, root, fileToPath, cache, (progress != null) ? sumProgress : null);
        }

        private void ExtractInner(DirectoryEntity entity, string root, Func<BaseEntity, string> fileToPath, DirectoryCache cache, IProgress<int> progress)
        {
            foreach (BaseEntity ent in entity.GetAllFileEntities())
            {
                foreach (Extractor ex in _extractors)
                {
                    if (ex.CanExtract(ent))
                    {
                        ex.Extract(root, ent, fileToPath, cache, progress);
                        break;
                    }
                }
            }
        }
    }
}
