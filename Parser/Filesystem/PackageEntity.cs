using System.Collections.Generic;

namespace VersionManager.Filesystem
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
