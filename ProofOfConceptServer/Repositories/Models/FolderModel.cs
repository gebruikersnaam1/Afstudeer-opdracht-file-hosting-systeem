using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.Factory;
using ProofOfConceptServer.Repositories.entities.helpers;
using ProofOfConceptServer.Repositories.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.Models
{
    public class FolderModel
    {
        private woefiedatabaseContext _context;
        private BlobItemModel blobModel;

        public FolderModel()
        {
            _context = new woefiedatabaseContext();
            blobModel = new BlobItemModel();
        }

        public Folder GetFolder(int folderID)
        {
            return _context.Folders.Where(f => f.FolderId == folderID).FirstOrDefault();
        }

        public Folder GetParentFolder(int folderID)
        {
            return _context.Folders.Where(f => f.ParentFolder == folderID).FirstOrDefault();
        }

        public bool DoesFolderExist(int folderID)
        {
            Folder f = GetFolder(folderID);
            if (f == null)
                return false;
            return true;
        }

        private int CreateID()
        {
            int id = _context.Folders.Count();
            
            while(true)
            {
                id++;
                if (!DoesFolderExist(id))
                    break;
            }
            return id;
        }

        public Folder CreateFolder(ICreateFolder data)
        {
            Folder f = CreateFolderFactory.Create(data, CreateID());
            if (f == null)
                return null;
            _context.Folders.Add(f);
            _context.SaveChanges();
            return f;
        }

        public List<IGetFolderResponse> GetFolderContent(int folderId)
        {
            List<FolderItems> folder = _context.FolderItems.Where(f => f.FolderId == folderId).ToList();
            List<IGetFolderResponse> i = new List<IGetFolderResponse>();

            foreach(FolderItems b in folder)
            {
                i.Add(FolderItemFactory.Create(blobModel.GetSingleFile(b.BlobId)));
            }
            foreach (FolderItems f in folder.GroupBy(x => x.FolderId).Select(x => x.First()).ToList())
            {
                i.Add(FolderItemFactory.Create(GetParentFolder(f.FolderId)));
            }
            return i;
        }
    }
}
