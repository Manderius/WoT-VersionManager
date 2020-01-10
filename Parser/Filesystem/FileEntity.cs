using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionSwitcher_Server.Filesystem
{
    public class FileEntity : BaseEntity
    {
        public string Hash;

        public FileEntity()
        {
        }

        public FileEntity(string name) : base(name)
        {
        }
    }
}
