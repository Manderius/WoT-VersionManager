using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AnyReplay_Player
{
    class PathsBuilder
    {
        public void CreatePaths(string dataFile, string containerPath, string outFile)
        {
            if (!File.Exists(dataFile))
                return;

            HashSet<string> paths = new HashSet<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(dataFile);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(outFile, settings);

            writer.WriteStartElement("root");
            writer.WriteStartElement("Paths");

            foreach (XmlNode item in doc.FirstChild.ChildNodes)
            {
                if (item.Name == "Archive")
                {
                    writer.WriteElementString("Path", "./" + item.Attributes["Path"].Value);
                }
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

        }
    }
}
