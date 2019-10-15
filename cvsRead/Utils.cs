using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvsRead
{
    class Utils
    {
        // DateTime.ParseExact slow,
        // try splitting dd.MM.yyyy
        public static DateTime SplitParseDT(string str) {
            var spl = str.Split('.');
            var days = int.Parse(spl[0]);
            var months = int.Parse(spl[1]);
            var years = int.Parse(spl[2]);
            return new DateTime(years, months, days);
        }

    }
}
