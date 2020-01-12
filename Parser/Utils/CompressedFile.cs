using System;
using System.Collections.Generic;

namespace VersionSwitcher_Server.Utils
{
    class CompressedFile
    {
        public string Name;
        public string FullName;
        public long Length;
        public long CompressedLength;

        public CompressedFile(string name, string fullName, long length, long compressedLength)
        {
            Name = name;
            FullName = fullName;
            Length = length;
            CompressedLength = compressedLength;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            CompressedFile file = (CompressedFile)obj;
            return (Name == file.Name) && (FullName == file.FullName);
        }
    }

    class CompressedFileEqualityComparer : IEqualityComparer<CompressedFile>
    {
        public bool Equals(CompressedFile item1, CompressedFile item2)
        {
            if (item1 == null && item2 == null)
                return true;
            else if ((item1 != null && item2 == null) ||
                    (item1 == null && item2 != null))
                return false;

            return item1.Equals(item2) &&
                   item1.Equals(item2);
        }

        public int GetHashCode(CompressedFile item)
        {
            return new { item.Name, item.FullName }.GetHashCode();
        }
    }
}
