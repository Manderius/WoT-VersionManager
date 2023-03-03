using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using VersionManager.GameVersionData;
using VersionManager.Replay;
using VersionManagerUI.Data;

namespace VersionManagerUI.Services
{
    public class ReplayService
    {
        private ManagedVersionsService _localVersionsService;
        public ReplayService(ManagedVersionsService localVersionsService) {
            _localVersionsService = localVersionsService;
        }

        public void PlayReplay(Replay replay, bool optimizePaths)
        {
            if (replay.Version == GameVersion.UNKNOWN)
                throw new InvalidOperationException("Cannot play replay with unknown version.");

            PlayReplay(replay, replay.Version, optimizePaths);
        }

        public void PlayReplay(Replay replay, GameVersion version, bool optimizePaths)
        {
            LocalGameVersion local = _localVersionsService.GetManagedVersions().FirstOrDefault(x => x.LocalVersion.Version == version.Version).LocalVersion;
            if (local is null)
                throw new InvalidOperationException("This version is not available.");

            PlayReplay(replay, local, optimizePaths);
        }

        public void PlayReplay(Replay replay, LocalGameVersion version, bool optimizePaths)
        {
            RestorePathsFile(version.Path);
            if (optimizePaths)
            {
                OptimizePaths(version.Path, replay);
            }
            string executable = Path.Combine(version.Path, "WorldOfTanks.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(executable)
            {
                Arguments = string.Format("\"{0}\"", replay.Path),
                WorkingDirectory = version.Path
            };
            Process.Start(startInfo);
        }

        private void RestorePathsFile(string gameRootPath)
        {
            string pathsBackupFile = Path.Combine(gameRootPath, "paths.xml.bak");
            if (File.Exists(pathsBackupFile))
            {
                File.Copy(pathsBackupFile, Path.Combine(gameRootPath, "paths.xml"), true);
            }
        }

        private void OptimizePaths(string gameRootPath, Replay replay)
        {
            string pathsBackupFile = Path.Combine(gameRootPath, "paths.xml.bak");
            string pathsFile = Path.Combine(gameRootPath, "paths.xml");
            if (!File.Exists(pathsBackupFile))
            {
                File.Copy(pathsFile, pathsBackupFile);
            }

            string[] lines = File.ReadAllLines(pathsFile);
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains(".pkg")) continue;
                if (CanRemovePackage(lines[i], replay))
                {
                    lines[i] = $"<!-- {lines[i]} -->";
                }
            }
            File.WriteAllLines(pathsFile, lines);
        }

        private bool CanRemovePackage(string package, Replay replay) {
            Regex mapAndHangarMatcher = new Regex(@"./res/packages/(h)?\d\d");
            // Remove other map packages - map packages start with a number
            return mapAndHangarMatcher.IsMatch(package) && !package.Contains(replay.Map.InternalName);
        }
    }
}
