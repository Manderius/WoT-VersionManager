using System.Runtime.Serialization;

namespace VersionManager.Filesystem
{
    [DataContract(Name = "RootDirectory", Namespace = "VersionManager.Filesystem")]
    public class RootDirectoryEntity : DirectoryEntity
    {
        [DataMember]
        public string Version { get; set; }

        public RootDirectoryEntity() : base("")
        {
            RelativePath = "";
        }

        public RootDirectoryEntity(string version) : base("")
        {
            RelativePath = "";
            Version = version;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RootDirectoryEntity);
        }

        public bool Equals(RootDirectoryEntity ent)
        {
            return ent != null && ent.Version == Version;
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode() + base.GetHashCode();
        }
    }
}
