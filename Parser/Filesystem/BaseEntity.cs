using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VersionManager.Filesystem
{
    [DataContract(Namespace = "VersionManager.Filesystem")]
    [KnownType(typeof(DirectoryEntity))]
    [KnownType(typeof(RootDirectoryEntity))]
    [KnownType(typeof(FileEntity))]
    [KnownType(typeof(PackageEntity))]
    [DebuggerDisplay("{Name}")]
    public class BaseEntity
    {
        [DataMember]
        public string Name;
        [XmlIgnore]
        public string RelativePath;

        public BaseEntity()
        {
        }

        public BaseEntity(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public bool Equals(BaseEntity other)
        {
            return other != null && Name == other.Name && RelativePath == other.RelativePath;
        }

        public override int GetHashCode()
        {
            return new { Name, RelativePath }.GetHashCode();
        }
    }
}
