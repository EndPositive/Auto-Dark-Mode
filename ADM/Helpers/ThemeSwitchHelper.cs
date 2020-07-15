using System;
using ADM.Properties;
using Microsoft.Win32;

namespace ADM.Helpers
{
    public static class ThemeSwitchHelper
    {
        private const string KeysFolder =
            "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";
        private const string AppsUseLightThemeKey = "AppsUseLightTheme";
        private const string SystemUsesLightThemeKey = "SystemUsesLightTheme";
        
        public enum Mode
        {
            Dark, Light
        }

        public static void Switch(Mode theme)
        {
            Registry.SetValue(KeysFolder, AppsUseLightThemeKey, (int) theme);
            Registry.SetValue(KeysFolder, SystemUsesLightThemeKey, (int) theme);
        }

        public static Mode? Now()
        {
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Settings.Default.StartTime.Hour, Settings.Default.StartTime.Day, 0);
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Settings.Default.EndTime.Hour, Settings.Default.EndTime.Day, 0);

            if (DateTime.Now >= startTime && DateTime.Now < endTime.Add(new TimeSpan(1,0,0,0))) return Mode.Dark;
            if (DateTime.Now > endTime) return Mode.Light;
            if (DateTime.Now <= endTime) return Mode.Dark;
            return null;
        }
    }
}