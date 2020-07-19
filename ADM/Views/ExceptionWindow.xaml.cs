using System.Windows;

namespace ADM.Views
{
    public partial class ExceptionWindow
    {
        public ExceptionWindow(string problem)
        {
            InitializeComponent();
            ErrorTextBox.Text = problem;
            ShowDialog();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}