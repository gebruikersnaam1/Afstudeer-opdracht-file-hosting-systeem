using ProofOfConceptServer.Repositories.entities.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class FolderItemFactory
    {
        public static IFolderResponse Create(BlobItem blob)
        {
            return new IFolderResponse
            {
                Name = blob.FileName,
                LastChanged = blob.Date,
                Id = blob.FileId,
                Size = blob.FileSize,
                Type = Path.GetExtension(blob.Path),
                IsFolder = false
            };
        }

        public static IFolderResponse Create(Folder folder)
        {
            return new IFolderResponse
            {
                Name = folder.FolderName,
                LastChanged = folder.DateChanged,
                Id = folder.FolderId,
                Size = 0,
                Type = "Folder",
                IsFolder = true
            };
        }
    }
}
