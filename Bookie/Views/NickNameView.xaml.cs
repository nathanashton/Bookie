namespace Bookie.Views
{
    using System.Windows;
    using MahApps.Metro.Controls;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for NickNameView.xaml
    /// </summary>
    public partial class NickNameView : MetroWindow
    {
        public NickNameView()
        {
            InitializeComponent();
        }

        public NickNameViewModel ViewModel => (NickNameViewModel) Resources["ViewModel"];

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}