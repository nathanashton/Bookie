namespace Bookie.Views
{
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LogWindow w = new LogWindow();
            w.ShowDialog();
        }
    }
}