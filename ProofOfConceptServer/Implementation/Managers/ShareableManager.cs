using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.Factory;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.interfaces;
using ProofOfConceptServer.Repositories.models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProofOfConceptServer.Repositories.Models
{
    public class ShareableManager
    {
        private woefiedatabaseContext _context;

        public ShareableManager()
        {
            _context = new woefiedatabaseContext();
        }

        private BlobItemManager GetBlobModel()
        {
            return new BlobItemManager();
        }

        private bool DoesBlobIdExists(int blobId)
        {
            return GetBlobModel().GetSingleFile(blobId) != null ? true : false;
        }

        private int CreateId()
        {
            Random rnd = new Random();
            int salt = rnd.Next(1000, 9999);
            int number = 0;
            int id = salt + number;
            bool idUnique = false;
            while (!idUnique)
            {
                id = salt + number;
                idUnique =_context.ShareItems.Where(
                        s => s.Id == id).Count() < 1 ? true : false;
                number++;
            }
            return id;
        }

        public ShareItem Create(int blobId)
        {
            if (!DoesBlobIdExists(blobId))
                return null;
            ShareItem s = ShareItemFactory.Create(CreateId(), blobId);
            _context.ShareItems.Add(s);
            _context.SaveChanges();
            return s;
        }

        public void DeleteShareItem(BlobItem blob)
        {
            ShareItem s = _context.ShareItems.Where(s => s.Id == blob.FileId).FirstOrDefault();
            if (s == null)
                return;
            _context.Remove(s);
            _context.SaveChanges();
        }


        public IShareItemInfo ShareItemInfo(int shareId)
        {
            try
            {
                ShareItem s = GetShareItem(shareId);
                BlobItem b = GetBlobModel().GetSingleFile(s.BlobId);
                return ShareItemInfoFactory.Create(s, b);
            }
            catch
            {
                return null;
            }
                
           
        }
        private ShareItem GetShareItem(int requestId)
        {
            ShareItem share = _context.ShareItems.Where(s => s.Id == requestId).FirstOrDefault();
            if (share == null)
                return null;
            else if (DateTime.Compare(DateTime.Now, share.ActiveUntil) < 0)
                return share;
            else
                return null;           
        }

        public IDownloadFileResponse DownloadFile(int requestId)
        {
            ShareItem s = GetShareItem(requestId);
            if(s == null)
                return null;
            return GetBlobModel().DownloadFile(s.BlobId);
        }

        public IFileInformation DownloadFileAssistent(int requestId)
        {
            ShareItem s = GetShareItem(requestId);
            if (s == null)
                return null;
            return GetBlobModel().DownloadFileAssistent(s.BlobId);
        }
    }
}
