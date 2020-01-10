using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VersionSwitcher_Server.Filesystem
{
    public class DirectoryEntity: BaseEntity
    {
        [XmlArrayItem(ElementName = "File", Type = typeof(FileEntity))]
        [XmlArrayItem(ElementName = "Directory", Type = typeof(DirectoryEntity))]
        public List<BaseEntity> Contents = new List<BaseEntity>();

        public DirectoryEntity(): base()
        {

        }

        public DirectoryEntity(string name): base(name)
        {

        }

        public void Add(BaseEntity file)
        {
            Contents.Add(file);
            file.RelativePath = Path.Combine(RelativePath, Name, file.Name);
        }

        public void Deserialize()
        {
            foreach (BaseEntity file in Contents)
            {
                file.RelativePath = Path.Combine(RelativePath, Name, file.Name);
                if (file is DirectoryEntity de)
                {
                    de.Deserialize();
                }
            }
        }
    }
}
