using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Extraction
{
    abstract class Extractor
    {
        public abstract bool CanExtract(BaseEntity entity);
        public abstract void Extract(string root, BaseEntity entity, Func<BaseEntity, string> entityToDir, ExtractionCache cache);
    }
}
