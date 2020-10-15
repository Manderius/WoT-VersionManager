using System;
using System.IO;
using VersionManager.Filesystem;
using VersionManager.Persistence;
using VersionManager.GameVersion;
using VersionManager.Utils;
using VersionManagerUI.Data;
using VersionManagerUI.Properties;

namespace VersionManagerUI.Services
{
    public class ImportService
    {
        private ManagedVersionsService _mvs;
        private GameDirectoryService _gds;

        public ImportService(ManagedVersionsService mvs, GameDirectoryService gds)
        {
            _mvs = mvs;
            _gds = gds;
        }

        public enum ImportStatus
        {
            CAN_IMPORT, INVALID_PATH, ALREADY_EXISTS
        }

        public ImportStatus CanImport(string path)
        {
            LocalGameVersion ver = null;
            try
            {
                ver = new LocalGameVersion(Helpers.GetGameVersion(path), path);
            } 
            catch (Exception ex) { }

            if (ver == null || ver.Version == null)
                return ImportStatus.INVALID_PATH;

            if (_mvs.Contains(new GameVersion(ver.Version)))
                return ImportStatus.ALREADY_EXISTS;

            return ImportStatus.CAN_IMPORT;
        }

        public void Import(string path, bool copyMods, IProgress<int> progress)
        {
            if (CanImport(path) != ImportStatus.CAN_IMPORT)
                return;

            RootDirectoryEntity root = Helpers.CreateRootEntityFromDirectory(path, false);
            int totalFiles = root.GetAllFileEntities(false).Count;
            int processed = 0;

            Progress<int> sum = new Progress<int>(prog =>
            {
                processed++;
                int percent = processed * 100 / totalFiles;
                progress.Report(percent / 3);
            });

            string xmlFilename = root.Version.Replace(".", "_") + ".xml";
            string xml = Path.Combine(Settings.Default.VersionDataDirectory, xmlFilename);
            string output = Path.Combine(Settings.Default.GameOutputDirectory, "World of Tanks " + root.Version);

            CreateVersionXML(path, xml, sum);

            _gds.ExtractToContainer(xml, path, sum);
            totalFiles = root.GetAllFileEntities(true).Count;
            processed = totalFiles * 2;
            _gds.CreateGameDirectory(xml, output, sum);

            if (copyMods)
            {
                CopyDirectory("mods", path, output);
                CopyDirectory("res_mods", path, output);
            }
            progress.Report(100);

            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                _mvs.Add(xml, output);
            });
        }

        private void CreateVersionXML(string dir, string output, IProgress<int> progress)
        {
            RootDirectoryEntity root = Helpers.CreateRootEntityFromDirectory(dir, true, progress);
            new RootDirectoryEntityIO().Serialize(root, output);
        }

        private void CopyDirectory(string dirname, string parent, string destination)
        {
            string dir = Path.Combine(parent, dirname);
            if (Directory.Exists(dir))
                CopyDirectoryRecursively(new DirectoryInfo(dir), Path.Combine(destination, dirname));
        }

        private void CopyDirectoryRecursively(DirectoryInfo directory, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (var file in directory.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(destination, file.Name), true);
            }

            foreach (var dir in directory.EnumerateDirectories())
            {
                CopyDirectoryRecursively(dir, Path.Combine(destination, dir.Name));
            }
        }
    }
}
