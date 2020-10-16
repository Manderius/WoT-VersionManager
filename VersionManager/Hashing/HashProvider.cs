using System.IO;

namespace VersionManager.Hashing
{
    abstract public class HashProvider
    {
        public abstract string FromStream(Stream str);
    }
}
