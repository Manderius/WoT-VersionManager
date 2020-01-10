using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionSwitcher_Server.Filesystem
{
    public class RootDirectoryEntity : DirectoryEntity
    {
        public string Version;

        public RootDirectoryEntity() : base("")
        {
            RelativePath = "";
        }
    }
}
