using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ADM.Properties;
using Microsoft.Win32;

namespace ADM.Helpers
{
    public static class RegistryHelper
    {
        public static ObservableCollection<string> GetObservable()
        {
            var observableCollection = new ObservableCollection<string>();
            
            foreach (var keyString in Settings.Default.Keys)
            {
                var key = new RegistrySetting(keyString);
                observableCollection.Add(key.Application);
            }

            return observableCollection;
        }

        public static IEnumerable<RegistrySetting> GetSettings()
        {
            var settings = new List<RegistrySetting>();
            
            foreach (var keyString in Settings.Default.Keys)
            {
                settings.Add(new RegistrySetting(keyString));
            }

            return settings;
        }

        public static void Add(string application, string key, string name, string light, string dark)
        {
            Settings.Default.Keys.Add(application + "###" + key + "###" + name + "###" + light + "###" + dark);
            Settings.Default.Save();
        }

        public static void Remove(string application)
        {
            foreach (var keyString in Settings.Default.Keys)
            {
                var key = new RegistrySetting(keyString);
                if (key.Application != application) continue;
                Settings.Default.Keys.Remove(keyString);
                Settings.Default.Save();
                break;
            }
        }

        public static void RestoreUi()
        {
            Add("System Apps", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "1", "0");
        }

        public static void RestoreApps()
        {
            Add("System Apps", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1", "0");
        }

        public static RegistryKey ParseKey(string key)
        {
            key = Regex.Replace(Regex.Replace(key, @"HKEY_CURRENT_USER\\", ""), @"\\", @"\\\\");
            return Registry.CurrentUser.OpenSubKey(key);
        }

        public static bool IsValidApplication(string application)
        {
            return !GetObservable().Contains(application);
        }

        public static bool IsValidKey(string key)
        {
            key = Regex.Replace(Regex.Replace(key, @"HKEY_CURRENT_USER\\", ""), @"\\", @"\\\\");
            return ParseKey(key) != null;
        }

        public static bool IsValidName(string name, string key)
        {
            if (ParseKey(key) == null) return false;
            return ParseKey(key).GetValue(name) != null;
        }

        public static bool IsValidValue(string value)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                int.Parse(value);
                return true;
            }
            catch (System.FormatException)
            {
                return false;
            }
        }
    }
}