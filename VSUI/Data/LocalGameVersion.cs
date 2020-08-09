using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSUI.Data
{
    public class LocalGameVersion: GameVersion
    {
        public string Path { get; set; }

        public LocalGameVersion(string version, string path): base(version)
        {
            Path = path;
        }
    }
}
