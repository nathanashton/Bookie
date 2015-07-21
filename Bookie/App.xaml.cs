namespace Bookie
{
    using Common;
    using log4net;
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using log4net.Core;
    using Views;

    public partial class App
    {
        public static ISplashScreen SplashScreen;

        // ReSharper disable once UnusedMember.Local
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ManualResetEvent _resetSplashCreated;
        private Thread _splashThread;

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

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Log.Error("Unhandled exception", e.Exception);
            MessagingService.ShowErrorMessage(Common.Resources.Strings.Resources.UnhandledException,
                e.Exception.ToString(), false);
            e.Handled = true;
        }

        private void ShowSplash()
        {
            var splashScreenWindow = new SplashView();
            SplashScreen = splashScreenWindow;
            splashScreenWindow.Show();
            _resetSplashCreated.Set();
            Dispatcher.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e != null && e.Args.Length > 0)
            {
                var args = e.Args[0];
                if (args == "debug")
                { Globals.DebugMode = true; }
            }
            var repository = LogManager.GetRepository();
            if (repository != null)
            {
                repository.Threshold = Globals.InDebugMode() ? Level.Debug : Level.Info;
            }

            _resetSplashCreated = new ManualResetEvent(false);
            _splashThread = new Thread(ShowSplash);
            _splashThread.SetApartmentState(ApartmentState.STA);
            _splashThread.IsBackground = true;
            _splashThread.Name = "Splash Screen";
            _splashThread.Start();
            _resetSplashCreated.WaitOne();
            base.OnStartup(e);
        }
    }
}