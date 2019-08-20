using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Infrastructure
{
    public static class MusicLength
    {
        public static string TotalChartsLength(int totalSecs)
        {
            int hours, mins, secs;
            string hh, mm, ss;

            hours = totalSecs / 3600;
            mins = (totalSecs % 3600) / 60;
            secs = (totalSecs % 3600) % 60;

            hh = hours.ToString();
            mm = mins.ToString();
            ss = secs.ToString();

            if (hours < 10)
            {
                hh = "0" + hours.ToString();
            }
            if (mins < 10)
            {
                mm = "0" + mins.ToString();
            }
            if (secs < 10)
            {
                ss = "0" + secs.ToString();
            }

            return $"{hh}:{mm}:{ss}";
        }
    }
}
