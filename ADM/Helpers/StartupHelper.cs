using System;
using System.Windows.Forms;
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
            _key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (_key == null) throw new Exception("Startup registry not found");
            
            var dllPath = Application.ExecutablePath;
            _exe = dllPath.Substring(0, dllPath.Length - 3) + "exe";

            Apply();
        }

        public void Apply()
        {
            if (Settings.Default.Startup && !IsAdded()) Add();
            else if (!Settings.Default.Startup && IsAdded()) Remove();
        }

        private void Add()
        {
            if (IsAdded()) throw new Exception("Already added");

            _key.SetValue(KeyName, _exe);
            
            NotificationHelper.New("Auto Dark Mode", "This app is set to run at startup.");
        }

        private void Remove()
        {
            if (!IsAdded()) throw new Exception("Key does not exist");
            _key.DeleteValue(KeyName);
            
            NotificationHelper.New("Auto Dark Mode", "This app is removed from startup.");
        }

        private bool IsAdded()
        {
            return (string) _key.GetValue(KeyName) == _exe;
        }
    }
}