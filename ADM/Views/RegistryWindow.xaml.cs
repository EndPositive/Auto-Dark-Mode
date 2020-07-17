using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ADM.Helpers;
using ADM.Properties;
using Microsoft.Win32;

namespace ADM.Views
{
    public partial class RegistryWindow
    {
        private RegistryKey _key;
        private readonly ObservableCollection<string> _themeRegistry;
        public RegistryWindow(ObservableCollection<string> themeRegistry)
        {
            _themeRegistry = themeRegistry;
            InitializeComponent();
        }

        private void Validate(object sender, TextChangedEventArgs e)
        {
            ValidateApplication();
            ValidateKey();
            ValidateName();
            ValidateValues();
        }

        private bool ValidateApplication()
        {
            ApplicationTextBox.BorderBrush = !_themeRegistry.Contains(ApplicationTextBox.Text) ? Brushes.GreenYellow : Brushes.Red;
            return !_themeRegistry.Contains(ApplicationTextBox.Text);
        }

        private bool ValidateKey()
        {
            _key = Registry.CurrentUser.OpenSubKey(KeySerializerHelper.ParseKey(KeyTextBox.Text));
            KeyTextBox.BorderBrush = _key != null ? Brushes.GreenYellow : Brushes.Red;
            return _key != null;
        }
        
        private bool ValidateName()
        {
            NameTextBox.BorderBrush = _key?.GetValue(NameTextBox.Text) != null ? Brushes.GreenYellow : Brushes.Red;
            return _key?.GetValue(NameTextBox.Text) != null;
        }

        private bool ValidateValues()
        {
            var valid = true;
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                int.Parse(DarkValueTextBox.Text);
                DarkValueTextBox.BorderBrush = Brushes.GreenYellow;
            }
            catch (System.FormatException)
            {
                DarkValueTextBox.BorderBrush = Brushes.Red;
                valid = false;
            }
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                int.Parse(DarkValueTextBox.Text);
                LightValueTextBox.BorderBrush = Brushes.GreenYellow;
            }
            catch (System.FormatException)
            {
                LightValueTextBox.BorderBrush = Brushes.Red;
                valid = false;
            }

            return valid;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateApplication() || !ValidateKey() || !ValidateName() || !ValidateValues()) return;
            var themeRegistry = KeySerializerHelper.ToString(ApplicationTextBox.Text, KeyTextBox.Text, NameTextBox.Text,
                LightValueTextBox.Text, DarkValueTextBox.Text);
            Settings.Default.Keys.Add(themeRegistry);
            Settings.Default.Save();
            Close();
        }
    }
}