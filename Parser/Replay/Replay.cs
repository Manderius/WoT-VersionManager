namespace VersionManager.Replay
{
    public class Replay
    {
        public string Version { get; set; }
        public string Tank { get; set; }
        public string Map { get; set; }
        public string Player { get; set; }
        public string Date { get; set; }

        public Replay(string version, string tank, string map, string player, string date)
        {
            Version = version;
            Tank = tank;
            Map = map;
            Player = player;
            Date = date;
        }
    }
}
