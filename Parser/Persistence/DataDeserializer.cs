using VersionManager.Filesystem;

namespace VersionManager.Persistence
{
    interface DataDeserializer
    {
        RootDirectoryEntity Deserialize(string path);
    }
}
