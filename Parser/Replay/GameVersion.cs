namespace VersionManager.Replay
{
    public class GameVersion
    {
        public static GameVersion UNKNOWN = new GameVersion("Unknown");

        public string Version { get; private set; }

        public GameVersion(string version)
        {
            Version = version;
        }

        public override bool Equals(object obj)
        {
            if (obj is GameVersion rv)
                return rv.Version == Version;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }

        public override string ToString()
        {
            return Version;
        }
    }
}
