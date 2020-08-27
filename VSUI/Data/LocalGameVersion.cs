using VersionManager.Replay;

namespace VSUI.Data
{
    public class LocalGameVersion: GameVersion
    {
        public string Path { get; set; }

        public LocalGameVersion(string version, string path): base(version)
        {
            Path = path;
        }
    }
}
