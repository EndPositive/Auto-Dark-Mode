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
        private readonly ObservableCollection<string> _observable;

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

            _observable = new ObservableCollection<string>();
            UpdateObservable();
            KeysListBox.ItemsSource = _observable;

            // TODO: Detect Windows theme switch while window is open
            StartupToggleSwitch.IsOn = Settings.Default.Startup;
        }

        private void UpdateObservable()
        {
            bool appsButtonVisible = true, uiButtonVisible = true;
            _observable.Clear();
            foreach (var applicationName in RegistryHelper.GetObservable())
            {
                _observable.Add(applicationName);
                if (applicationName == "System UI") uiButtonVisible = false;
                else if (applicationName == "System Apps") appsButtonVisible = false;
            }

            RestoreAppsButton.Visibility = appsButtonVisible ? Visibility.Visible : Visibility.Collapsed;
            RestoreUiButton.Visibility = uiButtonVisible ? Visibility.Visible : Visibility.Collapsed;
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

            if (StartTimePicker.SelectedDateTime == null || EndTimePicker.SelectedDateTime == null)
                new ExceptionWindow("Could not save due to no proper time selected.");
                
            Settings.Default.StartTime = StartTimePicker.SelectedDateTime ?? throw new Exception();
            Settings.Default.EndTime = EndTimePicker.SelectedDateTime ?? throw new Exception();
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
            var registryWindow = new RegistryWindow();
            registryWindow.ShowDialog();
            UpdateObservable();
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (KeysListBox.SelectedItem == null) return;
            RegistryHelper.Remove(KeysListBox.SelectedItem.ToString());
            UpdateObservable();
            RemoveButton.Visibility = Visibility.Collapsed;
        }

        private void RestoreAppsButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegistryHelper.RestoreApps();
            UpdateObservable();
            RestoreAppsButton.Visibility = Visibility.Collapsed;
        }

        private void RestoreUIButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegistryHelper.RestoreUi();
            UpdateObservable();
            RestoreUiButton.Visibility = Visibility.Collapsed;
        }
    }
}
    