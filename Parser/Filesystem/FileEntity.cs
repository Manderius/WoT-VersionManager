using System;
using System.IO;
using System.Xml.Serialization;

namespace VersionManager.Filesystem
{
    public class FileEntity : BaseEntity
    {
        public string Hash;
        public long Size;
        [XmlIgnore]
        public Func<Stream> Contents;

        public FileEntity()
        {
        }

        public FileEntity(string name) : base(name)
        {
        }

        public FileEntity(string name, long size) : base(name)
        {
            Size = size;
        }

        public FileEntity(string name, Func<Stream> contents) : base(name)
        {
            Contents = contents;
        }

        public FileEntity(string name, Func<Stream> contents, long size) : base(name)
        {
            Contents = contents;
            Size = size;
        }

        public FileEntity(string name, string hash) : base(name)
        {
            Hash = hash;
        }

        public FileEntity(string name, string hash, long size) : base(name)
        {
            Hash = hash;
            Size = size;
        }
    }
}
