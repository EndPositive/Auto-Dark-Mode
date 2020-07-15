using System;
using System.Windows;
using ADM.Helpers;
using ADM.Properties;

namespace ADM
{
    public partial class SettingsWindow
    {
        private readonly ThemeSwitchService _themeSwitchService;
        private readonly StartupHelper _startupHelper;

        public SettingsWindow(ThemeSwitchService themeSwitchService, StartupHelper startupHelper)
        {
            _themeSwitchService = themeSwitchService;
            _startupHelper = startupHelper;

            InitializeComponent();

            if (Settings.Default.Enabled)
            {
                EnableThemingCheckbox.IsChecked = true;
                TimePickers.Visibility = Visibility.Visible;
            }
            else
            {
                EnableThemingCheckbox.IsChecked = false;
                TimePickers.Visibility = Visibility.Collapsed;
            }

            StartTimePicker.SelectedDateTime = Settings.Default.StartTime;
            EndTimePicker.SelectedDateTime = Settings.Default.EndTime;

            SaveButton.Visibility = Visibility.Collapsed;

            // TODO: Detect Windows theme switch while window is open
            StartupToggleSwitch.IsOn = Settings.Default.Startup;
        }

        private void EnableThemingCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Enabled = EnableThemingCheckbox.IsChecked ?? false;
            var visibility = Settings.Default.Enabled ? Visibility.Visible : Visibility.Collapsed;
            TimePickers.Visibility = visibility;
            Settings.Default.Save();
            Request_Save(this, null);
        }

        private void StartupToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            Settings.Default.Startup = StartupToggleSwitch.IsOn;
            Settings.Default.Save();
            _startupHelper.Apply();
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveButton.Content = "Saving...";

            Settings.Default.StartTime = StartTimePicker.SelectedDateTime ?? throw new Exception("Big fuck");
            Settings.Default.EndTime = EndTimePicker.SelectedDateTime ?? throw new Exception("Big fuck");

            Settings.Default.Save();

            await _themeSwitchService.Restart();

            SaveButton.Content = "Saved!";
        }

        private void Request_Save(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            SaveButton.Content = "Apply";
            SaveButton.Visibility = Visibility.Visible;
        }
    }
}
    