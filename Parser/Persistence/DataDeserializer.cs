using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Persistence
{
    interface DataDeserializer
    {
        RootDirectoryEntity Deserialize(string path);
    }
}
