using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using VersionManager.Filesystem;

namespace VersionManager.GameGenerator
{
    class GameDirGenerator
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
            return CreateHardLink(path, source, IntPtr.Zero);
        }

        public static void Generate(DirectoryEntity entity, string destination, string container, Func<BaseEntity, string> entityToDir)
        {
            Directory.CreateDirectory(destination);

            foreach (FileEntity file in entity.Contents.OfType<FileEntity>())
            {
                CreateHardLinkForFileEntity(file, destination, Path.Combine(container, entityToDir(file)));
            }

            foreach (DirectoryEntity dir in entity.Contents.OfType<DirectoryEntity>())
            {
                Generate(dir, Path.Combine(destination, dir.Name), container, entityToDir);
            }
        }
    }
}
