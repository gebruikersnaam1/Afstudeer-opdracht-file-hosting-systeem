using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.helpers;
using ProofOfConceptServer.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Services.handlers
{
    public class FolderHandler
    {
        private FolderModel Model;
        public FolderHandler()
        {
            this.Model = new FolderModel();
        }
        public Folder CreateFolder(ICreateFolder data)
        {
            if (this.Model.DoesFolderExist(data.parentID))
            {
                System.Diagnostics.Debug.WriteLine("Parent class doesn't exist");
                return null;
            }
            return this.Model.CreateFolder(data);
        }

        public bool DoesFolderExist(int folderID)
        {
           return this.Model.DoesFolderExist(folderID);
        }

        public List<IGetFolderResponse> GetFolderContent(int folderID)
        {
            return this.Model.GetFolderContent(folderID);
        }
    }
}
