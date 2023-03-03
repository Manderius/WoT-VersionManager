﻿using VersionManager.GameVersionData;

namespace VersionManager.Replay
{
    public class Replay
    {
        public GameVersion Version { get; set; }
        public string Tank { get; set; }
        public GameMap Map { get; set; }
        public string Player { get; set; }
        public string Date { get; set; }
        public string Path { get; set; }

        public Replay(string version, string tank, GameMap map, string player, string date, string path)
        {
            Version = new GameVersion(version);
            Tank = tank;
            Map = map;
            Player = player;
            Date = date;
            Path = path;
        }

        public Replay(GameVersion version, string tank, GameMap map, string player, string date, string path)
        {
            Version = version;
            Tank = tank;
            Map = map;
            Player = player;
            Date = date;
            Path = path;
        }
    }

    
}
