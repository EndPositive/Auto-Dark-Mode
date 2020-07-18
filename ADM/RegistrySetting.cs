using ADM.Helpers;
using Microsoft.Win32;

namespace ADM
{
    public class RegistrySetting
    {
        public readonly string Application;
        public readonly RegistryKey Key;
        public readonly string Name;
        public readonly int Light;
        public readonly int Dark;

        public RegistrySetting(string keyString)
        {
            var themeRegistry = keyString.Split("###");
            Application = themeRegistry[0];
            Key = RegistryHelper.ParseKey(themeRegistry[1]);
            Name = themeRegistry[2];
            Light = int.Parse(themeRegistry[3]);
            Dark = int.Parse(themeRegistry[4]);
        }
    }
}