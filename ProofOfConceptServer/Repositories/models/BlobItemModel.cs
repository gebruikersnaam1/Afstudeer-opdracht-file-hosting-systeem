using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.entities;
using ProofOfConceptServer.entities.dummy_data;
using ProofOfConceptServer.entities.Factory;
using ProofOfConceptServer.entities.helpers;
using ProofOfConceptServer.Models;
using ProofOfConceptServer.Repositories.entities.helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.models
{
    public class BlobItemModel
    {
        ///private List<BlobItem> FilesStorage = DummyDataBlobfiles.GetDummyData();
        private string blobItemsPath = Path.Combine(Startup.apiRoot, "Models/uploads");
        private woefiedatabaseContext _context;

        public BlobItemModel()
        {
            _context = new woefiedatabaseContext();
        }
        public int RowsCount()
        {
            return _context.BlobItem.ToList().Count();
        }

        public List<BlobItem> GetPages(int itemsPerPage, int currentPage)
        {
            return _context.BlobItem.ToList().Skip((itemsPerPage * currentPage)).Take(itemsPerPage).ToList();
        }

        public BlobItem GetSingleFile(int id)
        {
            return _context.BlobItem.ToList().Find(item =>
                    item.FileId.Equals(id));
        }

        private int GenerateId()
        { 
            int id = _context.BlobItem.ToList().Count();
            int idUnique = 1;
            while(5 > 0)
            {
                id += 1;
                idUnique = _context.BlobItem.ToList().FindIndex(i => i.FileId == id);
                if (idUnique <= 0)
                    break;
            }
            return id;
        }

        public async Task<BlobItem> CreateBlobItem(CreateBlob postData)
        {

            int id = this.GenerateId();
            BlobItem blobItem = BlobItemFactory.Create(postData, id, blobItemsPath);

            if (blobItem == null)
                return null;

            string fileName = Path.GetFileName(blobItem.PathFile);

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
            var oldFile = _context.BlobItem.ToList().Find(
                item => item.FileId.Equals(newFile.FileId));

            if (oldFile == null)
                return false;

            oldFile.FileName = newFile.FileName;
            oldFile.Description = newFile.Description;
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            BlobItem blobItem = _context.BlobItem.ToList().Find(item =>
                   item.FileId.Equals(id));

            if (blobItem == null)
                return false;

            try
            {
                string fileOnCloud = Path.GetFileName(blobItem.PathFile);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileOnCloud);
                await blockBob.DeleteIfExistsAsync();
                _context.BlobItem.ToList().Remove(blobItem);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }

        public FileInformation DownloadFileAssistent(int id)
        {
            BlobItem blobItem = _context.BlobItem.ToList().Find(item =>
                    item.FileId.Equals(id));
            string extension = Path.GetExtension(blobItem.PathFile);

            return new FileInformation
            {
                fileName = Path.GetFileNameWithoutExtension(blobItem.FileName),
                extension = extension
            };
        }

        private async Task<bool> DownloadBlobFileToServer(BlobItem blobItem)
        {
            try
            {
                string fileName = Path.GetFileName(blobItem.PathFile);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                var rootDir = new FileInfo(blobItem.PathFile).Directory;
                if (!rootDir.Exists) //make sure the parent directory exists
                    rootDir.Create();

                await blockBob.DownloadToFileAsync(blobItem.PathFile, FileMode.Create);
                return true;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("File couldn't be downloaded from the Azure!");
                return false;
            }
        }


        public async Task<DownloadFileResponse> DownloadFile(int id)
        {
            BlobItem blobItem = _context.BlobItem.ToList().Find(item =>
                    item.FileId.Equals(id));

            if (blobItem == null)
                return null;

            //download the file to the local server
            await DownloadBlobFileToServer(blobItem);

            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(blobItem.PathFile);
                var content = new System.IO.MemoryStream(data);

                if (System.IO.File.Exists(blobItem.PathFile))
                    System.IO.File.Delete(blobItem.PathFile);

                // var z = File(data, "application/octet-stream", blobItem.fileName);
                return new DownloadFileResponse
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
