using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvsRead.Managers
{
    class DataMngr
    {
        private static DataMngr instance = null;
        private static readonly object padlock = new object();

        public List<CvsContainer> CvsContainers { get; set; }

        DataMngr()
        {
            CvsContainers = new List<CvsContainer>();
        }

        public static DataMngr Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DataMngr();
                    }
                    return instance;
                }
            }
        }

        internal CvsContainer GetCvs(string seriesName)
        {
            return CvsContainers.FirstOrDefault(c => c.Name == seriesName);
        }

        internal SlidingAverage GetSliding(string name, int value)
        {
            var cvs = GetCvs(name);
            var sa = new SlidingAverage(cvs, value);
            return sa;
        }
    }
}
