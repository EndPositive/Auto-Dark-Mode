using System.Text.RegularExpressions;
using ADM.Properties;
using Microsoft.Win32;

namespace ADM.Helpers
{
    public class KeySerializerHelper
    {
        private const string KeysFolder = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string AppsUseLightThemeKey = "AppsUseLightTheme";
        private const string SystemUsesLightThemeKey = "SystemUsesLightTheme";

        public readonly string Application;
        public readonly RegistryKey Key;
        public readonly string Name;
        public readonly int Light;
        public readonly int Dark;
        
        public KeySerializerHelper(string collectionString)
        {
            var themeRegistry = collectionString.Split("###");
            Application = themeRegistry[0];
            Key = Registry.CurrentUser.OpenSubKey(ParseKey(themeRegistry[1]), true);
            Name = themeRegistry[2];
            Light = int.Parse(themeRegistry[3]);
            Dark = int.Parse(themeRegistry[4]);
        }
        
        public static string ToString(string application, string key, string name, string light, string dark)
        {
            return application + "###" + key + "###" + name + "###" + light + "###" + dark;
        }

        public static string ParseKey(string key)
        {
            return Regex.Replace(Regex.Replace(key, @"HKEY_CURRENT_USER\\", ""), @"\\", @"\\\\");
        }

        public static void AddUI()
        {
            Settings.Default.Keys.Add(ToString("System UI", KeysFolder, SystemUsesLightThemeKey,
                "1", "0"));
            Settings.Default.Save();
        }

        public static void AddApps()
        {
            Settings.Default.Keys.Add(ToString("System Apps", KeysFolder, AppsUseLightThemeKey,
                "1", "0"));
            Settings.Default.Save();
        }
    }
}