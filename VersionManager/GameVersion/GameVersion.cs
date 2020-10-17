using System;
using System.Linq;
using System.Runtime.Serialization;

namespace VersionManager.GameVersion
{
    [DataContract(Namespace = "VersionManager.GameVersion")]
    public class GameVersion: IComparable
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

        public int CompareTo(GameVersion other)
        {
            if (this == UNKNOWN && other == UNKNOWN)
                return 0;
            if (other == UNKNOWN)
                return 1;
            if (this == UNKNOWN)
                return -1;

            string[] myVersionNumbers = Version.Split('.');
            string[] otherVersionNumbers = other.Version.Split('.');
            for (int i = 0; i < myVersionNumbers.Length; i++)
            {
                if (i == otherVersionNumbers.Length)
                    return 1;
                if (myVersionNumbers[i] == otherVersionNumbers[i])
                    continue;
                return Convert.ToInt32(myVersionNumbers[i]) - Convert.ToInt32(otherVersionNumbers[i]);
            }
            return 0;
        }

        public int CompareTo(object obj)
        {
            if (obj is GameVersion other)
                return CompareTo(other);
            return 0;
        }
    }
}
