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

        public ManagedGameVersion(LocalGameVersion localVersion, string gameXML)
        {
            LocalVersion = localVersion;
            GameXML = gameXML;
        }
    }
}
