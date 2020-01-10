using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VersionSwitcher_Server.Filesystem
{
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
