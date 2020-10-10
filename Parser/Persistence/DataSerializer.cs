namespace VersionManager.Persistence
{
    public interface DataSerializer
    {
        void Serialize<T>(T data, string path);
    }
}
