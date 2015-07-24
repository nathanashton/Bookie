namespace Bookie.Views
{
    using Common;

    public partial class SplashView : ISplashScreen
    {
        public SplashView()
        {
            InitializeComponent();
            VersionNumber.Content = Globals.VersionNumber;
        }

        public void AddMessage(string message)
        {
            Dispatcher.Invoke(delegate { this.Message.Content = message; });
        }

        public void LoadComplete()
        {
            Dispatcher.InvokeShutdown();
        }
    }
}