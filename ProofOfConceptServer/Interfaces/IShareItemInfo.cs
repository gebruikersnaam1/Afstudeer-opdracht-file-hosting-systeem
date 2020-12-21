using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.interfaces
{
    public class IShareItemInfo
    {
        public int ShareId { get; set; }
        public string FileName { get; set; }
        public DateTime AvailableUntil { get; set; }
    }
}
