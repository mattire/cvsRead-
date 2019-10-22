using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvsRead
{
    class CvsContainer
    {
        public List<DTVal> mDTVals;
        public string Name { get; set; }

        public CvsContainer(string path)
        {
            var ls = System.IO.File.ReadAllLines(path).ToList();
            #region SpeedTests
            //var times = ls.Select(l => l.Split(';')[0]).Select(s=>
            //                         Utils.SplitParseDT(s).ToOADate())
            //                         //DateTime.ParseExact(
            //                         //    s, 
            //                         //   "dd.MM.yyyy", 
            //                         //   new CultureInfo("en-US")))
            //                         //   .Select(s=>s.ToOADate())
            //                            .ToList();
            //var vals =  ls.Select(l => l.Split(';')[1]
            //                        .Replace(',', '.')).Select(v=>
            //                        double.Parse(v,
            //                        CultureInfo.InvariantCulture))
            //                        .ToList();
            //double.Parse(valStr.Replace(',', '.'), CultureInfo.InvariantCulture);

            #endregion
            mDTVals = ls.Select(l => ParseLine(l)).ToList();

            Name = System.IO.Path.GetFileName(path);
            Managers.DataMngr.Instance.CvsContainers.Add(this);
        }

        public CvsContainer(List<string> lines)
        {

            mDTVals = lines.Select(l => ParseLine(l)).ToList();
        }

        public struct DTVal
        {
            //public DateTime DT { get; set; }
            public double OADate { get; set; }
            public double Val { get; set; }

        }

        private DTVal ParseLine(string line)
        {
            var spl = line.Split(';');
            var dtStr = spl[0];
            var valStr = spl[1];
            //var dt = DateTime.ParseExact(dtStr, "dd.MM.yyyy", new CultureInfo("en-US"));
            var dt = Utils.SplitParseDT(dtStr);
            var val = double.Parse(valStr.Replace(',', '.'), CultureInfo.InvariantCulture);
            return new DTVal() { /*DT = dt,*/ OADate = dt.ToOADate(), Val = val };
        }

    }
}
