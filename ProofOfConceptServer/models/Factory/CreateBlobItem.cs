using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProofOfConceptServer.models.helpers;

namespace ProofOfConceptServer.models.Factory
{
    public class BlobItemFactory
    {

        private static string CreatePathFile(string uploadRoot, string fileName)
        {
            if (!File.Exists(Path.Combine(uploadRoot, fileName)))
                return Path.Combine(uploadRoot, fileName);

            string e = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);

            int id = 0;
            while (id < 1000) {
                id++;
                if (!File.Exists(Path.Combine(uploadRoot, (name+id+e))))
                    break;
            }
            return Path.Combine(uploadRoot, (name + id + e));
        }

        public static BlobItem Create(CreateBlob postInfo,string id, string uploadRoot)
        {
            try
            {
                return new BlobItem
                {
                    fileId = id,
                    fileName = postInfo.file.FileName,
                    date = DateTime.Today.ToString("dd-MM-yyyy"),
                    pathFile = BlobItemFactory.CreatePathFile(uploadRoot, postInfo.file.FileName),
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
