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
        private static List<BlobEntity> FilesStorage = DummyDataBlobfiles.GetDummyData();
        private static string blobItemsPath = Path.Combine(Startup.apiRoot, "Models/uploads");

        public int RowsCount()
        {
            return FilesStorage.Count();
        }

        public List<BlobEntity> GetPages(int itemsPerPage,int currentPage)
        {
            return FilesStorage.Skip((itemsPerPage * currentPage)).Take(itemsPerPage).ToList();
        }

        public BlobEntity GetSingleFile(string term)
        {
            return FilesStorage.Find(item =>
                    item.fileId.Equals(term, StringComparison.InvariantCultureIgnoreCase));
        }

        private int GenerateId()
        {
            //basic ID -- auto increment, but with a bit more safety
            int id = FilesStorage.Count();
            bool generatedId = false;
            while (!generatedId)
            {
                id += 1;
                if (FilesStorage.Find(i => i.fileId == id.ToString()) == null)
                    generatedId = true;
            }
            return id;
        }

        public async Task<BlobEntity> CreateBlobItem(CreateBlob postData)
        {

            string id = this.GenerateId().ToString();
            BlobEntity blobItem = BlobItemFactory.Create(postData, id, blobItemsPath);

            if (blobItem == null)
                return null;

            string fileName = Path.GetFileName(blobItem.pathFile);

            CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
            using (Stream fileStream = postData.file.OpenReadStream())
            {
                await blockBob.UploadFromStreamAsync(fileStream);
                FilesStorage.Add(blobItem); //set file in the "db"
            }

            return blobItem;
        }

        public List<BlobEntity> SearchFiles(string term)
        {
            return FilesStorage.Where(file =>
                    file.fileName.ToLower().Contains(term.ToLower())
                ).ToList();
        }

        public bool UpdateBlob(BlobEntity newFile)
        {
            var oldFile = FilesStorage.Find(
                item => item.fileId.Equals
                            (newFile.fileId, StringComparison.InvariantCultureIgnoreCase));

            if (oldFile == null)
                return false;

            oldFile.fileName = newFile.fileName;
            oldFile.description = newFile.description;
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var blobItem = FilesStorage.Find(item =>
                   item.fileId.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (blobItem == null)
                return false;

            try
            {
                string fileOnCloud =  Path.GetFileName(blobItem.pathFile);
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

        public FileInformation DownloadFileAssistent(string id)
        {
            var blobItem = FilesStorage.Find(item =>
                    item.fileId.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            string extension = Path.GetExtension(blobItem.pathFile);

            return new FileInformation
            {
                fileName = Path.GetFileNameWithoutExtension(blobItem.fileName),
                extension = extension
            };
        }

        private async Task<bool> DownloadBlobFileToServer(BlobEntity blobItem)
        {
            try
            {
                string fileName = Path.GetFileName(blobItem.pathFile);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                var rootDir = new FileInfo(blobItem.pathFile).Directory;
                if (!rootDir.Exists) //make sure the parent directory exists
                    rootDir.Create();

                await blockBob.DownloadToFileAsync(blobItem.pathFile, FileMode.Create);
                return true;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("File couldn't be downloaded from the Azure!");
                return false;
            }
        }


        public async Task<DownloadFileResponse> DownloadFile(string id)
        {
            BlobEntity blobItem = FilesStorage.Find(item =>
                    item.fileId.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (blobItem == null)
                return null;

            //download the file to the local server
            await DownloadBlobFileToServer(blobItem);

            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(blobItem.pathFile);
                var content = new System.IO.MemoryStream(data);

                if (System.IO.File.Exists(blobItem.pathFile))
                    System.IO.File.Delete(blobItem.pathFile);

                // var z = File(data, "application/octet-stream", blobItem.fileName);
                return new DownloadFileResponse {
                    File = data,
                    FileName = blobItem.fileName
                };
            }
            catch
            {
                return null;
            }
        }
    }
}

