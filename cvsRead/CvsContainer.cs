﻿using System;
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

        public CvsContainer(string path)
        {
            var ls = System.IO.File.ReadAllLines(path).ToList();
            mDTVals = ls.Select(l => ParseLine(l)).ToList();
        }

        public CvsContainer(List<string> lines)
        {
            mDTVals = lines.Select(l => ParseLine(l)).ToList();
        }

        public class DTVal
        {
            public DateTime DT { get; set; }
            public double OADate { get; set; }
            public double Val { get; set; }

        }

        private DTVal ParseLine(string line)
        {
            var spl = line.Split(';');
            var dtStr = spl[0];
            var valStr = spl[1];
            var dt = DateTime.ParseExact(dtStr, "dd.MM.yyyy", new CultureInfo("en-US"));
            var val = double.Parse(valStr.Replace(',', '.'), CultureInfo.InvariantCulture);
            return new DTVal() { DT = dt, OADate = dt.ToOADate(), Val = val };
        }

    }
}
