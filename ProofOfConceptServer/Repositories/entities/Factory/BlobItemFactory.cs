using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.entities.interfaces;

using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.interfaces;

namespace ProofOfConceptServer.entities.Factory
{
    public class BlobItemFactory
    {

        
        public static BlobItem Create(ICreateBlob postInfo,int id, string uploadPath)
        {
            return new BlobItem
            {
                FileId = id,
                FileName = postInfo.file.FileName,
                Date = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                Path = uploadPath,
                FileSize = (int)postInfo.file.Length / 1024,
                UserId = postInfo.userId,
                Description = postInfo.description
            };
        }

        public static BlobItem Create(ISynchronicFiles sync, int id)
        {
            return new BlobItem
            {
                FileId = id,
                FileName = sync.FileName,
                Date = sync.CreatedOn,
                Path = sync.FileName,
                FileSize = sync.FileSize,
                UserId = "unknown",
                Description = ""
            };
        }
    }
}
