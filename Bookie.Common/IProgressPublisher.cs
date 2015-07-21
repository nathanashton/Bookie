namespace Bookie.Common
{
    using System;

    public interface IProgressPublisher
    {
        event EventHandler<ProgressWindowEventArgs> ProgressChanged;
        event EventHandler<EventArgs> ProgressComplete;
        event EventHandler<EventArgs> ProgressStarted;
        void ProgressCancel();
    }
}