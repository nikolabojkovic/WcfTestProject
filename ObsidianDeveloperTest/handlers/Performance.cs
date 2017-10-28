using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObsidianDeveloperTest.handlers
{
    public class Performance
    {
        [DataMember]
        public DateTime PerformanceDate { get; set; }

        [DataMember]
        public string MonthlyReturn { get; set; }

        [DataMember]
        public decimal TotalReturn { get; set; }
    }
}