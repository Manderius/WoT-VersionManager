using System;
using System.IO;
using System.Xml.Serialization;

namespace VersionSwitcher_Server.Filesystem
{
    public class FileEntity : BaseEntity
    {
        public string Hash;
        [XmlIgnore]
        public Func<Stream> Contents;

        public FileEntity()
        {
        }

        public FileEntity(string name) : base(name)
        {
        }

        public FileEntity(string name, Func<Stream> contents) : base(name)
        {
            Contents = contents;
        }

        public FileEntity(string name, string hash): base(name)
        {
            Hash = hash;
        }
    }
}
