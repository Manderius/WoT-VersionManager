using VersionManager.Filesystem;

namespace VersionManager.Persistence
{
    public class RootDirectoryEntityIO: XMLStructureLoader<RootDirectoryEntity>
    {
        public override RootDirectoryEntity Deserialize(string path)
        {
            RootDirectoryEntity root = base.Deserialize(path);
            root.BuildPaths();
            return root;
        }
    }
}
