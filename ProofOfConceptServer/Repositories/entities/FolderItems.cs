using System;
using System.Collections.Generic;

namespace ProofOfConceptServer.Repositories.entities
{
    public partial class FolderItems
    {
        public int BlobId { get; set; }
        public int FolderId { get; set; }

        public virtual BlobItem Blob { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
