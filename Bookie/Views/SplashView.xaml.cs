namespace Bookie.Views
{
    using System;

    using Bookie.Common;

    public partial class SplashView : ISplashScreen
    {
        public SplashView()
        {
            InitializeComponent();
         }

        public void AddMessage(string message)
        {
            Dispatcher.Invoke((Action)delegate()
            {
                this.Message.Content = message;
            });
        }

        public void LoadComplete()
        {
            Dispatcher.InvokeShutdown();
        }
    }
}