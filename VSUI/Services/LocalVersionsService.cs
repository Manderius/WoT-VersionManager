using System.Collections.Generic;
using System.Linq;
using VersionManager.Replay;
using VSUI.Data;
using VSUI.Parsers;

namespace VSUI.Services
{
    public class LocalVersionsService
    {
        private List<LocalGameVersion> _items = new List<LocalGameVersion>()
            {
                new LocalGameVersion( "0.9.8",  @"C:\Program Files (x86)\World_of_Tanks"),
                new LocalGameVersion( "1.1.0",  @"C:\Program Files (x86)\World_of_Tanks"),
                new LocalGameVersion( "1.10.0.1",  @"C:\Program Files (x86)\World_of_Tanks_EU"),
            };

        public List<LocalGameVersion> GetLocalVersions()
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

        public ImportStatus CanImport(string path)
        {
            // TODO check if it already exists
            LocalGameVersion ver = LocalVersionParser.FromDirectory(path);
            if (ver == null)
                return ImportStatus.INVALID_PATH;

            return Contains(new GameVersion(ver.Version)) ? ImportStatus.ALREADY_EXISTS : ImportStatus.CAN_IMPORT;
        }

        public enum ImportStatus
        {
            CAN_IMPORT, INVALID_PATH, ALREADY_EXISTS
        }
    }
}
