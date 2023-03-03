namespace VersionManager.Replay
{
    public class GameMap
    {
        public string FullName { get; private set; }
        public string DisplayName { get; private set; }
        public string InternalName { get; private set; }

        public GameMap(string displayName, string internalName)
        {
            DisplayName = displayName;
            InternalName = internalName;
            FullName = $"{displayName} ({internalName})";
        }
    }
}
