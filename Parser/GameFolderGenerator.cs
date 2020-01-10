using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

namespace VersionSwitcher_Server
{
    class GameFolderGenerator
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
        );

        public bool CreateHardLinkForFile(XmlNode file, string baseDir, string container)
        {
            // Create directory for hard link
            Directory.CreateDirectory(Path.Combine(baseDir, file.Attributes["Path"].Value));
            // Check if file with hash exists
            string hash = file.ChildNodes[0].InnerText;
            string originalFile = Path.Combine(container, hash, file.Attributes["Name"].Value);
            if (!Directory.Exists(Path.Combine(container, hash)) || !File.Exists(originalFile))
            {
                throw new FileNotFoundException("Required file not found in container.", Path.Combine(container, hash, file.Attributes["Name"].Value));
            }

            return CreateHardLink(Path.Combine(baseDir, file.Attributes["Path"].Value, file.Attributes["Name"].Value), originalFile, IntPtr.Zero);
        }

        public void Generate(string versionContents, string container, string destination)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(versionContents);
            string versionInfo = Path.Combine(destination, "VersionSwitcher.txt");

            if (Directory.Exists(destination))
            {
                if (File.Exists(versionInfo))
                {
                    string version = File.ReadLines(versionInfo).First();
                    if (version == doc.FirstChild.Attributes["Version"].Value)
                    {
                        return;
                    }
                }
                Directory.Delete(destination, true);
            }

            foreach (XmlNode item in doc.FirstChild.ChildNodes)
            {
                if (item.Name == "Archive")
                {
                    string archiveDir = Path.Combine(destination, item.Attributes["Path"].Value);
                    Directory.CreateDirectory(archiveDir);
                    foreach (XmlNode file in item.ChildNodes)
                    {
                        CreateHardLinkForFile(file, archiveDir, container);
                    }
                }
                // Item is directly a file
                else
                {
                    CreateHardLinkForFile(item, destination, container);
                }
            }

            File.WriteAllLines(versionInfo, new string[]{ doc.FirstChild.Attributes["Version"].Value });

        }
    }
}
