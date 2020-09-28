using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VersionManager.Replay;
using VersionManager.Utils;
using VSUI.Data;

namespace VSUI.Services
{
    public class ManagedVersionsService
    {
        private ObservableCollection<LocalGameVersion> _items = new ObservableCollection<LocalGameVersion>()
            {
                new LocalGameVersion( "0.9.8",  @"C:\Program Files (x86)\World_of_Tanks"),
                new LocalGameVersion( "1.1.0",  @"C:\Program Files (x86)\World_of_Tanks"),
                new LocalGameVersion( "1.10.0.1",  @"C:\Program Files (x86)\World_of_Tanks_EU"),
            };

        public ObservableCollection<LocalGameVersion> GetManagedVersions()
        {
            return _items;
        }

        public bool Contains(GameVersion version)
        {
            return _items.FirstOrDefault(x => x.Version == version.Version) != null;
        }

        public bool Contains(LocalGameVersion version)
        {
            return _items.Contains(version);
        }
        public enum ImportStatus
        {
            CAN_IMPORT, INVALID_PATH, ALREADY_EXISTS
        }

        public ImportStatus CanImport(string path)
        {
            LocalGameVersion ver = new LocalGameVersion(Helpers.GetGameVersion(path), path);
            if (ver.Version == null)
                return ImportStatus.INVALID_PATH;

            if (Contains(new GameVersion(ver.Version)))
                return ImportStatus.ALREADY_EXISTS;

            return ImportStatus.CAN_IMPORT;
        }

        public void Import(string path)
        {
            if (CanImport(path) != ImportStatus.CAN_IMPORT)
                return;

            //TODO actual import

            _items.Add(new LocalGameVersion(Helpers.GetGameVersion(path), path));
        }
    }
}
