using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cvsRead.CvsContainer;

namespace cvsRead
{
    class SlidingAverage
    {
        public string Name { get; set; }
        public List<double> MSums { get; set; }
        public List<double> MAvgs { get; set; }
        //public List<double> MOATs { get; set; }
        public List<DTVal> MDTVals { get; set; }

        public SlidingAverage(CvsContainer container, int dayCount)
        {
            Name = container.Name;
            var sums = new List<double>();

            var dtVals = container.mDTVals;
            var sldCount = dtVals.Count - dayCount;
            var runningSum = dtVals.Take(dayCount).Select(dtv=>dtv.Val).Sum();
            sums.Add(runningSum);

            for (int i = 1; i < sldCount; i++)
            {
                var sumFstVal = dtVals.ElementAt(i).Val;
                var sumLstVal = dtVals.ElementAt(i+dayCount).Val;
                runningSum = runningSum - sumFstVal + sumLstVal;
                sums.Add(runningSum);
            }

            MSums = sums;
            MAvgs = sums.Select(s => s / dayCount).ToList();
            //MOATs = container.mDTVals.Skip(dayCount).Select(dtv => dtv.OADate).ToList();

            MDTVals = Enumerable.Range(dayCount, container.mDTVals.Count- dayCount -1)
                .Select(i => new DTVal() {
                    Val    = MAvgs.ElementAt(i-dayCount),
                    OADate = container.mDTVals.ElementAt(i).OADate
                }).ToList();
        }
    }
}
