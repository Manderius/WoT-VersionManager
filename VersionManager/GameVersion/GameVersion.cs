using System.Runtime.Serialization;

namespace VersionManager.GameVersion
{
    [DataContract(Namespace = "VersionManager.GameVersion")]
    public class GameVersion
    {
        public static GameVersion UNKNOWN = new GameVersion("Unknown");

        [DataMember]
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
