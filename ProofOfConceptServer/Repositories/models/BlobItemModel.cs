using ProofOfConceptServer.entities.Factory;
using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProofOfConceptServer.Repositories.Models;
using ProofOfConceptServer.Repositories.interfaces;
using System.IO.Compression;

namespace ProofOfConceptServer.Repositories.models
{

    public class BlobItemModel
    {
        private woefiedatabaseContext _context;
        private StorageModel Storage;
        private ShareableModel ShareableModel;
        public BlobItemModel()
        {
            Storage = new StorageModel();
            _context = new woefiedatabaseContext();
            this.ShareableModel = new ShareableModel();
        }


        public int RowsCount()
        {
            return _context.BlobItem.ToList().Count();
        }

        public List<BlobItem> GetPages(int itemsPerPage, int currentPage)
        {
            return _context.BlobItem.Skip((itemsPerPage * currentPage)).Take(itemsPerPage).ToList();
        }

        public BlobItem GetSingleFile(int id)
        {
            return _context.BlobItem.Where(item =>
                    item.FileId == id).FirstOrDefault();
        }

        private int GenerateId()
        {
            int id = _context.BlobItem.ToList().Count();
            int idUnique = 1;
            while (5 > 0)
            {
                id += 1;
                idUnique = _context.BlobItem.ToList().FindIndex(i => i.FileId == id);
                if (idUnique <= 0)
                    break;
            }
            return id;
        }

        public BlobItem CopyFile(BlobItem blobItem)
        {
            BlobItem copy = (BlobItem) blobItem.Clone();
            copy.FileId = this.GenerateId();
            _context.BlobItem.Add(copy); //set file in the "db"
            _context.SaveChanges();
            return copy;
        }

        public BlobItem CreateBlobItem(ICreateBlob postData)
        {

            int id = this.GenerateId();
            BlobItem blobItem = BlobItemFactory.Create(postData, id, Storage.CreatePathFile(postData.file.FileName));

            if (blobItem == null)
                return null;

            string fileName = Path.GetFileName(blobItem.Path);

            if (!Storage.Create(fileName, postData.file).Result)
                return null;

            _context.BlobItem.Add(blobItem); //set file in the "db"
            _context.SaveChanges();
        
            return blobItem;
        }

        public List<BlobItem> SearchFiles(string term)
        {
            return _context.BlobItem.Where(file =>
                    file.FileName.ToLower().Contains(term.ToLower())
                ).ToList();
        }

        public bool UpdateBlob(BlobItem newFile)
        {
            BlobItem oldFile = _context.BlobItem.Where(
                item => item.FileId == newFile.FileId).FirstOrDefault();

            if (oldFile == null)
                return false;

            oldFile.FileName = newFile.FileName;
            oldFile.Description = newFile.Description;
            _context.SaveChanges();
            return true;
        }

        private bool IsPathFileShared(string path)
        {
            int rows = _context.BlobItem.Where(i => i.Path == path).Count();
            if (rows > 1)
                return true;
            return false;
        }

        public bool DeleteBlobItem(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                   item.FileId == (id)).FirstOrDefault();

            if (blobItem == null)
                return false;

            this.ShareableModel.DeleteShareItem(blobItem);
            try
            {
                string fileOnCloud = Path.GetFileName(blobItem.Path);
               
                if(!IsPathFileShared(blobItem.Path))
                    Storage.Delete(fileOnCloud);
                _context.BlobItem.Remove(blobItem);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IFileInformation DownloadFileAssistent(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                    item.FileId == id).FirstOrDefault();
            string extension = Path.GetExtension(blobItem.Path);

            return new IFileInformation
            {
                fileName = Path.GetFileNameWithoutExtension(blobItem.FileName),
                extension = extension
            };
        }


        public  IDownloadFileResponse DownloadFile(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                    item.FileId == id).FirstOrDefault();

            if (blobItem == null)
                return null;

            //download the file to the local server
            string path = Storage.DownloadBlobFileToServer(blobItem).Result;

            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                return new IDownloadFileResponse
                {
                    File = data,
                    FileName = blobItem.FileName
                };
            }
            catch
            {
                return null;
            }
        }

        public byte[] DownloadFilesInZip(int[] ids)
        {
            List<IDownloadFileResponse> files = new List<IDownloadFileResponse>();
            foreach (int i in ids)
            {
                files.Add(DownloadFile(i));
            }
            Byte[] zipBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    foreach(IDownloadFileResponse f in files)
                    {
                        if(f != null) { 
                            var zipEntry = zipArchive.CreateEntry(f.FileName);
                            using (Stream entryStream = zipEntry.Open())
                            {
                                entryStream.Write(f.File, 0, f.File.Length);
                            }
                        }
                    }
                }
                zipBytes = memoryStream.ToArray();
            }
            return zipBytes;
        }

        public List<BlobItem> SynchronizationBlobs()
        {
            List<ISynchronicFiles> cloudFiles = this.Storage.Synchronization().Result;
            List<BlobItem> dbFiles = _context.BlobItem.ToList();
            List<BlobItem> l = new List<BlobItem>();
            BlobItem b;
            foreach (ISynchronicFiles c in cloudFiles)
            {
                if (dbFiles.Where(d => d.Path == c.FileName).FirstOrDefault() == null)
                {
                    b = (BlobItemFactory.Create(c, GenerateId()));
                    l.Add(b);
                    _context.BlobItem.Add(b);
                    _context.SaveChanges();
                }
            }
            return l;
        }
    }
}