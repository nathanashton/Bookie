namespace Bookie.Common
{
    using System;

    public class BookieMessageEventArgs : EventArgs
    {
        public string MoreDetails { get; set; }
        public string Message { get; set; }
        public bool Fatal { get; set; }
    }
}