using System;
using System.Diagnostics;
using ADM.Properties;
using Microsoft.Win32;

namespace ADM.Helpers
{
    public class StartupHelper
    {
        private readonly RegistryKey _key;
        private readonly string _exe;
        private const string KeyName = "ADM";

        public StartupHelper()
        {
            var startupRegistry = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            _key = Registry.CurrentUser.OpenSubKey(startupRegistry, true);
            if (_key == null) throw new Exception("Could not find startup registry: " + startupRegistry);
            
            _exe = Process.GetCurrentProcess().MainModule?.FileName;
            Apply();
        }

        public void Apply()
        {
            if (Settings.Default.Startup && !IsAdded()) Add();
            else if (!Settings.Default.Startup && IsAdded()) Remove();
        }

        private void Add()
        {
            if (IsAdded()) return;

            _key.SetValue(KeyName, _exe);
            
            NotificationHelper.New("Auto Dark Mode", "This app is set to run at startup.");
        }

        private void Remove()
        {
            if (!IsAdded()) return;
            
            _key.DeleteValue(KeyName);
            
            NotificationHelper.New("Auto Dark Mode", "This app is removed from startup.");
        }

        private bool IsAdded()
        {
            return (string) _key.GetValue(KeyName) == _exe;
        }
    }
}