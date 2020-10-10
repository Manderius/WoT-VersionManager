using System;
using System.IO;
using VersionManager.Persistence;
using VersionManager.Utils;
using VersionManagerUI.Properties;
using VersionManagerUI.Services;

namespace VersionManagerUI.Utils
{
    class VMSettings
    {
        public static InstanceCache LoadSettings()
        {
            InstanceCache cache = new InstanceCache();

            if (Settings.Default.AppDirectory == string.Empty)
            {
                CreateDefaultSettings();
            }

            DataDeserializer dds = new DataContractXMLLoader();
            cache.AddInstance(dds);
            cache.AddInstance((DataSerializer)dds);

            DirectoryCache dirCache = new DirectoryCache();
            if (File.Exists(Settings.Default.DirectoryCacheFile))
            {
                dirCache = dds.Deserialize<DirectoryCache>(Settings.Default.DirectoryCacheFile);
            }
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

        private static void CreateDefaultSettings()
        {
            // TODO first run wizard

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            Settings.Default.AppDirectory = appDir;
            Settings.Default.DataDirectory = Path.Combine(appDir, "Data");
            Settings.Default.GameOutputDirectory = Path.Combine(appDir, "World of Tanks Versions");
            Settings.Default.ManagedVersionsFile = Path.Combine(Settings.Default.DataDirectory, "ManagedVersions.xml");
            Settings.Default.DirectoryCacheFile = Path.Combine(Settings.Default.DataDirectory, "DirectoryCache.xml");

            Settings.Default.Save();
        }
    }
}
