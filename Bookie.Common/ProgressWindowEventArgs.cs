namespace Bookie.Common
{
    using System;

    public class ProgressWindowEventArgs : EventArgs
    {
        public int ProgressPercentage { get; set; }
        public string OperationName { get; set; }
        public bool Cancel { get; set; }
        public string ProgressBarText { get; set; }
        public string ProgressText { get; set; }
    }
}