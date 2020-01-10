using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Hashing
{
    class Hashing
    {
        public static void ComputeHashes(DirectoryEntity root, HashProvider provider)
        {
            foreach (FileEntity file in root.Contents.OfType<FileEntity>())
            {
                file.Hash = provider.FromStream(file.Contents()) + file.RelativePath.GetHashCode();
            }

            foreach (DirectoryEntity directory in root.Contents.OfType<DirectoryEntity>())
            {
                ComputeHashes(directory, provider);
            }
        }
    }
}
