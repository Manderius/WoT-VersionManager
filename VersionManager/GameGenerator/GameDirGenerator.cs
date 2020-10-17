using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using VersionManager.Filesystem;

namespace VersionManager.GameGenerator
{
    public class GameDirGenerator
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
        );

        private static bool CreateHardLinkForFileEntity(FileEntity entity, string destination, string sourceDir)
        {
            string path = Path.Combine(destination, entity.Name);
            string source = Path.Combine(sourceDir, entity.Name);
            if (!File.Exists(source))
            {
                throw new FileNotFoundException("Required file not found in container: " + source);
            }
            if (File.Exists(path))
            {
                return true;
            }
            return CreateHardLink(path, source, IntPtr.Zero);
        }

        public static void Generate(DirectoryEntity root, string destination, string container, Func<BaseEntity, string> entityToDir, IProgress<int> progress)
        {
            int totalFiles = root.GetAllFileEntities(true).Count;
            int processed = 0;
            Progress<int> sumProgress = new Progress<int>(prog =>
            {
                int percent = ++processed * 100 / totalFiles;
                progress?.Report(percent);
            });
            GenerateInner(root, destination, container, entityToDir, sumProgress);
            FixPathsFile(destination);
        }

        private static void FixPathsFile(string dir)
        {
            string paths = Path.Combine(dir, "paths.xml");
            string[] lines = File.ReadAllLines(paths);
            string[] edited = lines.Select(line => line.Replace("<Package ", "<Path ").Replace("</Package>", "</Path>")).Where(line => !line.Contains("Packages>")).ToArray();
            File.WriteAllLines(paths, edited);
        }

        private static void GenerateInner(DirectoryEntity entity, string destination, string container, Func<BaseEntity, string> entityToDir, IProgress<int> progress)
        {
            Directory.CreateDirectory(destination);

            foreach (FileEntity file in entity.Contents.OfType<FileEntity>())
            {
                CreateHardLinkForFileEntity(file, destination, Path.Combine(container, entityToDir(file)));
                progress?.Report(1);
            }

            foreach (DirectoryEntity dir in entity.Contents.OfType<DirectoryEntity>())
            {
                GenerateInner(dir, Path.Combine(destination, dir.Name), container, entityToDir, progress);
            }

            if (entity is RootDirectoryEntity root)
            {
                CreateDirectory(Path.Combine(destination, "mods", root.Version));
                CreateDirectory(Path.Combine(destination, "res_mods", root.Version));
            }
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
