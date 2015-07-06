namespace Bookie.Views
{
    using Bookie.Common;
    using Bookie.Core.Domains;
    using Bookie.ViewModels;
    using MahApps.Metro;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainViewModel ViewModel;

        public MainView()
        {
            Load();
            InitializeComponent();
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
            DataContext = ViewModel;
            var savedTileWidth = AppConfig.LoadSetting("TileWidth");
            ViewModel.TileWidth = String.IsNullOrEmpty(savedTileWidth) ? 130 : Int32.Parse(savedTileWidth);

            App.SplashScreen.LoadComplete();
        }

        private void ApplySettings()
        {
            AppConfig.AddSetting("CoverImageFolder", Globals.ApplicationPath + @"\Covers\");
            Globals.CoverImageFolder = AppConfig.LoadSetting("CoverImageFolder");

            if (!File.Exists(Globals.ApplicationDatabaseFullPath))
            {
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

        private void PublisherTree_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var title = sender as TextBlock;
            if (title == null)
            {
                return;
            }
            var book = ViewModel.AllBooks.FirstOrDefault(x => x.Title.Equals(title.Text));
            if (book != null)
            {
                ViewModel.SelectedBook = book;
            }
        }

        private void AuthorTree_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var title = sender as TextBlock;
            if (title == null)
            {
                return;
            }
            var book = ViewModel.AllBooks.FirstOrDefault(x => x.Title.Equals(title.Text));
            if (book != null)
            {
                ViewModel.SelectedBook = book;
            }
        }
    }
}