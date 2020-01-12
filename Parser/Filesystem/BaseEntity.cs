using System.Diagnostics;
using System.Xml.Serialization;

namespace VersionSwitcher_Server.Filesystem
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
