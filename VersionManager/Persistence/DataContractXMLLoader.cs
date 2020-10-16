using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace VersionManager.Persistence
{
    public class DataContractXMLLoader : DataSerializer, DataDeserializer
    {
        public void Serialize<T>(T data, string path)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter xw = XmlWriter.Create(path, settings))
            {
                serializer.WriteObject(xw, data);
            }
        }

        public T Deserialize<T>(string path)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));

            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (XmlReader reader = XmlReader.Create(fs))
            {
                T result = (T)serializer.ReadObject(reader);
                return result;
            }
           
        }
    }
}
