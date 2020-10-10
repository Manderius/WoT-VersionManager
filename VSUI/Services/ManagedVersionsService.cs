using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VersionManager.Persistence;
using VersionManager.GameVersion;
using VersionManager.Utils;
using VersionManagerUI.Data;
using VersionManagerUI.Properties;

namespace VersionManagerUI.Services
{
    [DataContract(Name = "ManagedVersions", Namespace = "VersionManagerUI")]
    public class ManagedVersionsService
    {
        public DataSerializer Serializer { private get; set; }

        [DataMember(Name = "InstalledVersions")]
        private ManagedVersionCollection _items { get; set; }

        public ManagedVersionsService() {
            _items = new ManagedVersionCollection();
        }

        public ManagedVersionsService(DataSerializer serializer)
        {
            Serializer = serializer;
            _items = new ManagedVersionCollection()
            {
                new ManagedGameVersion( new LocalGameVersion( "0.9.4",  @"C:\Program Files (x86)\World_of_Tanks_EU"), @"E:\WoT\VersionData\094.xml")
            };
        }

        public ObservableCollection<ManagedGameVersion> GetManagedVersions()
        {
            return _items;
        }

        public bool Contains(string version)
        {
            return Contains(new GameVersion(version));
        }

        public bool Contains(GameVersion version)
        {
            return _items.FirstOrDefault(x => x.LocalVersion.Version == version.Version) != null;
        }
        
        public bool Contains(ManagedGameVersion version)
        {
            return _items.Contains(version);
        }

        public void Add(string versionXml, string directory)
        {
            string dirVersion = Helpers.GetGameVersion(directory);
            string xmlVersion = new RootDirectoryEntityIO().Deserialize(versionXml).Version;

            if (dirVersion != xmlVersion)
                return;

            if (Contains(dirVersion))
                return;

            Add(new ManagedGameVersion(new LocalGameVersion(dirVersion, directory), versionXml));
        }

        public void Add(ManagedGameVersion mgv)
        {
            _items.Add(mgv);
            Save();
        }

        public void Save()
        {
            string path = Settings.Default.ManagedVersionsFile;
            Serializer.Serialize(_items, path);
        }

        public void Load(DataDeserializer dds)
        {
            string path = Settings.Default.ManagedVersionsFile;
            _items = dds.Deserialize<ManagedVersionCollection>(path);
        }
    }
}
