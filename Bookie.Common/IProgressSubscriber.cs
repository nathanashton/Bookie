namespace Bookie.Common
{
    using System;

    public interface IProgressSubscriber
    {
        void _progress_ProgressChanged(object sender, ProgressWindowEventArgs e);

        void _progress_ProgressStarted(object sender, EventArgs e);

        void _progress_ProgressCompleted(object sender, EventArgs e);
    }
}