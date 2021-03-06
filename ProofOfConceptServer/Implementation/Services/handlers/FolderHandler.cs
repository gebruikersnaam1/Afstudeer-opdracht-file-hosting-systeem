﻿using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.interfaces;
using ProofOfConceptServer.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Services.handlers
{
    public class FolderHandler
    {
        private FolderModel Model;
        public FolderHandler()
        {
            this.Model = new FolderModel();
        }

        public Folder CreateFolder(ICreateFolder data)
        {
            if (this.Model.GetFolder(data.parentID) == null)
            {
                System.Diagnostics.Debug.WriteLine("Parent class doesn't exist");
                return null;
            }
            return this.Model.CreateFolder(data);
        }

        public bool DoesFolderExist(int folderID)
        {
           return this.Model.DoesFolderExist(folderID);
        }

        public List<IFolderContent> GetFolderContent(int folderID)
        {
            return this.Model.GetFolderContent(folderID);
        }

        public BlobItem CreateFolderBlobItem(ICreateBlob postData, int folderId)
        {
            return this.Model.CreateFolderBlobItem(postData, folderId);
        }

        public List<IFolderContent> SearchForFiles(string searchTerm)
        {
            return this.Model.SearchForFile(searchTerm);
        }

        public Folder GetParentFolder(int folderId)
        {
            Folder f = this.Model.GetFolder(folderId);
            return this.Model.GetFolder((int)f.ParentFolder);
        }

        public IFolderWithParent GetFolderWithParent(int folderId)
        {
            return this.Model.GetFolderWithParent(folderId);
        }

        public Folder ChangeFolderName(IChangeFolder changeFolder){
            return this.Model.ChangeFolderName(changeFolder);
        }

        public bool DeleteFolder(int folderId)
        {
            return this.Model.DeleteFolder(folderId);
        }

        public IFolderStructure GetFolderStructure()
        {
            return this.Model.GetFolderStructure();
        }

        public bool RemoveBlobFromFolders(int blobId)
        {
            return this.Model.RemoveBlobFromFolders(blobId);
        }

        public bool BlobsRemoveFromFolder(int[] blobIds)
        {
            bool result = true;
            bool t;
            foreach(int b in blobIds)
            {
                t = this.Model.RemoveBlobFromFolders(b);
                if (!t)
                    result = t;
            }
            return result;
        }

        public void FilesSynchronization()
        {
            this.Model.SynchronizationFiles();
        }

        public bool MoveBlobToFolder(int blobId, int folderId)
        {
            return this.Model.MoveBlobToFolder(blobId, folderId);
        }

        public BlobItem CopyFileToAnotherFolder(int blobId, int folderId)
        {
            return this.Model.CopyFileToAnotherFolder(blobId, folderId);
        }

        public Folder FindFolderOfBlob(int blobId)
        {
            return this.Model.FindFolderOfBlob(blobId);
        }
    }
}
