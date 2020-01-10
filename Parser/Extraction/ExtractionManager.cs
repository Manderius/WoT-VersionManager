﻿using System;
using System.Collections.Generic;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Utils;

namespace VersionSwitcher_Server.Extraction
{
    class ExtractionManager
    {
        IList<Extractor> _extractors;
        public ExtractionManager(IList<Extractor> extractors)
        {
            _extractors = extractors;
        }

        public void Extract(DirectoryEntity entity, string root, Func<BaseEntity, string> fileToPath, DirectoryCache cache)
        {
            foreach (BaseEntity ent in entity.GetAllEntities())
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