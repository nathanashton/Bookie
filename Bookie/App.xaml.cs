namespace Bookie
{
    using Bookie.Common;
    using Bookie.Views;
    using log4net;
    using System;
    using System.Threading;
    using System.Windows;

    public partial class App
    {
        public static ISplashScreen SplashScreen;
        private ManualResetEvent _resetSplashCreated;
        private Thread _splashThread;

        // ReSharper disable once UnusedMember.Local
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            //    System.Threading.Thread.CurrentThread.CurrentUICulture =
            //                new System.Globalization.CultureInfo("de-DE");

            Globals.Language = Globals.Languages.English;
            AppDomain.CurrentDomain.SetData("DataDirectory", Globals.ApplicationPath);
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Current.Exit += Current_Exit;

            Logger.Log.Info("Application started");
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Logger.Log.Info("Application terminated");
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Log.Error("Unhandled exception", e.Exception);
            MessagingService.ShowErrorMessage(Common.Resources.Strings.Resources.UnhandledException, e.Exception.ToString(), false);
            e.Handled = true;
        }

        private void ShowSplash()
        {
            var splashScreenWindow = new SplashView();
            SplashScreen = splashScreenWindow;
            splashScreenWindow.Show();
            _resetSplashCreated.Set();
            System.Windows.Threading.Dispatcher.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _resetSplashCreated = new ManualResetEvent(false);
            _splashThread = new Thread(ShowSplash); _splashThread.SetApartmentState(ApartmentState.STA);
            _splashThread.IsBackground = true;
            _splashThread.Name = "Splash Screen";
            _splashThread.Start();
            _resetSplashCreated.WaitOne();
            base.OnStartup(e);
        }
    }
}