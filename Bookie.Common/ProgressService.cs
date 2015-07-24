namespace Bookie.Common
{
    using System;

    public static class ProgressService
    {
        private static IProgressSubscriber subscriber;
        private static IProgressPublisher publisher;

        public static void RegisterSubscriber(IProgressSubscriber progressSubscriber)
        {
            subscriber = progressSubscriber;
        }

        public static void RegisterPublisher(IProgressPublisher progressPublisher)
        {
            publisher = progressPublisher;
            publisher.ProgressChanged += publisher_ProgressChanged;
            publisher.ProgressStarted += publisher_ProgressStarted;
            publisher.ProgressComplete += publisher_ProgressComplete;
        }

        private static void publisher_ProgressComplete(object sender, EventArgs e)
        {
            subscriber._progress_ProgressCompleted(publisher, e);
        }

        private static void publisher_ProgressStarted(object sender, EventArgs e)
        {
            subscriber._progress_ProgressStarted(publisher, e);
        }

        private static void publisher_ProgressChanged(object sender, ProgressWindowEventArgs e)
        {
            subscriber._progress_ProgressChanged(publisher, e);
        }

        public static void Cancel()
        {
            publisher.ProgressCancel();
        }
    }
}