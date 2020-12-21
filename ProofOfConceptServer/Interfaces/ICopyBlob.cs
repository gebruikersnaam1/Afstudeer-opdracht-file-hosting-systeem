using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.interfaces
{
    public class ICopyBlob
    {
        public int blobId { get; set; }
        public int folderId { get; set; }
    }
}
