using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Xml;
using VersionSwitcher_Server.Filesystem;
using VersionSwitcher_Server.Hashing;

namespace VersionSwitcher_Server
{
    class FileClassifier
    {
        SortedSet<string> _ignoredFiles = new SortedSet<string>();
        string _containerDir;
        HashSet<string> _existingDirs = new HashSet<string>();
        HashProvider _hashProvider;

        public FileClassifier(string container, HashProvider hashProvider, SortedSet<string> ignoredFiles)
        {
            _hashProvider = hashProvider;
            _containerDir = container;
            _ignoredFiles = ignoredFiles;
            LoadIgnoredFiles();
            //LoadExistingDirs(_containerDir);
        }

        private void LoadIgnoredFiles()
        {
            foreach (string s in File.ReadAllLines("ignored.txt"))
            {
                _ignoredFiles.Add(s);
            }
        }

        private void LoadExistingDirs(string root, int depth = 0)
        {
            foreach (DirectoryInfo dir in new DirectoryInfo(root).EnumerateDirectories())
            {
                _existingDirs.Add(dir.FullName);
                if (depth == 0)
                {
                    LoadExistingDirs(dir.FullName, depth + 1);
                }
            }
        }

        private string GetSHAFromFile(string path)
        {
            //Stream st = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16 * 1024 * 1024);
            Stream st = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string result = _hashProvider.FromStream(st);
            st.Close();
            return result;

        }

        private void WriteFileNode(XmlWriter writer, string path, string name, string hash)
        {
            //writer.WriteStartElement("File");
            //writer.WriteAttributeString("Path", path);
            //writer.WriteAttributeString("Name", name);
            //writer.WriteElementString("Hash", hash);
            //writer.WriteEndElement();
        }


        public void Parse(DirectoryInfo directory, DirectoryEntity parent, int prefixLength, bool computeHash)
        {
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                string relativePath = file.FullName.Substring(prefixLength);

                // TODO skip if ignored

                if (file.Extension == ".pkg")
                {
                    ParsePackage(file, parent, computeHash);
                }
                else
                {
                    FileEntity fileEnt = (computeHash) ? new FileEntity(file.Name, _hashProvider.FromStream(new FileStream(file.FullName, FileMode.Open))) : new FileEntity(file.Name);
                    parent.Add(fileEnt);
                }
            }

            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                DirectoryEntity child = new DirectoryEntity(dir.Name);
                parent.Add(child);
                Parse(dir, child, prefixLength, computeHash);
            }
        }

        private void ParsePackage(FileInfo package, DirectoryEntity parent, bool computeHash)
        {
            DirectoryEntity root = new DirectoryEntity(package.Name);
            parent.Add(root);

            using (ZipArchive archive = ZipFile.OpenRead(package.FullName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Directory
                    if (entry.Length == 0)
                        continue;

                    string relativePath = entry.FullName.Substring(0, entry.FullName.Length - entry.Name.Length);
                    FileEntity file = (computeHash) ? new FileEntity(entry.Name, _hashProvider.FromStream(entry.Open())) : new FileEntity(entry.Name);
                    (root.GetEntityFromRelativePath(relativePath, true) as DirectoryEntity).Add(file);
                }
            }
        }

        public void Classify(string sourceDir, string outFile, string version)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(outFile, settings);

            writer.WriteStartElement("FileData");
            writer.WriteAttributeString("Version", version);

            List<string> contents = FolderCompare.GetDirectoryContent(sourceDir, sourceDir);

            foreach (string filepath in contents)
            {
                string relativePath = filepath.Substring(sourceDir.Length);

                // Filter out unnecessary files
                bool ignored = false;
                if (_ignoredFiles.Contains(relativePath))
                {
                    ignored = true;
                }
                else
                {
                    foreach (string ignoredFile in _ignoredFiles)
                    {
                        if (relativePath.StartsWith(ignoredFile))
                        {
                            ignored = true;
                            break;
                        }
                    }
                }

                if (ignored)
                    continue;
                // Filtering done


                if (File.Exists(filepath))
                {
                    FileInfo details = new FileInfo(filepath);
                    string sha = "";

                    // Found a package
                    if (details.Name.EndsWith(".pkg"))
                    {
                        writer.WriteStartElement("Archive");
                        writer.WriteAttributeString("Path", filepath.Substring(sourceDir.Length).Replace('\\', '/'));
                        writer.WriteAttributeString("Name", details.Name);

                        using (ZipArchive archive = ZipFile.OpenRead(filepath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.Length == 0)
                                    continue;

                                sha = _hashProvider.FromStream(entry.Open()) + entry.FullName.GetHashCode();
                                string path = GetFileDirectory(sha);

                                WriteFileNode(writer, entry.FullName.Substring(0, entry.FullName.Length - entry.Name.Length), entry.Name, sha);

                                if (!_existingDirs.Contains(path))
                                {
                                    Directory.CreateDirectory(path);
                                    entry.ExtractToFile(Path.Combine(path, entry.Name));
                                }
                            }
                        }
                        // End Archive element
                        writer.WriteEndElement();

                    }
                    // Normal file
                    else
                    {
                        //string relativePath = filepath.Substring(sourceDir.Length);
                        sha = GetSHAFromFile(filepath) + relativePath.GetHashCode();
                        relativePath = relativePath.Substring(0, relativePath.Length - details.Name.Length);

                        string path = GetFileDirectory(sha);
                        if (!_existingDirs.Contains(path))
                        {
                            Directory.CreateDirectory(path);
                            File.Copy(filepath, Path.Combine(path, details.Name));
                            _existingDirs.Add(path);
                        }


                        WriteFileNode(writer, relativePath, details.Name, sha);
                    }


                }
            }

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        private string GetFileDirectory(string hash)
        {
            string topDir = hash.Substring(0, 3);
            string dir = hash.Substring(3);
            string fullPath = Path.Combine(_containerDir, topDir, dir);
            return fullPath;
        }
    }
}
