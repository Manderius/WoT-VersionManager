using System.Diagnostics;
using System.Xml.Serialization;

namespace VersionManager.Filesystem
{
    [DebuggerDisplay("{Name}")]
    public class BaseEntity
    {
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
    }
}
