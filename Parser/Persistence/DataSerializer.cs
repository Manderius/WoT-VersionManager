using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Persistence
{
    interface DataSerializer
    {
        void Serialize(RootDirectoryEntity baseEntity, string path);
    }
}
