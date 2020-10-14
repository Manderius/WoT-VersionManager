using System.IO;
using System.Xml.Serialization;

namespace VersionManager.Persistence
{
    public class XMLStructureLoader: DataDeserializer, DataSerializer
    {
        public T Deserialize<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T data;

            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                data = (T)serializer.Deserialize(reader);
            }

            return data;
        }

        public void Serialize<T>(T data, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
