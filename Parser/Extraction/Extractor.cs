﻿using System;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Utils;

namespace VersionSwitcher_Server.Extraction
{
    abstract class Extractor
    {
        public abstract bool CanExtract(BaseEntity entity);
        public abstract void Extract(string root, BaseEntity entity, Func<BaseEntity, string> entityToDir, DirectoryCache cache);
    }
}