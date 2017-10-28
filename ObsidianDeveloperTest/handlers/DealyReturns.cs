using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ObsidianDeveloperTest.handlers
{
    public class DealyReturns
    {
        [DataMember]
        public IList<Performance> Performances { get; set; }
    }
}