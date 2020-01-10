using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VersionSwitcher_Server.Filesystem
{
    public class DirectoryEntity : BaseEntity
    {
        [XmlArrayItem(ElementName = "File", Type = typeof(FileEntity))]
        [XmlArrayItem(ElementName = "Directory", Type = typeof(DirectoryEntity))]
        public List<BaseEntity> Contents = new List<BaseEntity>();

        public DirectoryEntity() : base()
        {

        }

        public DirectoryEntity(string name) : base(name)
        {

        }

        public void Add(BaseEntity file)
        {
            Contents.Add(file);
            file.RelativePath = Path.Combine(RelativePath, file.Name);
        }

        public void Deserialize()
        {
            foreach (BaseEntity file in Contents)
            {
                file.RelativePath = Path.Combine(RelativePath, file.Name);
                if (file is DirectoryEntity de)
                {
                    de.Deserialize();
                }
            }
        }

        public BaseEntity GetEntityFromRelativePath(string path, bool create)
        {
            if (path == "")
                return this;

            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            string[] parts = path.Replace("\\", "/").Split('/');
            return GetEntityFromRelativePath(parts, create);
        }

        public BaseEntity GetEntityFromRelativePath(string[] parts, bool create)
        {
            BaseEntity match = GetByName(parts[0], create);
            if (parts.Length == 1)
            {
                return match;
            }
            else if (match is DirectoryEntity dir)
            {
                return dir.GetEntityFromRelativePath(parts.Skip(1).ToArray(), create);
            }
            else
            {
                return null;
            }
        }

        private BaseEntity GetByName(string name, bool create)
        {
            IEnumerable<BaseEntity> entities = Contents.Where(ent => ent.Name == name);
            if (entities.Any())
            {
                return entities.First();
            }
            else if (create)
            {
                DirectoryEntity dir = new DirectoryEntity(name);
                Add(dir);
                return dir;
            }
            else
            {
                return null;
            }
        }
    }
}
