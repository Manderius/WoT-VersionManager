using System;
using System.IO;
using System.IO.Compression;
using VersionManager.Filesystem;
using VersionManager.Hashing;

namespace VersionManager.Parsing
{
    public class GameDirectoryParser
    {
        public static void Parse(DirectoryInfo directory, DirectoryEntity parent, int prefixLength, HashProvider hashProvider, IgnoreList ignoreList, IProgress<int> progress)
        {
            bool computeHash = hashProvider != null;

            foreach (FileInfo file in directory.EnumerateFiles())
            {
                string relativePath = file.FullName.Substring(prefixLength).TrimStart('\\');

                if (ignoreList.IsIgnored(relativePath))
                {
                    continue;
                }

                if (file.Extension == ".pkg")
                {
                    ParsePackage(file, parent, hashProvider);
                    progress?.Report(1);
                }
                else
                {
                    FileEntity fileEnt = computeHash ? new FileEntity(file.Name, hashProvider.FromStream(new FileStream(file.FullName, FileMode.Open)) + relativePath.GetHashCode(), file.Length) : new FileEntity(file.Name, file.Length);
                    parent.Add(fileEnt);
                    progress?.Report(1);
                }
            }

            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                if (ignoreList.IsIgnored(Path.Combine(parent.RelativePath, dir.Name) + "\\"))
                    continue;

                DirectoryEntity child = new DirectoryEntity(dir.Name);
                parent.Add(child);
                Parse(dir, child, prefixLength, hashProvider, ignoreList, progress);
            }
        }

        private static void ParsePackage(FileInfo package, DirectoryEntity parent, HashProvider hashProvider)
        {
            PackageEntity root = new PackageEntity(package.Name);
            parent.Add(root);
            bool computeHash = hashProvider != null;

            using (ZipArchive archive = ZipFile.OpenRead(package.FullName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Directory
                    if (entry.Length == 0)
                        continue;

                    string relativePath = entry.FullName.Substring(0, entry.FullName.Length - entry.Name.Length);
                    FileEntity file = computeHash ? new FileEntity(entry.Name, hashProvider.FromStream(entry.Open()) + entry.FullName.GetHashCode(), entry.Length) : new FileEntity(entry.Name, entry.Length);
                    (root.GetEntityFromRelativePath(relativePath, true) as DirectoryEntity).Add(file);
                }
            }
        }
    }
}
