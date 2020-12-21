using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public class FolderBlobFactory
    {
        public static FolderItem Create(int blobId, int folderId)
        {
            return new FolderItem
            {
                BlobId = blobId,
                FolderId = folderId
            };
        }
    }
}
