﻿using ProofOfConceptServer.Repositories.entities.helpers;
using System;
using System.Collections.Generic;
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
                IsFolder = true
            };
        }
    }
}