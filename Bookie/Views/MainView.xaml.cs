using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Bookie.Common;
using Bookie.Core.Domains;
using Bookie.Properties;
using Bookie.ViewModels;
using MahApps.Metro;

namespace Bookie.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainViewModel ViewModel;

        public MainView()
        {
            ViewModel = new MainViewModel();
            Load();
            InitializeComponent();
            DataContext = ViewModel;

            VersionNumber.Content = Globals.VersionNumber;
        }

        private void Load()
        {
            MessagingService.Register(this, MessagingService_messages);
            App.SplashScreen.AddMessage("Applying Theme...");
            Thread.Sleep(500);
            ApplyTheme();
            App.SplashScreen.AddMessage("Loading Settings...");
            Thread.Sleep(500);
            ApplySettings();
            App.SplashScreen.AddMessage("Loading Books...");

            ViewModel = new MainViewModel();

            var savedTileWidth = Settings.Default.TileWidth;
            ViewModel.TileWidth = savedTileWidth == 0 ? 130 : savedTileWidth;

            ViewModel.TileWidth = 130;

            App.SplashScreen.LoadComplete();
        }

        private void ApplySettings()
        {
            if (!File.Exists(Globals.ApplicationDatabaseFullPath))
            {
                App.SplashScreen.AddMessage("Creating Database...");
                Db d = new Db();
                d.ReCreateDB();
                Logger.Log.Info("Database doesn't exist so created it");
            }

            if (!Directory.Exists(Globals.CoverImageFolder))
            {
                Directory.CreateDirectory(Globals.CoverImageFolder);
            }
        }

        private void ApplyTheme()
        {
            ThemeManager.AddAccent("CustomBlue", new Uri("pack://application:,,,/Themes/ShitBrown.xaml"));
            var accent = ThemeManager.GetAccent("CustomBlue");
            var theme = ThemeManager.GetAppTheme("BaseLight");
            ThemeManager.ChangeAppStyle(Application.Current.Resources, accent, theme);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void MessagingService_messages(object sender, BookieMessageEventArgs e)
        {
            var exceptionView = new ExceptionView
            {
                ViewModel =
                {
                    Message = e.Message,
                    MoreDetails = e.MoreDetails,
                    Fatal = e.Fatal
                }
            };
            exceptionView.ShowDialog();
        }

        private void Filter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            var tBox = (TextBox)sender;
            var prop = TextBox.TextProperty;

            var binding = BindingOperations.GetBindingExpression(tBox, prop);
            if (binding != null)
            {
                binding.UpdateSource();
            }
        }

    }
}