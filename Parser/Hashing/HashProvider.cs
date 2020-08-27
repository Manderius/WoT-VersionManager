using System.IO;

namespace VersionManager.Hashing
{
    abstract class HashProvider
    {
        public abstract string FromStream(Stream str);
    }
}
