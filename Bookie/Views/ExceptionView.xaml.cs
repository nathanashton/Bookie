using System;
using System.Windows;

namespace Bookie.Views
{
    using Bookie.ViewModels;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for ExceptionView.xaml
    /// </summary>
    public partial class ExceptionView : MetroWindow
    {
        public ExceptionViewModel ViewModel { get; set; }

        public ExceptionView()
        {
            InitializeComponent();
            ViewModel = new ExceptionViewModel();
            DataContext = ViewModel;
        }

        private void Center()
        {
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize == e.NewSize)
                return;

            var w = SystemParameters.PrimaryScreenWidth;
            var h = SystemParameters.PrimaryScreenHeight;

            this.Left = (w - e.NewSize.Width) / 2;
            this.Top = (h - e.NewSize.Height) / 2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModel.Fatal)
            {
                Environment.Exit(0);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}