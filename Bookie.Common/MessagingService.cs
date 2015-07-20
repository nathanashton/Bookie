using System.Windows;

namespace Bookie.Common
{
    public static class MessagingService
    {
        public static Window View { get; private set; }

        public delegate void MessageDelegate(object sender, BookieMessageEventArgs e);

        private static MessageDelegate Message { get; set; }

        public static void Register(Window window, MessageDelegate handler)
        {
            View = window;
            Message = handler;
        }

        public static void ShowMessage(string message)
        {
            Message(null, new BookieMessageEventArgs { MoreDetails = null, Message = message });
        }

        public static void ShowErrorMessage(string message, bool fatal)
        {
            Message(null, new BookieMessageEventArgs { MoreDetails = null, Message = message, Fatal = fatal });
        }

        public static void ShowErrorMessage(string message, string moredetails, bool fatal)
        {
            Message(null, new BookieMessageEventArgs { MoreDetails = moredetails, Message = message, Fatal = fatal });
        }

        public static void ShowInfoMessage(string message, bool fatal)
        {
            Message(null, new BookieMessageEventArgs { MoreDetails = null, Message = message, Fatal = false });
        }
    }
}