namespace Bookie.Views
{
    using System.Windows;
    using Core;
    using MahApps.Metro.Controls;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        public SettingsViewModel ViewModel => (SettingsViewModel) Resources["ViewModel"];

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var w = new LogWindow(new LogViewModel());
            w.ShowDialog();
        }

        private void CleanImages()
        {
            var l = new Library();
            l.CleanImages();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CleanImages();
        }
    }
}