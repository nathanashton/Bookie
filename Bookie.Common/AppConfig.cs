namespace Bookie.Common
{
    using System.Configuration;

    public static class AppConfig
    {
        public static void AddSetting(string in_key, string keyvalue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(in_key);
            config.AppSettings.Settings.Add(in_key, keyvalue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSetting");
        }

        public static string LoadSetting(string in_key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string keyvalue = ConfigurationManager.AppSettings[in_key];
            return keyvalue;
        }
    }
}