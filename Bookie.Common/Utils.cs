using System;

namespace Bookie.Common
{
    public static class Utils
    {
        public static int CalculatePercentage(int current, int startIndex, int endIndex)
        {
            float range = endIndex - startIndex;
            if (range == 0)
            {
                range = 1;
            }
            float percentage = ((current - startIndex) / range) * 100;
            return Convert.ToInt32(percentage);
        }

        public static int CalculatePercentage(Int64 current, Int64 startIndex, Int64 endIndex)
        {
            var range = endIndex - startIndex;
            var percentage = ((current - startIndex) / range) * 100;
            return Convert.ToInt32(percentage);
        }
    }
}