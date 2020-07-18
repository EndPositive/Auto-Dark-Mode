using System.Collections.Specialized;
using ADM.Helpers;
using ADM.Properties;

namespace ADM
{
    public partial class App
    {
        public App()
        {
            if (Settings.Default.Keys == null)
            {
                Settings.Default.Keys = new StringCollection();
                RegistryHelper.RestoreApps();
                RegistryHelper.RestoreUi();
            }
            var themeSwitchService = new ThemeSwitchService();
            var startupHelper = new StartupHelper();
            new TrayIconHelper(themeSwitchService, startupHelper, this);
        }
    }
}
