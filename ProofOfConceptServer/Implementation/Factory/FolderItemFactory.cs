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
        public static IFolderContent Create(BlobItem blob)
        {
            return new IFolderContent
            {
                Name = blob.FileName,
                LastChanged = blob.Date,
                Id = blob.FileId,
                Size = blob.FileSize,
                Keywords = blob.Description.Split(","),
                Type = Path.GetExtension(blob.Path),
                IsFolder = false
            };
        }

        public static IFolderContent Create(Folder folder)
        {
            return new IFolderContent
            {
                Name = folder.FolderName,
                LastChanged = folder.DateChanged,
                Id = folder.FolderId,
                Size = 0,
                Keywords = {},
                Type = "Folder",
                IsFolder = true
            };
        }
    }
}
