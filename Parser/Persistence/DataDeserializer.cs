using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Persistence
{
    interface DataDeserializer
    {
        RootDirectoryEntity Deserialize(string path);
    }
}
