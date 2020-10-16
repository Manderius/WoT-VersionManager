using System;
using System.IO;
using VersionManager.Persistence;
using VersionManager.Utils;
using VersionManagerUI.Properties;
using VersionManagerUI.Services;

namespace VersionManagerUI.Utils
{
    static class VMSettings
    {
        public static bool IsFirstRun => Settings.Default.AppDirectory == string.Empty;
        public static InstanceCache LoadSettings()
        {
            InstanceCache cache = new InstanceCache();

            DataDeserializer dds = new DataContractXMLLoader();
            cache.AddInstance(dds);
            cache.AddInstance((DataSerializer)dds);

            DirectoryCache dirCache = new DirectoryCache();
            if (File.Exists(Settings.Default.DirectoryCacheFile))
            {
                dirCache = dds.Deserialize<DirectoryCache>(Settings.Default.DirectoryCacheFile);
            }
            dirCache.ContainerPath = Settings.Default.ContainerDirectory;
            cache.AddInstance(dirCache);

            ManagedVersionsService mvs = new ManagedVersionsService();
            if (File.Exists(Settings.Default.ManagedVersionsFile))
            {
                mvs.Load(dds);
            }
            mvs.Serializer = (DataSerializer)dds;
            cache.AddInstance(mvs);

            return cache;
        }

        public static void CreateDefaultSettings()
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            Settings.Default.AppDirectory = appDir;
            Settings.Default.DataDirectory = Path.Combine(appDir, "Data");
            Settings.Default.GameOutputDirectory = Path.Combine(appDir, "World of Tanks Versions");
            Settings.Default.ManagedVersionsFile = Path.Combine(Settings.Default.DataDirectory, "ManagedVersions.xml");
            Settings.Default.DirectoryCacheFile = Path.Combine(Settings.Default.DataDirectory, "DirectoryCache.xml");
            Settings.Default.ContainerDirectory = Path.Combine(Settings.Default.DataDirectory, "Container");
            Settings.Default.VersionDataDirectory = Path.Combine(Settings.Default.DataDirectory, "VersionData");

            Directory.CreateDirectory(Settings.Default.GameOutputDirectory);
            Directory.CreateDirectory(Settings.Default.DataDirectory);
            Directory.CreateDirectory(Settings.Default.ContainerDirectory);
            Directory.CreateDirectory(Settings.Default.VersionDataDirectory);

            Settings.Default.Save();
        }
    }
}
