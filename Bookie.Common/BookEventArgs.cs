namespace Bookie.Common
{
    using System;
    using Model;

    public class BookEventArgs : EventArgs
    {
        public enum BookState
        {
            Added,
            Removed,
            Updated,
            NoChange
        }

        public Book Book { get; set; }
        public BookState State { get; set; }
        public int? Progress { get; set; }
    }
}