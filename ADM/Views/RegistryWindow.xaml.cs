using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ADM.Helpers;

namespace ADM.Views
{
    public partial class RegistryWindow
    {
        public RegistryWindow()
        {
            InitializeComponent();
        }

        private void Validate(object sender, TextChangedEventArgs e)
        {
            ValidationColor(ApplicationTextBox, RegistryHelper.IsValidApplication(ApplicationTextBox.Text));
            ValidationColor(KeyTextBox, RegistryHelper.IsValidKey(KeyTextBox.Text));
            ValidationColor(NameTextBox, RegistryHelper.IsValidName(NameTextBox.Text, KeyTextBox.Text));
            ValidationColor(DarkValueTextBox, RegistryHelper.IsValidValue(DarkValueTextBox.Text));
            ValidationColor(LightValueTextBox, RegistryHelper.IsValidValue(LightValueTextBox.Text));
        }

        private static void ValidationColor(Control textBox, bool valid)
        {
            textBox.BorderBrush = valid ? Brushes.GreenYellow : Brushes.Red;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!RegistryHelper.IsValidApplication(ApplicationTextBox.Text) ||
                !RegistryHelper.IsValidKey(KeyTextBox.Text) ||
                !RegistryHelper.IsValidName(NameTextBox.Text, KeyTextBox.Text) ||
                !RegistryHelper.IsValidValue(DarkValueTextBox.Text) ||
                !RegistryHelper.IsValidValue(LightValueTextBox.Text)) return;
            RegistryHelper.Add(ApplicationTextBox.Text, KeyTextBox.Text, NameTextBox.Text,
                LightValueTextBox.Text, DarkValueTextBox.Text);
            Close();
        }
    }
}