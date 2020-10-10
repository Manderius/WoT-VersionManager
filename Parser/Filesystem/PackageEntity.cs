using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VersionManager.Filesystem
{
    [DataContract(Name = "Package", Namespace = "VersionManager.Filesystem")]
    public class PackageEntity : DirectoryEntity
    {
        public PackageEntity() : base()
        {

        }

        public PackageEntity(string name) : base(name)
        {

        }

        public override List<BaseEntity> GetAllFileEntities(bool packageIsDirectory)
        {
            if (packageIsDirectory)
            {
                return base.GetAllFileEntities(packageIsDirectory);
            }
            else
            {
                return new List<BaseEntity>() { this };
            }
        }

    }
}
