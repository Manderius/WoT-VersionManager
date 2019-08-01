using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AnyReplay_Player
{
    class FileClassifier
    {
        SortedSet<string> ignoredFiles = new SortedSet<string>();

        public FileClassifier() {
            LoadIgnoredFiles();
        }

        private void LoadIgnoredFiles() {
            foreach (string s in File.ReadAllLines("ignored.txt")) {
                ignoredFiles.Add(s);

            }
        }

        private string GetSHAFromFile(string path)
        {
            Stream st = File.Open(path, FileMode.Open);
            string result = GetSHAFromStream(st);
            st.Close();
            return result;
            
        }

        private string GetSHAFromStream(Stream stream)
        {
            string strResult = "";
            string strHashData = "";

            byte[] arrbytHashValue;

            System.Security.Cryptography.SHA1CryptoServiceProvider oSHA1Hasher =
                       new System.Security.Cryptography.SHA1CryptoServiceProvider();

            try
            {
                arrbytHashValue = oSHA1Hasher.ComputeHash(stream);

                strHashData = System.BitConverter.ToString(arrbytHashValue);
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",
                         System.Windows.Forms.MessageBoxButtons.OK,
                         System.Windows.Forms.MessageBoxIcon.Error,
                         System.Windows.Forms.MessageBoxDefaultButton.Button1);
            }

            return strResult;
        }

        private void WriteFileNode(XmlWriter writer, string path, string name, string hash)
        {
            writer.WriteStartElement("File");
            writer.WriteAttributeString("Path", path);
            writer.WriteAttributeString("Name", name);
            writer.WriteElementString("Hash", hash);
            writer.WriteEndElement();
        }

        public void Classify(string sourceDir, string containerDir, string outFile, string version)
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
                if (ignoredFiles.Contains(relativePath))
                {
                    ignored = true;
                }
                else
                {
                    foreach (string ignoredFile in ignoredFiles)
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

                        List<CompressedFile> result = new List<CompressedFile>();

                        using (ZipArchive archive = ZipFile.OpenRead(filepath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.Length == 0)
                                    continue;

                                sha = GetSHAFromStream(entry.Open()) + entry.FullName.GetHashCode();
                                string path = Path.Combine(containerDir, sha);

                                WriteFileNode(writer, entry.FullName.Substring(0, entry.FullName.Length - entry.Name.Length), entry.Name, sha);

                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                    entry.ExtractToFile(Path.Combine(path, entry.Name));

                                    /*string fullPath = Path.Combine(path, entry.FullName);
                                    string basedir = fullPath.Substring(0, fullPath.Length - entry.Name.Length);
                                    Directory.CreateDirectory(basedir);

                                    entry.ExtractToFile(fullPath);

                                    string tempName = Path.Combine(destination, details.Name);

                                    ZipFile.CreateFromDirectory(path, tempName, CompressionLevel.NoCompression, false);

                                    foreach (string dirName in Directory.GetDirectories(path))
                                    {
                                        Directory.Delete(dirName, true);
                                    }
                                    foreach (string fileName in Directory.GetFiles(path))
                                    {
                                        File.Delete(fileName);
                                    }

                                    File.Move(tempName, Path.Combine(path, details.Name));*/

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
                        
                        string path = Path.Combine(containerDir, sha);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            File.Copy(filepath, Path.Combine(path, details.Name));
                        }

                        
                        WriteFileNode(writer, relativePath, details.Name, sha);
                    }

                    
                }
            }

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }
    }
}
