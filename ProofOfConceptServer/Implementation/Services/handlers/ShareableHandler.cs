using ProofOfConceptServer.entities.interfaces;
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
    public class ShareableHandler
    {
        private ShareableManager Model;
        public ShareableHandler()
        {
            this.Model = new ShareableManager();
        }
        public ShareItem Create(ICreateSharable postData)
        {
            return this.Model.Create(postData.Share);
        }

        public IShareItemInfo ShareItemInfo(int id)
        {
            return this.Model.ShareItemInfo(id);
        }

        public IDownloadFileResponse DownloadFile(int sharedId)
        {
            return this.Model.DownloadFile(sharedId);
        }

        public IFileInformation DownloadFileAssistent(int sharedId)
        {
            return this.Model.DownloadFileAssistent(sharedId);
        }
    }
}
