using System.Windows;
using MahApps.Metro.Controls;

namespace Bookie.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogWindow w = new LogWindow(new ViewModels.LogViewModel());
            w.ShowDialog();
        }
    }
}