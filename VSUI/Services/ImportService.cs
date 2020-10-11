using System;
using System.Collections.Generic;
using System.IO;
using VersionManager.Extraction;
using VersionManager.Filesystem;
using VersionManager.GameGenerator;
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
        private DirectoryCache _dirCache;
        private DataSerializer _ds;

        public ImportService(ManagedVersionsService mvs, DirectoryCache directoryCache, DataSerializer ds)
        {
            _mvs = mvs;
            _dirCache = directoryCache;
            _ds = ds;
        }

        public enum ImportStatus
        {
            CAN_IMPORT, INVALID_PATH, ALREADY_EXISTS
        }

        public ImportStatus CanImport(string path)
        {
            LocalGameVersion ver = new LocalGameVersion(Helpers.GetGameVersion(path), path);
            if (ver.Version == null)
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
            string xml = Path.Combine(Settings.Default.DataDirectory, "VersionData", xmlFilename);
            string container = Path.Combine(Settings.Default.DataDirectory, "Container");
            string output = Path.Combine(Settings.Default.GameOutputDirectory, "World of Tanks " + root.Version);

            CreateVersionXML(path, xml, sum);

            ExtractToContainer(xml, container, path, sum);
            totalFiles = root.GetAllFileEntities(true).Count;
            processed = totalFiles * 2;
            CreateGameDirectory(xml, output, container, sum);
            if (copyMods)
            {
                string modsDir = Path.Combine(path, "mods");
                if (Directory.Exists(modsDir))
                    CopyDirectory(new DirectoryInfo(modsDir), Path.Combine(output, "mods"));

                string resModsDir = Path.Combine(path, "res_mods");
                if (Directory.Exists(resModsDir))
                    CopyDirectory(new DirectoryInfo(resModsDir), Path.Combine(output, "res_mods"));
            }
            progress.Report(100);

            string cacheXml = Settings.Default.DirectoryCacheFile;
            _ds.Serialize(_dirCache, cacheXml);

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

        private void ExtractToContainer(string versionXml, string container, string gameDir, IProgress<int> progress)
        {
            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });
            RootDirectoryEntity deser = new RootDirectoryEntityIO().Deserialize(versionXml);
            ex.Extract(deser, gameDir, Helpers.EntityToPath(container), _dirCache, progress);
        }

        private void CreateGameDirectory(string versionXml, string outputDir, string container, IProgress<int> progress)
        {
            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(versionXml);
            GameDirGenerator.Generate(root, outputDir, container, Helpers.EntityToPath(container), progress);
        }

        private void CopyDirectory(DirectoryInfo directory, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (var file in directory.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(destination, file.Name), true);
            }

            foreach (var dir in directory.EnumerateDirectories())
            {
                CopyDirectory(dir, Path.Combine(destination, dir.Name));
            }
        }
    }
}
