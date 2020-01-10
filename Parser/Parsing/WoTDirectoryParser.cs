using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Hashing;

namespace VersionSwitcher_Server.Parsing
{
    class WoTDirectoryParser
    {
        public static void Parse(DirectoryInfo directory, DirectoryEntity parent, int prefixLength, bool computeHash, HashProvider hashProvider, IgnoreList ignoreList)
        {
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                string relativePath = file.FullName.Substring(prefixLength);

                if (ignoreList.IsIgnored(relativePath))
                {
                    continue;
                }

                if (file.Extension == ".pkg")
                {
                    ParsePackage(file, parent, computeHash, hashProvider);
                }
                else
                {
                    FileEntity fileEnt = computeHash ? new FileEntity(file.Name, hashProvider.FromStream(new FileStream(file.FullName, FileMode.Open))) : new FileEntity(file.Name);
                    parent.Add(fileEnt);
                }
            }

            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                DirectoryEntity child = new DirectoryEntity(dir.Name);
                parent.Add(child);
                Parse(dir, child, prefixLength, computeHash, hashProvider, ignoreList);
            }
        }

        private static void ParsePackage(FileInfo package, DirectoryEntity parent, bool computeHash, HashProvider hashProvider)
        {
            PackageEntity root = new PackageEntity(package.Name);
            parent.Add(root);

            using (ZipArchive archive = ZipFile.OpenRead(package.FullName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Directory
                    if (entry.Length == 0)
                        continue;

                    string relativePath = entry.FullName.Substring(0, entry.FullName.Length - entry.Name.Length);
                    FileEntity file = (computeHash) ? new FileEntity(entry.Name, hashProvider.FromStream(entry.Open())) : new FileEntity(entry.Name);
                    (root.GetEntityFromRelativePath(relativePath, true) as DirectoryEntity).Add(file);
                }
            }
        }
    }
}
