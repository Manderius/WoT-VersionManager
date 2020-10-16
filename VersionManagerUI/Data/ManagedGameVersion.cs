using System.Runtime.Serialization;

namespace VersionManagerUI.Data
{
    [DataContract(Name = "ManagedGameVersion", Namespace = "VersionManagerUI")]
    public class ManagedGameVersion
    {
        [DataMember]
        public LocalGameVersion LocalVersion { get; set; }
        [DataMember]
        public string GameXML { get; set; }

        public string Version => LocalVersion.Version;

        public string Path => LocalVersion.Path;

        public ManagedGameVersion(LocalGameVersion localVersion, string gameXML)
        {
            LocalVersion = localVersion;
            GameXML = gameXML;
        }

        public ManagedGameVersion(string directory, string version, string gameXML)
        {
            LocalVersion = new LocalGameVersion(version, directory);
            GameXML = gameXML;
        }
    }
}
