using System.Collections.Generic;
using System.IO;

namespace VSUI.Services
{
    public class LocalVersionsService
    {
        private List<LocalGameVersion> _items = new List<LocalGameVersion>()
            {
                new LocalGameVersion() { Version = "0.9.8", Location = @"C:\Program Files (x86)\World_of_Tanks" },
                new LocalGameVersion() { Version = "1.1.0" },
                new LocalGameVersion() { Version = "1.1.2" },
            };

        public List<LocalGameVersion> GetLocalVersions()
        {
            return _items;
        }

        public ImportResult CanImport(string path) {
            // TODO check if it already exists
            if (Directory.Exists(path) && File.Exists(Path.Combine(path, "WorldOfTanks.exe")))
            {
                return ImportResult.CAN_IMPORT;
            }

            return ImportResult.INVALID_PATH;
        }

        public enum ImportResult { 
            CAN_IMPORT, INVALID_PATH, ALREADY_EXISTS, IMPORT_FAILED, IMPORT_SUCCESS
        }
    }

    public class LocalGameVersion
    {
        public string Version { get; set; }
        public string Location { get; set; }
    }
}
