using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ADM.Helpers;
using ADM.Properties;

namespace ADM.Views
{
    public partial class SettingsWindow
    {
        private readonly ThemeSwitchService _themeSwitchService;
        private readonly StartupHelper _startupHelper;
        private readonly ObservableCollection<string> _themeRegistry;

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

            _themeRegistry = new ObservableCollection<string>();
            UpdateRegistry();
            KeysListBox.ItemsSource = _themeRegistry;

            // TODO: Detect Windows theme switch while window is open
            StartupToggleSwitch.IsOn = Settings.Default.Startup;
        }

        private void UpdateRegistry()
        {
            bool containsSystemApps = false, containsSystemUi = false;
            _themeRegistry.Clear();
            foreach (var keyString in Settings.Default.Keys)
            {
                var key = new KeySerializerHelper(keyString);
                _themeRegistry.Add(key.Application);

                if (key.Application == "System UI") containsSystemUi = true;
                else if (key.Application == "System Apps") containsSystemApps = true;
            }

            if (!containsSystemApps) RestoreAppsButton.Visibility = Visibility.Visible;
            if (!containsSystemUi) RestoreUiButton.Visibility = Visibility.Visible;
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

        private void KeysListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var registryWindow = new RegistryWindow(_themeRegistry);
            registryWindow.ShowDialog();
            UpdateRegistry();
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (KeysListBox.SelectedItem == null) return;
            var application = KeysListBox.SelectedItem.ToString();
            foreach (var keyString in Settings.Default.Keys)
            {
                var key = new KeySerializerHelper(keyString);
                if (key.Application != application) continue;
                Settings.Default.Keys.Remove(keyString);
                Settings.Default.Save();
                break;
            }
            UpdateRegistry();
            RemoveButton.Visibility = Visibility.Collapsed;
        }

        private void RestoreAppsButton_OnClick(object sender, RoutedEventArgs e)
        {
            KeySerializerHelper.AddApps();
            UpdateRegistry();
            RestoreAppsButton.Visibility = Visibility.Collapsed;
        }

        private void RestoreUIButton_OnClick(object sender, RoutedEventArgs e)
        {
            KeySerializerHelper.AddUI();
            UpdateRegistry();
            RestoreUiButton.Visibility = Visibility.Collapsed;
        }
    }
}
    