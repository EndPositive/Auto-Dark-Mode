using ADM.Helpers;

namespace ADM
{
    public partial class App
    {
        public App()
        {
            var themeSwitchService = new ThemeSwitchService();
            var startupHelper = new StartupHelper();
            new TrayIconHelper(themeSwitchService, startupHelper, this);
        }
    }
}
