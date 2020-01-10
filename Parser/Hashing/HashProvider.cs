using System.IO;

namespace VersionSwitcher_Server.Hashing
{
    abstract class HashProvider
    {
        public abstract string FromStream(Stream str);
    }
}
