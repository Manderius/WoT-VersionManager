using VersionManager.Filesystem;

namespace VersionManager.Persistence
{
    public class RootDirectoryEntityIO: DataContractXMLLoader
    {
        public RootDirectoryEntity Deserialize(string path)
        {
            RootDirectoryEntity root = base.Deserialize<RootDirectoryEntity>(path);
            root.RelativePath = "";
            root.BuildPaths();
            return root;
        }
    }
}
