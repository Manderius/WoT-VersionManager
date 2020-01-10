using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Windows.Forms;

namespace VersionSwitcher_Server
{
    class FolderCompare
    {
        public static List<string> GetDirectoryContent(string path, string prefix)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            List<string> result = info.EnumerateFiles().Select(x => Path.Combine(prefix, x.Name)).ToList();
            foreach (DirectoryInfo dir in info.EnumerateDirectories())
            {
                result.AddRange(GetDirectoryContent(dir.FullName, Path.Combine(prefix, dir.Name)));
            }

            return result;
        }

        private static long GetTotalFileSize(string basedir, IEnumerable<string> files)
        {
            long size = 0;
            bool isFirst = true;
            bool isSecond = true;

            List<CompressedFile> contents = new List<CompressedFile>();
            string lastPkgPath = "";

            foreach (string file in files)
            {

                if (file.StartsWith("[NOT]"))
                {
                    continue;
                }

                if (file.StartsWith("[CMP]") || file.StartsWith("[IFO]") || file.StartsWith("[ISO]"))
                {
                    string prefix = file.Substring(1, 3);
                    if ((prefix == "IFO" && isFirst) || (prefix == "ISO" && isSecond))
                    {
                        string[] fileData = file.Substring(5).Split('|');
                        string pkgPath = Path.Combine(basedir, fileData[0]);
                        if (File.Exists(pkgPath))
                        {
                            if (lastPkgPath != pkgPath)
                            {
                                contents = GetFilesFromArchive(pkgPath);
                                lastPkgPath = pkgPath;
                            }
                            foreach (CompressedFile cfile in contents)
                            {
                                if (cfile.FullName == fileData[1])
                                {
                                    if (prefix == "IFO")
                                    {
                                        isSecond = false;
                                    }
                                    else if (prefix == "ISO")
                                    {
                                        isFirst = false;
                                    }
                                    if (cfile.Length > 0)
                                    {
                                        size += cfile.CompressedLength;
                                    }
                                    continue;
                                }

                            }
                        }
                    }

                }
                else
                {
                    FileInfo fi = new FileInfo(Path.Combine(basedir, file));
                    size += fi.Length;
                }

            }

            return size;
        }

        public static List<CompressedFile> GetFilesFromArchive(string path)
        {
            List<CompressedFile> result = new List<CompressedFile>();

            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    result.Add(new CompressedFile(entry.Name, entry.FullName, entry.Length, entry.CompressedLength));
                }
            }

            return result;
        }

        private static List<string> GetExtraFiles(List<string> originalContent, List<string> comparedContent)
        {
            // Get files not present in the second one
            List<string> files = originalContent.Where(x => !comparedContent.Contains(x)).ToList();

            return files;
        }

        private static List<CompressedFile> GetExtraFiles(List<CompressedFile> originalCompress, List<CompressedFile> comparedCompress)
        {
            return originalCompress.Where(x => !comparedCompress.Contains(x)).ToList();

        }

        private static List<CompressedFile> GetFilesWithDifferentContent(List<CompressedFile> originalCompress, List<CompressedFile> comparedCompress)
        {
            return originalCompress.Where(x => !comparedCompress.Contains(x)).ToList();
        }

        /// <summary>
        /// Names of files that exist in both paths, but have different contents
        /// </summary>
        /// <param name="originBase"></param>
        /// <param name="comparedBase"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private static List<string> GetFilesWithDifferentContent(string originBase, string comparedBase, IEnumerable<string> files)
        {
            List<string> differentFiles = new List<string>();

            foreach (string file in files)
            {
                FileInfo original = new FileInfo(Path.Combine(originBase, file));
                FileInfo compared = new FileInfo(Path.Combine(comparedBase, file));
                if (file.EndsWith(".pkg"))
                {
                    differentFiles.Add("[NOT]" + file); // Do NOT count into total file size
                    List<CompressedFile> originalCompress = GetFilesFromArchive(original.FullName);
                    List<CompressedFile> comparedCompress = GetFilesFromArchive(compared.FullName);

                    List<CompressedFile> firstOnly = GetExtraFiles(originalCompress, comparedCompress);
                    List<CompressedFile> secondOnly = GetExtraFiles(comparedCompress, originalCompress);

                    differentFiles.AddRange(firstOnly.Select(x => "[IFO]" + file + "|" + x.FullName)); // In First Only
                    differentFiles.AddRange(secondOnly.Select(x => "[ISO]" + file + "|" + x.FullName)); // In Second Only

                    foreach (CompressedFile cf in originalCompress.Intersect(comparedCompress, new CompressedFileEqualityComparer()))
                    {
                        CompressedFile second = comparedCompress.Find(x => x.FullName == cf.FullName);
                        if (cf.Length != second.Length)
                        {
                            differentFiles.Add("[CMP]" + file + "|" + cf.FullName); // CoMPressed, present in both but different
                        }
                    }

                }
                else if (original.Length != compared.Length)
                {
                    differentFiles.Add(file);
                }
            }

            return differentFiles;
        }

        public static void Compare(string origin, string compared)
        {

            DirectoryInfo originDir = new DirectoryInfo(origin);
            DirectoryInfo comparedDir = new DirectoryInfo(compared);

            List<string> originContent = GetDirectoryContent(origin, "");
            List<string> comparedContent = GetDirectoryContent(compared, "");

            List<string> firstHasExtra = GetExtraFiles(originContent, comparedContent);
            List<string> secondHasExtra = GetExtraFiles(comparedContent, originContent);
            //List<string> differentFiles = new List<string>();

            List<string> differentContents = GetFilesWithDifferentContent(origin, compared, originContent.Intersect(comparedContent));

            /*MessageBox.Show(String.Format("{0} has {1} extra files\nSize: {2} MB", origin, firstHasExtra.Count, GetTotalFileSize(origin, firstHasExtra) / (1024 * 1024)));
            MessageBox.Show(String.Format("{0} has {1} extra files\nSize: {2} MB", compared, secondHasExtra.Count, GetTotalFileSize(compared, secondHasExtra) / (1024 * 1024)));
            MessageBox.Show(String.Format("Files with the same name but different contents have {0} MB and {1} MB respectively", GetTotalFileSize(origin, differentContents) / (1024 * 1024), GetTotalFileSize(compared, differentContents) / (1024 * 1024)));
            */

            List<string> firstOnly = differentContents.Where(x => (!x.StartsWith("[ISO]") && !x.StartsWith("[NOT]"))).ToList();
            List<string> secondOnly = differentContents.Where(x => (!x.StartsWith("[IFO]") && !x.StartsWith("[NOT]"))).ToList();

            string result = String.Format("{0} has {1} extra files\nSize: {2} MB", origin, firstHasExtra.Count, GetTotalFileSize(origin, firstHasExtra) / (1024 * 1024)) + "\n\n" +
                            String.Format("{0} has {1} extra files\nSize: {2} MB", compared, secondHasExtra.Count, GetTotalFileSize(compared, secondHasExtra) / (1024 * 1024)) + "\n\n" +
                            String.Format("Files with the same name but different contents have {0:N0} MB and {1:N0} MB respectively", GetTotalFileSize(origin, firstOnly) / (1024 * 1024), GetTotalFileSize(compared, secondOnly) / (1024 * 1024));

            MessageBox.Show(result);
        }
    }
}
