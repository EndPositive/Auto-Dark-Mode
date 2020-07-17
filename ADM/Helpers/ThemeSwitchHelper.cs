using System;
using ADM.Properties;

namespace ADM.Helpers
{
    public static class ThemeSwitchHelper
    {
        public enum Mode
        {
            Dark, Light
        }

        public static void Switch(Mode theme)
        {
            foreach (var keyString in Settings.Default.Keys)
            {
                var key = new KeySerializerHelper(keyString);
                key.Key.SetValue(key.Name, theme == Mode.Light ? key.Light : key.Dark);
            }
        }

        public static Mode? Now()
        {
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Settings.Default.StartTime.Hour, Settings.Default.StartTime.Minute, 0);
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Settings.Default.EndTime.Hour, Settings.Default.EndTime.Minute, 0);

            if (DateTime.Now >= startTime && DateTime.Now < endTime.Add(new TimeSpan(1,0,0,0))) return Mode.Dark;
            if (DateTime.Now > endTime) return Mode.Light;
            if (DateTime.Now <= endTime) return Mode.Dark;
            return null;
        }
    }
}