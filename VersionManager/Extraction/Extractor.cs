﻿using System;
using VersionManager.Filesystem;
using VersionManager.Utils;

namespace VersionManager.Extraction
{
    public abstract class Extractor
    {
        public abstract bool CanExtract(BaseEntity entity);
        public abstract void Extract(string root, BaseEntity entity, Func<BaseEntity, string> entityToDir, DirectoryCache cache, IProgress<int> progress);
    }
}
