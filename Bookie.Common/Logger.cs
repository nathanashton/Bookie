using System.Reflection;
using log4net;

namespace Bookie.Common
{
    public static class Logger
    {
        public static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}