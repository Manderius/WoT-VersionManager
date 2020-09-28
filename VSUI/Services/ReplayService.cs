using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VersionManager.Replay;
using VSUI.Data;

namespace VSUI.Services
{
    public class ReplayService
    {
        private ManagedVersionsService _localVersionsService;
        public ReplayService(ManagedVersionsService localVersionsService) {
            _localVersionsService = localVersionsService;
        }

        public void PlayReplay(Replay replay)
        {
            if (replay.Version == GameVersion.UNKNOWN)
                throw new InvalidOperationException("Cannot play replay with unknown version.");

            PlayReplay(replay, replay.Version);
        }

        public void PlayReplay(Replay replay, GameVersion version)
        {
            LocalGameVersion local = _localVersionsService.GetManagedVersions().FirstOrDefault(x => x.Version == version.Version);
            if (local is null)
                throw new InvalidOperationException("This version is not available.");

            PlayReplay(replay, local);
        }

        public void PlayReplay(Replay replay, LocalGameVersion version)
        {
            string executable = Path.Combine(version.Path, "WorldOfTanks.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(executable);
            startInfo.Arguments = string.Format("\"{0}\"", replay.Path);
            Process.Start(startInfo);
        }
    }
}
