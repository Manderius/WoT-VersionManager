using System;
using System.Collections.Generic;
using System.IO;
using VersionManager.Extraction;
using VersionManager.Filesystem;
using VersionManager.GameGenerator;
using VersionManager.GameRemover;
using VersionManager.Persistence;
using VersionManager.Utils;
using VersionManagerUI.Data;
using VersionManagerUI.Properties;

namespace VersionManagerUI.Services
{
    public class GameDirectoryService
    {
        private DirectoryCache _dirCache;
        private DataSerializer _ds;
        private ManagedVersionsService _mvs;

        public GameDirectoryService(ManagedVersionsService mvs, DirectoryCache dirCache, DataSerializer ds)
        {
            _mvs = mvs;
            _dirCache = dirCache;
            _ds = ds;
        }

        public void ExtractToContainer(string versionXml, string gameDir, IProgress<int> progress)
        {
            string container = Settings.Default.ContainerDirectory;
            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });
            RootDirectoryEntity deser = new RootDirectoryEntityIO().Deserialize(versionXml);
            ex.Extract(deser, gameDir, Helpers.EntityToPath(container), _dirCache, progress);

            string cacheXml = Settings.Default.DirectoryCacheFile;
            _ds.Serialize(_dirCache, cacheXml);
        }

        public void CreateGameDirectory(string versionXml, string outputDir, IProgress<int> progress)
        {
            string container = Settings.Default.ContainerDirectory;
            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(versionXml);
            GameDirGenerator.Generate(root, outputDir, container, Helpers.EntityToPath(container), progress);
        }

        public void DeleteGameDirectory(ManagedGameVersion game, IProgress<int> progress)
        {
            progress.Report(10);
            if (Directory.Exists(game.Path))
                Directory.Delete(game.Path, true);

            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(game.GameXML);
            List<RootDirectoryEntity> otherVersions = new List<RootDirectoryEntity>();
            foreach (var mgv in _mvs.GetManagedVersions())
            {
                RootDirectoryEntity data = new RootDirectoryEntityIO().Deserialize(mgv.GameXML);
                otherVersions.Add(data);
            }

            Progress<int> partialProgress = new Progress<int>(prog =>
            {
                int percent = (int) (prog * 0.9);
                progress.Report(10 + percent);
            });

            string container = Settings.Default.ContainerDirectory;
            GameFilesRemover.RemoveFiles(root, otherVersions, container, Helpers.EntityToPath(container), _dirCache, partialProgress);
            SaveDirCache();

            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                _mvs.Remove(game);
            });
            progress.Report(100);
        }

        public void RebuildGameDirectory(ManagedGameVersion game, IProgress<int> progress)
        {
            progress.Report(10);
            if (Directory.Exists(game.Path))
                Directory.Delete(game.Path, true);

            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(game.GameXML);
            int totalFiles = root.GetAllFileEntities(true).Count;
            int processed = 0;
            Progress<int> partialProgress = new Progress<int>(prog =>
            {
                int percent = 90 * ++processed / totalFiles;
                progress.Report(10 + percent);
            });

            string container = Settings.Default.ContainerDirectory;
            GameDirGenerator.Generate(root, game.Path, container, Helpers.EntityToPath(container), partialProgress);
            progress.Report(100);
        }

        private void SaveDirCache()
        {
            string cacheFile = Settings.Default.DirectoryCacheFile;
            new DataContractXMLLoader().Serialize(_dirCache, cacheFile);
        }
    }
}
