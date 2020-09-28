using VersionManager.Filesystem;

namespace VersionManager.Persistence
{
    interface DataSerializer<T>
    {
        void Serialize(T data, string path);
    }
}
