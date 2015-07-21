namespace Bookie.Common
{
    using System.Reflection;
    using log4net;

    public static class Logger
    {
        public static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

       
    }

  
}