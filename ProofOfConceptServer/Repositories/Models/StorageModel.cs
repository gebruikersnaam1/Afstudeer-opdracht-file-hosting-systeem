using Microsoft.WindowsAzure.Storage.Blob;
using ProofOfConceptServer.database;
using ProofOfConceptServer.Repositories.entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using ProofOfConceptServer.Repositories.interfaces;
using ProofOfConceptServer.Repositories.entities.Factory;
namespace ProofOfConceptServer.Repositories.Models
{
    public class StorageModel
    {
        private string PathUpload = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "repositories/uploads");

        public async Task<bool> IsFileNameAvailable(string fileName)
        {
            if (StorageContext.Environments[1] == StorageContext.Environment)
            {
                return false;
            }
            else
            {
                CloudBlobContainer c = AzureConnection.Container;
                return await c.GetBlockBlobReference(fileName).ExistsAsync();
            }
        }

        public string CreatePathFile(string fileName)
        {

            bool fileExist = IsFileNameAvailable(fileName).Result;

            if (!fileExist)
                return fileName;

            string e = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);

            int id = 0;
            while (fileExist)
            {
                id++;
                fileExist = IsFileNameAvailable((name + id + e)).Result;
                if (!fileExist)
                    break;
            }
            return (name + id + e);
        }

        public async Task<bool> Create(string fileName, IFormFile file)
        {
            try
            {
                if (StorageContext.Environments[0].ToString() == StorageContext.Environment)
                {
                    CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                    using (Stream fileStream = file.OpenReadStream())
                    {
                        await blockBob.UploadFromStreamAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false; //amazon
                }
            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine("Error " + e);
                return false;
            }
        }

        public async void Delete(string fileName)
        {
            if (StorageContext.Environments[1].ToString() == StorageContext.Environment)
            {
                //amazon
            }
            else
            {
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                await blockBob.DeleteIfExistsAsync();
            }
        }

        private async Task<string> GetDownloadPath(BlobItem blobItem)
        {
            try
            {
                string path = Path.Combine(PathUpload, blobItem.Path);
                var rootDir = new FileInfo(blobItem.Path).Directory;
                if (!rootDir.Exists) //make sure the parent directory exists
                    rootDir.Create();

                if (StorageContext.Environments[0] == StorageContext.Environment)
                {
                    CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(blobItem.Path);
                    await blockBob.DownloadToFileAsync(path, FileMode.Create);
                }
                else
                {
                    //amazon
                }

                return path;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("File couldn't be downloaded from the Azure!");
                return null;
            }
        }
        public async Task<Byte[]> DownloadBlobFile(BlobItem blobItem)
        {
            try
            {
                string downloadPath = await GetDownloadPath(blobItem);

                var net = new System.Net.WebClient();
                byte[] data = net.DownloadData(downloadPath);

                if (System.IO.File.Exists(downloadPath))
                    System.IO.File.Delete(downloadPath);

                return data;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ISynchronicFiles>> Synchronization()
        {
            List<ISynchronicFiles> f = new List<ISynchronicFiles>();

            if (StorageContext.Environments[0] == StorageContext.Environment)
            {
                BlobContainerClient blockBob = AzureConnection.containerClient;
                await foreach (Azure.Storage.Blobs.Models.BlobItem blobItem in blockBob.GetBlobsAsync())
                {
                    f.Add(SynchronicFilesFactory.Create(
                            blobItem.Properties.CreatedOn.Value.DateTime,
                            blobItem.Name.ToString(),
                            (long)blobItem.Properties.ContentLength
                        ));
                }
            }
            else
            {
                //amazon
            }
            return f;
        }
    }
}
