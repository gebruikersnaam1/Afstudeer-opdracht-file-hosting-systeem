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

        public List<BlobItem> GetPages(int itemsPerPage,int currentPage)
        {
            if (itemsPerPage < 1 || currentPage < 0)
                return null;

            List<BlobItem> b  = this.model.GetPages(itemsPerPage, currentPage);
            if (b.Count == 0)
                return null;

            return b;
        }


        public BlobItem GetSingleFile(int id)
        {
            BlobItem b = this.model.GetSingleFile(id);

            if (b == null)
                return null;
            return b;
        }

        public BlobItem CreateBlobItem(CreateBlob postData)
        {
            return this.model.CreateBlobItem(postData).Result;
        }

        public List<BlobItem> SearchFiles(string term)
        {
            if (term == "" || term == null)
                return null;

            return this.model.SearchFiles(term);
        }

        public bool UpdateBlob(BlobItem newFile)
        {
            return this.model.UpdateBlob(newFile);
        }
        public bool Delete(int id)
        {
            return this.model.Delete(id).Result;
        }

        public FileInformation DownloadFileAssistent(int id)
        {
            return this.model.DownloadFileAssistent(id);
        }
 
        public DownloadFileResponse DownloadFile(int id)
        {
            return this.model.DownloadFile(id).Result;
        }
    }
}
