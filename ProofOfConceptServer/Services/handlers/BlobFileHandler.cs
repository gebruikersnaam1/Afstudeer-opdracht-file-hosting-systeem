using ProofOfConceptServer.entities;
using ProofOfConceptServer.entities.helpers;
using ProofOfConceptServer.Repositories.entities.helpers;
using ProofOfConceptServer.Repositories.models;
using System.Collections.Generic;

namespace ProofOfConceptServer.Services.Handlers
{
    public class BlobFileHandler
    {
        private BlobItemModel model;

        public BlobFileHandler()
        {
            this.model = new BlobItemModel();
        }


        public int GetCountOfRows()
        {
            return this.model.RowsCount();
        }

        public List<BlobEntity> GetPages(int itemsPerPage,int currentPage)
        {
            if (itemsPerPage < 1 || currentPage < 0)
                return null;

            List<BlobEntity> b  = this.model.GetPages(itemsPerPage, currentPage);
            if (b.Count == 0)
                return null;

            return b;
        }


        public  BlobEntity GetSingleFile(string term)
        {
            BlobEntity b = this.model.GetSingleFile(term);

            if (b == null)
                return null;
            return b;
        }

        public BlobEntity CreateBlobItem(CreateBlob postData)
        {
            return this.model.CreateBlobItem(postData).Result;
        }

        public List<BlobEntity> SearchFiles(string term)
        {
            if (term == "" || term == null)
                return null;

            return this.model.SearchFiles(term);
        }

        public bool UpdateBlob(BlobEntity newFile)
        {
            return this.model.UpdateBlob(newFile);
        }
        public bool Delete(string id)
        {
            return this.model.Delete(id).Result;
        }

        public FileInformation DownloadFileAssistent(string id)
        {
            return this.model.DownloadFileAssistent(id);
        }
 
        public DownloadFileResponse DownloadFile(string id)
        {
            return this.model.DownloadFile(id).Result;
        }
    }
}
