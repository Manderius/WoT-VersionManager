using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using VSUI.Data;

namespace VSUI.Parsers
{
    class LocalVersionParser
    {
        public static LocalGameVersion FromDirectory(string path)
        {
            string versionXml = Path.Combine(path, "version.xml");
            if (Directory.Exists(path) && File.Exists(versionXml))
            {
                XElement xml = XElement.Parse(File.ReadAllText(versionXml));
                string versionText = xml.Element("version").Value.Trim();

                Regex regex = new Regex(@"v\.([0-9\.]+)\s");
                Match match = regex.Match(versionText);

                return new LocalGameVersion(match.Groups[1].Value, path);
            }

            return null;
        }
    }
}
