namespace VersionManager.Filesystem
{
    public class RootDirectoryEntity : DirectoryEntity
    {
        public string Version;

        public RootDirectoryEntity() : base("")
        {
            RelativePath = "";
        }

        public RootDirectoryEntity(string version) : base("")
        {
            RelativePath = "";
            Version = version;
        }
    }
}
