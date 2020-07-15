using System.Windows.Forms;
using ADM.Properties;

namespace ADM.Helpers
{
    public class TrayIconHelper
    {
        private readonly ToolStripMenuItem _forceDarkMenuItem = new ToolStripMenuItem("Force Dark Mode");
        private readonly ToolStripMenuItem _forceLightMenuItem = new ToolStripMenuItem("Force Light Mode");
        private readonly ToolStripMenuItem _openConfigMenuItem = new ToolStripMenuItem("Open Config");
        private readonly ToolStripMenuItem _exitMenuItem = new ToolStripMenuItem("Exit");

        private readonly ThemeSwitchService _themeSwitchService;
        private readonly StartupHelper _startupHelper;
        
        public TrayIconHelper(ThemeSwitchService themeSwitchService, StartupHelper startupHelper, System.Windows.Application app)
        {
            _themeSwitchService = themeSwitchService;
            _startupHelper = startupHelper;
            
            var notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resource.TrayIcon;
            notifyIcon.Text = "Auto Dark Mode";
            notifyIcon.MouseDown += TrayIcon_Click;
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(_forceDarkMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add(_forceLightMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add(_openConfigMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add(_exitMenuItem);
            
            _forceDarkMenuItem.Click += (_, __) => ThemeSwitchHelper.Switch(ThemeSwitchHelper.Mode.Dark);
            _forceLightMenuItem.Click += (_, __) => ThemeSwitchHelper.Switch(ThemeSwitchHelper.Mode.Light);
            _openConfigMenuItem.Click += (_, __) => TrayIcon_Click(this, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
            _exitMenuItem.Click += (_, __) => app.Shutdown();
            
            notifyIcon.Visible = true;
        }

        private void TrayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var settingsWindow = new SettingsWindow(_themeSwitchService, _startupHelper);
            settingsWindow.Show();
        }
    }
}