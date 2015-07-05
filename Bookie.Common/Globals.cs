namespace Bookie.Common
{
    using System;
    using System.IO;

    public static class Globals
    {
        public static string ApplicationName = "Bookie";

        public static string DatabaseName = "bookie.sdf";

        public static string CurrentUser = "Nathan Ashton";

        public static string ApplicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ApplicationName);

        public static string ApplicationDatabaseFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ApplicationName + @"\" + DatabaseName);

        public static string BookFolder { get; set; }

        public static string CoverImageFolder = ApplicationPath + @"\Covers\";

        public static string DbConnectionString = @"Data Source = " + ApplicationDatabaseFullPath;

        public static string VersionNumber = "ALPHA";

        public enum Languages
        {
            English,
            German,
            French
        }

        public static Languages Language;
    }
}