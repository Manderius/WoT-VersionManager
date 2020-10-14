namespace VersionManager.Persistence
{
    public interface DataDeserializer
    {
        T Deserialize<T>(string path);
    }
}
