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

        public void Extract(DirectoryEntity entity, string root, Func<BaseEntity, string> fileToPath, DirectoryCache cache)
        {
            foreach (BaseEntity ent in entity.GetAllFileEntities())
            {
                foreach (Extractor ex in _extractors)
                {
                    if (ex.CanExtract(ent))
                    {
                        ex.Extract(root, ent, fileToPath, cache);
                        break;
                    }
                }
            }
        }
    }
}
