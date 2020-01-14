using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VersionManager
{
    class ReplayParser
    {
        public static Dictionary<string, string> Parse(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            Dictionary<string, string> result = new Dictionary<string, string>();

            using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                b.ReadInt32(); // Skips magic number
                b.ReadInt32(); // Skips result
                int length = b.ReadInt32();
                byte[] data = b.ReadBytes(length);

                string str = Encoding.Default.GetString(data);

                dynamic json = JsonConvert.DeserializeObject(str);

                string map = json.mapName;
                string version = json.clientVersionFromExe;
                version = version.Replace(',', '.').Replace(" ", "");

                result.Add("version", version);
                result.Add("map", map);
            }

            return result;
        }
    }
}
