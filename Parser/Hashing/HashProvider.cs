using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionSwitcher_Server.Hashing
{
    abstract class HashProvider
    {
        public abstract string FromStream(Stream str);
    }
}
