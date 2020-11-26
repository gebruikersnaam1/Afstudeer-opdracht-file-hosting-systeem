using ProofOfConceptServer.entities;
using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.models;
using System.Collections.Generic;

namespace ProofOfConceptServer.Services.Handlers
{
    public class BlobFileHandler
    {
        private BlobItemModel model;

        public BlobFileHandler()
        {
            model = new BlobItemModel();
        }


        public int GetCountOfRows()
        {
            return model.RowsCount();
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

        public BlobItem CreateBlobItem(ICreateBlob postData)
        {
            return model.CreateBlobItem(postData);
        }

        public List<BlobItem> SearchFiles(string term)
        {
            if (term == "" || term == null)
                return null;

            return model.SearchFiles(term);
        }

        public bool UpdateBlob(BlobItem newFile)
        {
            return model.UpdateBlob(newFile);
        }

        public IFileInformation DownloadFileAssistent(int id)
        {
            return model.DownloadFileAssistent(id);
        }
 
        public IDownloadFileResponse DownloadFile(int id)
        {
            return model.DownloadFile(id).Result;
        }
    }
}
