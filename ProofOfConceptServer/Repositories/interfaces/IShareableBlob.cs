using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.interfaces
{
    public class IShareableBlob
    {
        public int ShareId { get; set; }
        public DateTime ActiveUntil { get; set; }
        public int BlobId { get; set; }
    }
}
