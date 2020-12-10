using System;
using System.Collections.Generic;

namespace ProofOfConceptServer.Repositories.entities
{
    public partial class ShareItems
    {
        public int Id { get; set; }
        public int BlobId { get; set; }
        public DateTime ActiveUntil { get; set; }

        public virtual BlobItem Blob { get; set; }
    }
}
