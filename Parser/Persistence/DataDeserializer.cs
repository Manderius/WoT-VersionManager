using VersionManager.Filesystem;

namespace VersionManager.Persistence
{
    interface DataDeserializer<T>
    {
        T Deserialize(string path);
    }
}
