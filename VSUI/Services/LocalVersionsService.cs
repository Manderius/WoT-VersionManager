using System.Collections.Generic;
using System.IO;
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
                new LocalGameVersion( "1.2.0",  @"C:\Program Files (x86)\World_of_Tanks"),
            };

        public List<LocalGameVersion> GetLocalVersions()
        {
            return _items;
        }

        public ImportResult CanImport(string path) {
            // TODO check if it already exists
            LocalGameVersion ver = LocalVersionParser.FromDirectory(path);
            if (ver != null)
            {
                return ImportResult.CAN_IMPORT;
            }

            return ImportResult.INVALID_PATH;
        }

        public enum ImportResult { 
            CAN_IMPORT, INVALID_PATH, ALREADY_EXISTS, IMPORT_FAILED, IMPORT_SUCCESS
        }
    }
}
