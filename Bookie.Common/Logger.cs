namespace Bookie.Common
{
    using log4net;

    public static class Logger
    {
        public static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}