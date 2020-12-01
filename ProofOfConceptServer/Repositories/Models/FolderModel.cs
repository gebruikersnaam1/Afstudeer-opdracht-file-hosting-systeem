using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.Factory;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.interfaces;
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
        private int rootFolder = 1;

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

        public List<IFolderContent> GetFolderContent(int folderId)
        {
            List<FolderItems> folder = _context.FolderItems.Where(f => f.FolderId == folderId).ToList();
            List<IFolderContent> i = new List<IFolderContent>();

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
            BlobItem b = this.blobModel.CreateBlobItem(postData);

            if (b == null)
                return null;

            FolderItems f = FolderBlobFactory.Create(b.FileId, folderId);
            _context.FolderItems.Add(f);
            _context.SaveChanges();
            return b;
        }

        public List<IFolderContent> SearchForFile(string searchTerm)
        {
            List<BlobItem> blobList = this.blobModel.SearchFiles(searchTerm);
            List<IFolderContent> f = new List<IFolderContent>();
            foreach(BlobItem b in blobList)
            {
                f.Add(FolderItemFactory.Create(b));
            }
            return f;
        }

        public IFolderWithParent GetFolderWithParent(int folderId)
        {
            Folder f = GetFolder(folderId);
            if (f == null)
                return null;

            return FolderWithParentFactory.Create(f, GetFolderWithParent(f.ParentFolder));
        }

        public Folder ChangeFolderName(IChangeFolder changeFolder)
        {
            Folder f = GetFolder(changeFolder.folderId);
            if (f == null)
                return null;
            f.FolderName = changeFolder.folderName;
            f.DateChanged = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy"));
            _context.Update(f);
            _context.SaveChanges();
            return f;
        }

        public IFolderStructure GetFolderStructure()
        {
            return GetFolderTree(GetFolder(1));
        }

        private IFolderStructure GetFolderTree(Folder folder)
        {
            IFolderStructure tree = BranchFactory.Create(folder);

            List<Folder> childFolders = _context.Folders.Where(z => z.ParentFolder == folder.FolderId).ToList();

            if (childFolders == null || childFolders.Count == 0)
                return tree;

            IFolderStructure branch;
            foreach (Folder f in childFolders)
            {
                branch = BranchFactory.Create(f);
                branch.ChildBranches = GetFolderTree(f).ChildBranches;
                tree.ChildBranches.Add(branch);
            }

            return tree;
        }
        
        private List<Folder> GetAllChildFolders(Folder folder)
        {
            List<Folder> tree = new List<Folder>();
            tree.Add(folder);

            List <Folder> childFolders = _context.Folders.Where(z => z.ParentFolder == folder.FolderId).ToList();

            if (childFolders == null || childFolders.Count == 0)
                return tree;

            foreach(Folder f in childFolders)
            {
                tree.AddRange(GetAllChildFolders(f));
            }

            return tree;
        }

        public int CountDuplicates(int fileId)
        {
            return _context.FolderItems.Where(f => f.BlobId == fileId).ToList().Count;
        }

        private void RemoveBlobsWithoutFolder()
        {
            List<BlobItem> blobs = _context.BlobItem.ToList();
            FolderItems item;
            foreach(BlobItem b in blobs)
            {
                item = null;
                item = _context.FolderItems.Where(i => i.BlobId == b.FileId).FirstOrDefault();
                if (item == null)
                     blobModel.DeleteBlobItem(b.FileId);
            }
        }

        public bool RemoveBlobFromFolders(int blobId)
        {
            try
            {
                List<FolderItems> fi = _context.FolderItems.Where(fi => fi.BlobId == blobId).ToList();
                foreach (FolderItems i in fi)
                {
                    _context.Remove(i);
                }
                _context.SaveChanges();
                blobModel.DeleteBlobItem(blobId);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }

        public bool DeleteFolder(int folderId)
        {
            try
            {
                List <Folder> folders = GetAllChildFolders(GetFolder(folderId));
                List<FolderItems> items = new List<FolderItems>();

                foreach (Folder f in folders)
                {
                    items.AddRange(_context.FolderItems.Where(i => i.FolderId == f.FolderId).ToList());
                    foreach (FolderItems fi in items)
                    {
                        _context.RemoveRange(fi);
                    }
                    _context.Remove(f);
                    _context.SaveChanges();
                    RemoveBlobsWithoutFolder();
            };
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SynchronizationFiles()
        {
           List<BlobItem> newBlobs = this.blobModel.SynchronizationBlobs();
           FolderItems f;
           foreach (BlobItem b in newBlobs)
           {
                 f = FolderBlobFactory.Create(b.FileId, rootFolder);
                _context.FolderItems.Add(f);
                _context.SaveChanges();
            }
        }
    }
}
