using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VersionManager.Filesystem
{
    [DataContract(Name = "File", Namespace = "VersionManager.Filesystem")]
    public class FileEntity : BaseEntity
    {
        [DataMember]
        public string Hash;
        [DataMember]
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

        public override int GetHashCode()
        {
            if (Hash != null && Size != 0)
            {
                return new { Hash, Size, Name, RelativePath }.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileEntity);
        }

        public bool Equals(FileEntity other)
        {
            if (other == null)
                return false;

            if (Hash != null && Size != 0 && other.Hash != null && other.Size != 0)
                return Hash == other.Hash && Size == other.Size && base.Equals(other);

            return base.Equals(other);
        }
    }
}
