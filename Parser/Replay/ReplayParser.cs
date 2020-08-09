using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace VersionManager.Replay
{
    public class ReplayParser
    {
        public static Replay Parse(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                b.ReadInt32(); // Skips magic number
                b.ReadInt32(); // Skips result
                int length = b.ReadInt32();
                byte[] data = b.ReadBytes(length);

                string str = Encoding.Default.GetString(data);

                dynamic json = JsonConvert.DeserializeObject(str);

                string map = string.Format("{0} ({1})", json.mapDisplayName, json.mapName);
                string version = json.clientVersionFromExe ?? "Unknown";
                version = version.Replace(',', '.').Replace(" ", "");

                string tank = json.playerVehicle;
                string player = json.playerName;
                string date = json.dateTime;

                return new Replay(version, tank, map, player, date);
            }
        }
    }
}
