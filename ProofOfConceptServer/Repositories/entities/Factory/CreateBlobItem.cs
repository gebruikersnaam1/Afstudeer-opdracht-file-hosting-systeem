using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.entities.helpers;

namespace ProofOfConceptServer.entities.Factory
{
    public class BlobItemFactory
    {

        private static async Task<string> CreatePathFile(string uploadRoot, string fileName)
        {
            CloudBlobContainer c =  AzureConnection.Container;
            bool fileExist = await c.GetBlockBlobReference(fileName).ExistsAsync();
            
            if(!fileExist)
                return Path.Combine(uploadRoot, fileName).ToString();

            string e = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);

            int id = 0;
            while(fileExist) {
                id++;
                fileExist = await c.GetBlockBlobReference((name + id + e)).ExistsAsync();
                if(!fileExist)
                    break;
            }
            return Path.Combine(uploadRoot, (name + id + e));
        }

        public static BlobEntity Create(CreateBlob postInfo,string id, string uploadRoot)
        {
            try
            {
                return new BlobEntity
                {
                    fileId = id,
                    fileName = postInfo.file.FileName,
                    date = DateTime.Today.ToString("dd-MM-yyyy"),
                    pathFile = BlobItemFactory.CreatePathFile(uploadRoot, postInfo.file.FileName).Result,
                    fileSize = (postInfo.file.Length.ToString()),
                    userId = postInfo.userId,
                    description = postInfo.description
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
