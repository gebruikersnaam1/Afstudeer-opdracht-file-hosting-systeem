using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.Factory;
using ProofOfConceptServer.Repositories.entities.interfaces;
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

        public List<IFolderResponse> GetFolderContent(int folderId)
        {
            List<FolderItems> folder = _context.FolderItems.Where(f => f.FolderId == folderId).ToList();
            List<IFolderResponse> i = new List<IFolderResponse>();

            foreach(FolderItems b in folder)
            {
                i.Add(FolderItemFactory.Create(blobModel.GetSingleFile(b.BlobId)));
            }
            foreach (Folder f in _context.Folders.Where(f => f.ParentFolder == folderId).ToList())
            {
                i.Add(FolderItemFactory.Create(f));
            }
            return i;
        }

        public BlobItem CreateFolderBlobItem(ICreateBlob postData, int folderId)
        {
            BlobItem b = this.blobModel.CreateBlobItem(postData).Result;

            if (b == null)
                return null;

            FolderItems f = FolderBlobFactory.Create(b.FileId, folderId);
            _context.FolderItems.Add(f);
            _context.SaveChanges();
            return b;
        }

        public List<IFolderResponse> SearchForFile(string searchTerm)
        {
            List<BlobItem> blobList = this.blobModel.SearchFiles(searchTerm);
            List<IFolderResponse> f = new List<IFolderResponse>();
            foreach(BlobItem b in blobList)
            {
                f.Add(FolderItemFactory.Create(b));
            }
            return f;
        }
    }
}
