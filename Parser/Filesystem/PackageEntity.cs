using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VersionSwitcher_Server.Filesystem
{
    public class PackageEntity: DirectoryEntity
    {
        public PackageEntity(): base()
        {

        }

        public PackageEntity(string name): base(name)
        {

        }

        public override List<BaseEntity> GetAllEntities()
        {
            return new List<BaseEntity>() { this };
        }

    }
}
