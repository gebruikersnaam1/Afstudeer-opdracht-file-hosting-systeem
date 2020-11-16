using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.entities;
using ProofOfConceptServer.entities.dummy_data;
using ProofOfConceptServer.entities.Factory;
using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.models;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.models
{
    public class BlobItemModel
    {
        private string blobItemsPath = Path.Combine(Startup.apiRoot, "Models/uploads");
        private woefiedatabaseContext _context;

        public BlobItemModel()
        {
            _context = new woefiedatabaseContext();
            //LoadDummyData();
        }

        public void LoadDummyData()
        {
            try
            {
                foreach (BlobItem bItem in DummyDataBlobfiles.GetDummyData())
                {
                    _context.BlobItem.Add(bItem);
                    _context.SaveChanges();
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Dummy data error, but data may be loaded");
            }
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

        public async Task<BlobItem> CreateBlobItem(ICreateBlob postData)
        {

            int id = this.GenerateId();
            BlobItem blobItem = BlobItemFactory.Create(postData, id, blobItemsPath);

            if (blobItem == null)
                return null;

            string fileName = Path.GetFileName(blobItem.Path);

            CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
            using (Stream fileStream = postData.file.OpenReadStream())
            {
                await blockBob.UploadFromStreamAsync(fileStream);
                _context.BlobItem.Add(blobItem); //set file in the "db"
                _context.SaveChanges();
            }

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

        public async Task<bool> Delete(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                   item.FileId == (id)).FirstOrDefault();

            if (blobItem == null)
                return false;

            try
            {
                string fileOnCloud = Path.GetFileName(blobItem.Path);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileOnCloud);
                await blockBob.DeleteIfExistsAsync();
                _context.BlobItem.Remove(blobItem);
                _context.SaveChanges();
                return true;
            }
            catch (ArgumentException e)
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

        private async Task<bool> DownloadBlobFileToServer(BlobItem blobItem)
        {
            try
            {
                string fileName = Path.GetFileName(blobItem.Path);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                var rootDir = new FileInfo(blobItem.Path).Directory;
                if (!rootDir.Exists) //make sure the parent directory exists
                    rootDir.Create();

                await blockBob.DownloadToFileAsync(blobItem.Path, FileMode.Create);
                return true;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("File couldn't be downloaded from the Azure!");
                return false;
            }
        }


        public async Task<IDownloadFileResponse> DownloadFile(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                    item.FileId == id).FirstOrDefault();

            if (blobItem == null)
                return null;

            //download the file to the local server
            await DownloadBlobFileToServer(blobItem);

            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(blobItem.Path);
                var content = new System.IO.MemoryStream(data);

                if (System.IO.File.Exists(blobItem.Path))
                    System.IO.File.Delete(blobItem.Path);

                // var z = File(data, "application/octet-stream", blobItem.fileName);
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
    }
}