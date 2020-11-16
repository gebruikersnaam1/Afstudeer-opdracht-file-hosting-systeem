using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public class FolderBlobFactory
    {
        public static FolderItems Create(int blobId, int folderId)
        {
            return new FolderItems
            {
                BlobId = blobId,
                FolderId = folderId
            };
        }
    }
}
