using ProofOfConceptServer.Repositories.entities.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class CreateFolderFactory
    {
        public static Folder Create(ICreateFolder data, int uniqueID)
        {
            try
            {
                return new Folder
                {
                    FolderId = uniqueID,
                    FolderName = data.folderName,
                    ParentFolder = data.parentID,
                    DateChanged = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy")),
                    CreatedDate = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy"))
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
