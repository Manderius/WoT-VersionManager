using System.IO;
using System.Xml.Serialization;
using VersionSwitcher_Server.Filesystem;

namespace VersionSwitcher_Server.Persistence
{
    class XMLStructureLoader : DataDeserializer, DataSerializer
    {
        public RootDirectoryEntity Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootDirectoryEntity));

            RootDirectoryEntity root;

            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                root = (RootDirectoryEntity)serializer.Deserialize(reader);
            }

            root.Deserialize();

            return root;
        }

        public void Serialize(RootDirectoryEntity baseEntity, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootDirectoryEntity));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, baseEntity);
            }
        }
    }
}
