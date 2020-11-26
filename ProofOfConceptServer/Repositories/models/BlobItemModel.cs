﻿using Microsoft.WindowsAzure.Storage.Blob;
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
using ProofOfConceptServer.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace ProofOfConceptServer.Repositories.models
{

    public class BlobStorageModel
    {
        private string container;
        private string PathUpload = Path.Combine(Startup.apiRoot, "Repositories/upload");
        public BlobStorageModel()
        {
            container = StorageContext.Environment;
        }

        public async Task<bool> IsFileNameAvailable(string fileName)
        {
            if(StorageContext.Environments[1] == StorageContext.Environment)
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
                return Path.Combine(PathUpload, fileName).ToString();

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
            return Path.Combine(PathUpload, (name + id + e));
        }

        public async Task<bool> Create(string fileName, IFormFile file)
        {
            try
            {
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                using (Stream fileStream = file.OpenReadStream())
                {
                    await blockBob.UploadFromStreamAsync(fileStream);
                }
                return true;
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
                //azure
            }
            else
            {
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                await blockBob.DeleteIfExistsAsync();
            }
        }
        public async Task<bool> DownloadBlobFileToServer(BlobItem blobItem)
        {
            try
            {
                string fileName = Path.GetFileName(blobItem.Path);
                var rootDir = new FileInfo(blobItem.Path).Directory;
                if (!rootDir.Exists) //make sure the parent directory exists
                    rootDir.Create();

                if (StorageContext.Environments[0] == StorageContext.Environment)
                {
                    CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                    await blockBob.DownloadToFileAsync(blobItem.Path, FileMode.Create);
                }
                else
                {
                    //TODO:
                }
                    
                return true;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("File couldn't be downloaded from the Azure!");
                return false;
            }
        }
    }

    public class BlobItemModel
    {
        private woefiedatabaseContext _context;
        private BlobStorageModel Storage;

        public BlobItemModel()
        {
            Storage = new BlobStorageModel();
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

        public bool DeleteBlobItem(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                   item.FileId == (id)).FirstOrDefault();

            if (blobItem == null)
                return false;

            try
            {
                string fileOnCloud = Path.GetFileName(blobItem.Path);
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


        public async Task<IDownloadFileResponse> DownloadFile(int id)
        {
            BlobItem blobItem = _context.BlobItem.Where(item =>
                    item.FileId == id).FirstOrDefault();

            if (blobItem == null)
                return null;

            //download the file to the local server
            bool z = Storage.DownloadBlobFileToServer(blobItem).Result;

            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(blobItem.Path);
                var content = new System.IO.MemoryStream(data);

                //if (System.IO.File.Exists(blobItem.Path))
                //    System.IO.File.Delete(blobItem.Path);

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