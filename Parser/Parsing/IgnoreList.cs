using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionSwitcher_Server.Parsing
{
    class IgnoreList
    {
        SortedSet<string> _ignored;
        public IgnoreList FromEnumerable(IEnumerable<string> ignored)
        {
            return new IgnoreList().SetIgnored(new SortedSet<string>(ignored));
        }

        public IgnoreList SetIgnored(SortedSet<string> ignored)
        {
            _ignored = ignored;
            return this;
        }
        public bool IsIgnored(string relativePath)
        {
            if (_ignored.Contains(relativePath))
            {
                return true;
            }
            else
            {
                foreach (string ignoredFile in _ignored)
                {
                    if (relativePath.StartsWith(ignoredFile))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
