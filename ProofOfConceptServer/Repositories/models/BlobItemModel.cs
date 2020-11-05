using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.entities;
using ProofOfConceptServer.entities.dummy_data;
using ProofOfConceptServer.entities.Factory;
using ProofOfConceptServer.entities.helpers;
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
        private static List<BlobItem> FilesStorage = DummyDataBlobfiles.GetDummyData();
        private static string blobItemsPath = Path.Combine(Startup.apiRoot, "Models/uploads");

        public int RowsCount()
        {
            return FilesStorage.Count();
        }

        public List<BlobItem> GetPages(int itemsPerPage, int currentPage)
        {
            return FilesStorage.Skip((itemsPerPage * currentPage)).Take(itemsPerPage).ToList();
        }

        public BlobItem GetSingleFile(int id)
        {
            return FilesStorage.Find(item =>
                    item.FileId.Equals(id));
        }

        private int GenerateId()
        {
            //basic ID -- auto increment, but with a bit more safety
            int id = FilesStorage.Count();
            bool generatedId = false;
            while (!generatedId)
            {
                id += 1;
                //if (FilesStorage.Find(i => i.FileId == id)
                    generatedId = true;
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
                FilesStorage.Add(blobItem); //set file in the "db"
            }

            return blobItem;
        }

        public List<BlobItem> SearchFiles(string term)
        {
            return FilesStorage.Where(file =>
                    file.FileName.ToLower().Contains(term.ToLower())
                ).ToList();
        }

        public bool UpdateBlob(BlobItem newFile)
        {
            var oldFile = FilesStorage.Find(
                item => item.FileId.Equals(newFile.FileId));

            if (oldFile == null)
                return false;

            oldFile.FileName = newFile.FileName;
            oldFile.Description = newFile.Description;
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            BlobItem blobItem = FilesStorage.Find(item =>
                   item.FileId.Equals(id));

            if (blobItem == null)
                return false;

            try
            {
                string fileOnCloud = Path.GetFileName(blobItem.PathFile);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileOnCloud);
                await blockBob.DeleteIfExistsAsync();
                FilesStorage.Remove(blobItem);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }

        public FileInformation DownloadFileAssistent(int id)
        {
            BlobItem blobItem = FilesStorage.Find(item =>
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
            BlobItem blobItem = FilesStorage.Find(item =>
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
