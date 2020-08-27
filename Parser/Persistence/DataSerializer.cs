using VersionManager.Filesystem;

namespace VersionManager.Persistence
{
    interface DataSerializer
    {
        void Serialize(RootDirectoryEntity baseEntity, string path);
    }
}
