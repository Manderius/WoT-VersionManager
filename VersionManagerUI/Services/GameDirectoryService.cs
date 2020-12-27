using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            RootDirectoryEntity deser = new RootDirectoryEntityIO().Deserialize(versionXml);
            ExtractToContainer(deser, gameDir, progress);
        }

        public void ExtractToContainer(RootDirectoryEntity root, string gameDir, IProgress<int> progress)
        {
            string container = Settings.Default.ContainerDirectory;
            ExtractionManager ex = new ExtractionManager(new List<Extractor>() { new PackageExtractor(), new FileExtractor() });
            ex.Extract(root, gameDir, Helpers.EntityToPath(container), _dirCache, progress);

            string cacheXml = Settings.Default.DirectoryCacheFile;
            _ds.Serialize(_dirCache, cacheXml);
        }

        public void CreateGameDirectory(string versionXml, string outputDir, IProgress<int> progress)
        {
            RootDirectoryEntity root = new RootDirectoryEntityIO().Deserialize(versionXml);
            CreateGameDirectory(root, outputDir, progress);
        }

        public void CreateGameDirectory(RootDirectoryEntity game, string outputDir, IProgress<int> progress)
        {
            string container = Settings.Default.ContainerDirectory;
            GameDirGenerator.Generate(game, outputDir, container, Helpers.EntityToPath(container), progress);
        }

        public void UpdateGameDirectory(RootDirectoryEntity updated, string outputDir, IProgress<int> progress)
        {
            ManagedGameVersion oldMGV = _mvs.GetManagedVersions().First(mvg => mvg.Version == updated.Version);
            RootDirectoryEntity oldBuild = new RootDirectoryEntityIO().Deserialize(oldMGV.GameXML);
            HashSet<FileEntity> oldFiles = oldBuild.GetAllFileEntities(true).OfType<FileEntity>().ToHashSet();
            HashSet<FileEntity> newFiles = updated.GetAllFileEntities(true).OfType<FileEntity>().ToHashSet();
            HashSet<FileEntity> changedFiles = newFiles.Except(oldFiles).ToHashSet();
            HashSet<FileEntity> filesToRemove = oldFiles.Except(newFiles).ToHashSet();

            // Do not remove files that only changed location
            HashSet<string> newFilesHashes = newFiles.Select(x => x.Hash).ToHashSet();
            HashSet<FileEntity> movedFileHashes = filesToRemove.Where(f => newFilesHashes.Contains(f.Hash)).ToHashSet();
            filesToRemove.ExceptWith(movedFileHashes);

            string container = Settings.Default.ContainerDirectory;
            List<RootDirectoryEntity> otherVersions = new List<RootDirectoryEntity>();
            foreach (var mgv in _mvs.GetManagedVersions().Except(new List<ManagedGameVersion>() { oldMGV }))
            {
                RootDirectoryEntity data = new RootDirectoryEntityIO().Deserialize(mgv.GameXML);
                otherVersions.Add(data);
            }

            int basePercent = 0;
            Progress<int> partialProgress = new Progress<int>(percent =>
            {
                progress.Report(basePercent + percent / 2);
            });

            // Remove old unused files from cache
            GameFilesRemover.RemoveFiles(filesToRemove,
                otherVersions,
                Helpers.EntityToPath(container),
                _dirCache,
                partialProgress
                );
            SaveDirCache();
            basePercent = 50;
            // Remove old unused files from game directory
            foreach (FileEntity unusedFile in filesToRemove) {
                string path = Path.Combine(oldMGV.Path, unusedFile.RelativePath);
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex) { }
            }

            // Update files in directory
            CreateGameDirectory(updated, oldMGV.Path, partialProgress);
            progress.Report(100);
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
                int percent = (int)(prog * 0.9);
                progress.Report(10 + percent);
            });

            string container = Settings.Default.ContainerDirectory;
            GameFilesRemover.RemoveFiles(root, otherVersions, Helpers.EntityToPath(container), _dirCache, partialProgress);
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

            Progress<int> partialProgress = new Progress<int>(percent =>
            {
                progress.Report((int)(10 + 0.9 * percent));
            });

            CreateGameDirectory(game.GameXML, game.Path, partialProgress);
            progress.Report(100);
        }

        private void SaveDirCache()
        {
            string cacheFile = Settings.Default.DirectoryCacheFile;
            new DataContractXMLLoader().Serialize(_dirCache, cacheFile);
        }
    }
}
