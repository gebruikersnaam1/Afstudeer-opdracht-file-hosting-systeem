using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.entities.interfaces;

using ProofOfConceptServer.Repositories.entities;

namespace ProofOfConceptServer.entities.Factory
{
    public class BlobItemFactory
    {

      
        public static BlobItem Create(ICreateBlob postInfo,int id, string uploadPath)
        {
            try
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
            catch(ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine("Following error: " + e);
                return null;
            }
            
        }
    }
}
