using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class ShareItemFactory
    {

        public static ShareItem Create(int id, int blobId)
        {
            return new ShareItem
            {
                Id = id,
                BlobId = blobId,
                ActiveUntil = DateTime.Now.AddDays(+2)
        };
        }
    }
}
