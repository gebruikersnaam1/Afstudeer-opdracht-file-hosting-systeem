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
        public static IGetFolderResponse Create(BlobItem blob)
        {
            return new IGetFolderResponse
            {
                Name = blob.FileName,
                LastChanged = blob.Date,
                Id = blob.FileId,
                Size = blob.FileSize,
                Type = Path.GetExtension(blob.Path),
                IsFolder = false
            };
        }

        public static IGetFolderResponse Create(Folder folder)
        {
            return new IGetFolderResponse
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
