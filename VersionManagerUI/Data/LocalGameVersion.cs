using System.Runtime.Serialization;
using VersionManager.GameVersionData;

namespace VersionManagerUI.Data
{
    [DataContract(Namespace = "VersionManagerUI")]
    public class LocalGameVersion: GameVersion
    {
        [DataMember]
        public string Path { get; set; }

        public LocalGameVersion(string version, string path): base(version)
        {
            Path = path;
        }
    }
}
