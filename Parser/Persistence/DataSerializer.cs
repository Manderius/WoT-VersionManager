using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Persistence
{
    interface DataSerializer
    {
        void Serialize(RootDirectoryEntity baseEntity, string path);
    }
}
